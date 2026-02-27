package com.bookit.app.services

import com.bookit.app.models.*
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import kotlinx.serialization.json.Json
import kotlinx.serialization.json.JsonNull
import kotlinx.serialization.json.JsonPrimitive
import kotlinx.serialization.json.JsonElement
import kotlinx.serialization.encodeToString
import kotlinx.serialization.serializer
import java.net.HttpURLConnection
import java.net.URL
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter

/**
 * REST client for the BookIt API.
 * Uses HttpURLConnection + kotlinx.serialization — no external HTTP library needed.
 *
 * Thread-safety: all token mutations and HTTP calls are dispatched to [Dispatchers.IO].
 */
object BookItApiService {

    private const val BASE_URL = "https://api.bookit.app"

    @Volatile private var accessToken: String? = null

    private val json = Json {
        ignoreUnknownKeys = true
        coerceInputValues = true
    }

    // -------------------------------------------------------------------------
    // Token management
    // -------------------------------------------------------------------------

    fun setToken(token: String) { accessToken = token }
    fun clearToken()            { accessToken = null  }

    // -------------------------------------------------------------------------
    // Auth
    // -------------------------------------------------------------------------

    suspend fun login(email: String, password: String): AuthResponse =
        post("/api/auth/login", mapOf("email" to email, "password" to password))

    suspend fun register(
        email: String,
        password: String,
        firstName: String,
        lastName: String,
        membershipNumber: String?
    ): AuthResponse {
        val body = buildMap<String, Any?> {
            put("email", email)
            put("password", password)
            put("firstName", firstName)
            put("lastName", lastName)
            put("membershipNumber", membershipNumber)
        }
        return post("/api/auth/register", body)
    }

    // -------------------------------------------------------------------------
    // Tenant
    // -------------------------------------------------------------------------

    suspend fun getTenant(slug: String): TenantResponse =
        get("/api/tenants/$slug")

    // -------------------------------------------------------------------------
    // Appointments
    // -------------------------------------------------------------------------

    suspend fun getAppointments(
        tenantSlug: String,
        from: LocalDateTime,
        to: LocalDateTime
    ): List<AppointmentResponse> {
        val fmt = DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss")
        val path = "/api/tenants/$tenantSlug/appointments?from=${from.format(fmt)}&to=${to.format(fmt)}"
        return get(path)
    }

    // -------------------------------------------------------------------------
    // Generic HTTP helpers
    // -------------------------------------------------------------------------

    private suspend inline fun <reified T> get(path: String): T = withContext(Dispatchers.IO) {
        val conn = openConnection(path, "GET")
        val body = conn.inputStream.bufferedReader().readText()
        conn.disconnect()
        json.decodeFromString(body)
    }

    private suspend inline fun <reified T> post(path: String, payload: Map<String, Any?>): T =
        withContext(Dispatchers.IO) {
            val conn = openConnection(path, "POST")
            conn.doOutput = true
            conn.setRequestProperty("Content-Type", "application/json")
            // Encode via kotlinx.serialization to avoid manual escaping issues
            val bodyJson = json.encodeToString(
                serializer<Map<String, JsonElement>>(),
                payload.mapValues { (_, v) ->
                    when (v) {
                        null       -> JsonNull
                        is String  -> JsonPrimitive(v)
                        is Number  -> JsonPrimitive(v)
                        is Boolean -> JsonPrimitive(v)
                        else       -> JsonPrimitive(v.toString())
                    }
                }
            )
            java.io.OutputStreamWriter(conn.outputStream).use { it.write(bodyJson) }
            val statusCode = conn.responseCode
            if (statusCode !in 200..299) {
                val errBody = conn.errorStream?.bufferedReader()?.readText() ?: ""
                conn.disconnect()
                throw ApiException(statusCode, errBody)
            }
            val body = conn.inputStream.bufferedReader().readText()
            conn.disconnect()
            json.decodeFromString(body)
        }

    private fun openConnection(path: String, method: String): HttpURLConnection {
        val url = URL("$BASE_URL$path")
        val conn = url.openConnection() as HttpURLConnection
        conn.requestMethod = method
        conn.setRequestProperty("Accept", "application/json")
        accessToken?.let { conn.setRequestProperty("Authorization", "Bearer $it") }
        conn.connectTimeout = 15_000
        conn.readTimeout    = 30_000
        return conn
    }
}

// ---------------------------------------------------------------------------
// Exception
// ---------------------------------------------------------------------------

class ApiException(val statusCode: Int, val body: String) : Exception() {
    override val message: String
        get() = when (statusCode) {
            401  -> "Invalid credentials. Please check your email and password."
            400  -> "Bad request: $body"
            else -> "Server error ($statusCode)."
        }
}
