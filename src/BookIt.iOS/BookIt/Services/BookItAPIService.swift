import Foundation

// MARK: - BookIt API Service

/// Async/await HTTP client for the BookIt REST API.
/// Mirror of the C# `BookItApiService` in `BookIt.UI.Shared`.
actor BookItAPIService {

    static let shared = BookItAPIService()

    private let baseURL = URL(string: "https://api.bookit.app")!
    private var accessToken: String?

    private let decoder: JSONDecoder = {
        let d = JSONDecoder()
        // ASP.NET Core emits camelCase JSON by default; Swift properties are camelCase.
        // No key conversion needed — the names match directly.
        // Dates come as ISO-8601 strings from ASP.NET Core
        let iso = ISO8601DateFormatter()
        iso.formatOptions = [.withInternetDateTime, .withFractionalSeconds]
        d.dateDecodingStrategy = .custom { decoder in
            let str = try decoder.singleValueContainer().decode(String.self)
            if let date = iso.date(from: str) { return date }
            // fallback without fractional seconds
            let iso2 = ISO8601DateFormatter()
            iso2.formatOptions = [.withInternetDateTime]
            if let date = iso2.date(from: str) { return date }
            throw DecodingError.dataCorruptedError(
                in: try decoder.singleValueContainer(),
                debugDescription: "Cannot decode date: \(str)"
            )
        }
        return d
    }()

    private let encoder: JSONEncoder = {
        let e = JSONEncoder()
        // Use default key encoding; ASP.NET Core model binding is case-insensitive
        return e
    }()

    // MARK: Token management

    func setToken(_ token: String) {
        accessToken = token
    }

    func clearToken() {
        accessToken = nil
    }

    // MARK: - Auth

    func login(email: String, password: String) async throws -> AuthResponse {
        struct Body: Encodable { let email: String; let password: String }
        return try await post(path: "/api/auth/login", body: Body(email: email, password: password))
    }

    func register(
        email: String,
        password: String,
        firstName: String,
        lastName: String,
        membershipNumber: String?
    ) async throws -> AuthResponse {
        struct Body: Encodable {
            let email: String
            let password: String
            let firstName: String
            let lastName: String
            let membershipNumber: String?
        }
        return try await post(
            path: "/api/auth/register",
            body: Body(
                email: email,
                password: password,
                firstName: firstName,
                lastName: lastName,
                membershipNumber: membershipNumber
            )
        )
    }

    // MARK: - Tenant

    func getTenant(slug: String) async throws -> TenantResponse {
        return try await get(path: "/api/tenants/\(slug)")
    }

    // MARK: - Appointments

    func getAppointments(
        tenantSlug: String,
        from: Date,
        to: Date
    ) async throws -> [AppointmentResponse] {
        let fmt = DateFormatter()
        fmt.dateFormat = "yyyy-MM-dd'T'HH:mm:ss"
        let fromStr = fmt.string(from: from)
        let toStr = fmt.string(from: to)
        let path = "/api/tenants/\(tenantSlug)/appointments?from=\(fromStr)&to=\(toStr)"
        return try await get(path: path)
    }

    // MARK: - Generic HTTP helpers

    private func get<T: Decodable>(path: String) async throws -> T {
        guard let url = URL(string: path, relativeTo: baseURL)?.absoluteURL else {
            throw APIError.invalidResponse
        }
        var request = URLRequest(url: url)
        request.httpMethod = "GET"
        setAuth(on: &request)
        let (data, response) = try await URLSession.shared.data(for: request)
        try validateResponse(response, data: data)
        return try decoder.decode(T.self, from: data)
    }

    private func post<Body: Encodable, T: Decodable>(path: String, body: Body) async throws -> T {
        guard let url = URL(string: path, relativeTo: baseURL)?.absoluteURL else {
            throw APIError.invalidResponse
        }
        var request = URLRequest(url: url)
        request.httpMethod = "POST"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        setAuth(on: &request)
        request.httpBody = try encoder.encode(body)
        let (data, response) = try await URLSession.shared.data(for: request)
        try validateResponse(response, data: data)
        return try decoder.decode(T.self, from: data)
    }

    private func setAuth(on request: inout URLRequest) {
        if let token = accessToken {
            request.setValue("Bearer \(token)", forHTTPHeaderField: "Authorization")
        }
    }

    private func validateResponse(_ response: URLResponse, data: Data) throws {
        guard let http = response as? HTTPURLResponse else { return }
        guard (200..<300).contains(http.statusCode) else {
            let body = String(data: data, encoding: .utf8) ?? ""
            throw APIError.httpError(statusCode: http.statusCode, body: body)
        }
    }
}

// MARK: - API Error

enum APIError: LocalizedError {
    case httpError(statusCode: Int, body: String)
    case invalidResponse

    var errorDescription: String? {
        switch self {
        case .httpError(let code, let body):
            if code == 401 { return "Invalid credentials. Please check your email and password." }
            if code == 400 { return "Bad request: \(body)" }
            return "Server error (\(code))."
        case .invalidResponse:
            return "Unexpected response from the server."
        }
    }
}
