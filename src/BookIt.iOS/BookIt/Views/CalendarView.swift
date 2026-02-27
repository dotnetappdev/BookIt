import SwiftUI

/// Month calendar — shows which days have appointments and lets the user
/// tap a day to see that day's bookings.  Mirrors `CalendarPage.razor`.
struct CalendarView: View {

    @EnvironmentObject var authStore: AuthStore

    @State private var selectedDate    = Date()
    @State private var displayedMonth  = Date()          // first day of the shown month
    @State private var appointments: [AppointmentResponse] = []
    @State private var isLoading = false
    @State private var error: String?

    private var calendar = Calendar.current

    // Days that have at least one appointment
    private var busyDays: Set<DateComponents> {
        Set(appointments.map {
            calendar.dateComponents([.year, .month, .day], from: $0.startTime)
        })
    }

    private var selectedDayAppointments: [AppointmentResponse] {
        appointments.filter { calendar.isDate($0.startTime, inSameDayAs: selectedDate) }
            .sorted { $0.startTime < $1.startTime }
    }

    var body: some View {
        NavigationView {
            ZStack {
                Color(hex: "1a1a1a").ignoresSafeArea()

                ScrollView {
                    VStack(spacing: 0) {
                        monthCalendar
                            .padding()

                        Divider()
                            .background(Color(hex: "3a3a3a"))

                        // Selected day appointments
                        VStack(alignment: .leading, spacing: 12) {
                            Text(selectedDate, style: .date)
                                .font(.headline)
                                .foregroundColor(.white)
                                .padding(.horizontal)
                                .padding(.top, 16)

                            if isLoading {
                                HStack { Spacer(); ProgressView().tint(Color(hex: "a29bfe")); Spacer() }
                                    .padding()
                            } else if selectedDayAppointments.isEmpty {
                                EmptyStateView(
                                    icon: "calendar.badge.minus",
                                    title: "No bookings",
                                    subtitle: "No appointments on this day"
                                )
                            } else {
                                ForEach(selectedDayAppointments) { apt in
                                    AppointmentRowView(appointment: apt)
                                        .padding(.horizontal)
                                }
                            }
                        }
                        Spacer(minLength: 30)
                    }
                }
                .refreshable { await loadRange() }
            }
            .navigationTitle("Calendar")
            .navigationBarTitleDisplayMode(.inline)
            .toolbarColorScheme(.dark, for: .navigationBar)
            .task { await loadRange() }
        }
    }

    // MARK: - Month calendar grid

    private var monthCalendar: some View {
        VStack(spacing: 12) {

            // Month navigation header
            HStack {
                Button { changeMonth(by: -1) } label: {
                    Image(systemName: "chevron.left")
                        .foregroundColor(Color(hex: "a29bfe"))
                }
                Spacer()
                Text(monthYearString(from: displayedMonth))
                    .font(.title3).fontWeight(.bold).foregroundColor(.white)
                Spacer()
                Button { changeMonth(by: 1) } label: {
                    Image(systemName: "chevron.right")
                        .foregroundColor(Color(hex: "a29bfe"))
                }
            }

            // Day-of-week labels
            let dayLabels = ["Mo", "Tu", "Wed", "Th", "Fr", "Sa", "Su"]
            HStack {
                ForEach(dayLabels, id: \.self) { d in
                    Text(d)
                        .font(.caption).fontWeight(.medium)
                        .foregroundColor(Color(hex: "9e9e9e"))
                        .frame(maxWidth: .infinity)
                }
            }

            // Calendar grid
            let days = daysInMonth(for: displayedMonth)
            let columns = Array(repeating: GridItem(.flexible(), spacing: 0), count: 7)
            LazyVGrid(columns: columns, spacing: 4) {
                ForEach(days, id: \.self) { day in
                    if let day {
                        let comps = calendar.dateComponents([.year, .month, .day], from: day)
                        let hasBooking = busyDays.contains(comps)
                        let isSelected = calendar.isDate(day, inSameDayAs: selectedDate)
                        let isToday    = calendar.isDateInToday(day)

                        Button {
                            selectedDate = day
                        } label: {
                            ZStack {
                                if isSelected {
                                    Circle()
                                        .fill(Color(hex: "6c5ce7"))
                                        .frame(width: 34, height: 34)
                                } else if isToday {
                                    Circle()
                                        .stroke(Color(hex: "6c5ce7"), lineWidth: 1.5)
                                        .frame(width: 34, height: 34)
                                }

                                VStack(spacing: 2) {
                                    Text("\(calendar.component(.day, from: day))")
                                        .font(.system(size: 14, weight: isToday || isSelected ? .bold : .regular))
                                        .foregroundColor(isSelected ? .white : (isToday ? Color(hex: "a29bfe") : .white))

                                    if hasBooking {
                                        Circle()
                                            .fill(isSelected ? .white : Color(hex: "a29bfe"))
                                            .frame(width: 5, height: 5)
                                    } else {
                                        Circle().fill(.clear).frame(width: 5, height: 5)
                                    }
                                }
                            }
                            .frame(height: 44)
                        }
                    } else {
                        Color.clear.frame(height: 44)
                    }
                }
            }
        }
    }

    // MARK: - Helpers

    private func changeMonth(by months: Int) {
        if let newMonth = calendar.date(byAdding: .month, value: months, to: displayedMonth) {
            displayedMonth = newMonth
            Task { await loadRange() }
        }
    }

    private func loadRange() async {
        guard !authStore.tenantSlug.isEmpty else { return }
        isLoading = true
        defer { isLoading = false }
        do {
            let start = firstDayOfMonth(displayedMonth)
            let end   = calendar.date(byAdding: .month, value: 1, to: start) ?? start
            appointments = try await BookItAPIService.shared.getAppointments(
                tenantSlug: authStore.tenantSlug,
                from: start,
                to: end
            )
        } catch {
            self.error = error.localizedDescription
        }
    }

    private func firstDayOfMonth(_ date: Date) -> Date {
        calendar.date(from: calendar.dateComponents([.year, .month], from: date)) ?? date
    }

    /// Returns an array of optional Dates for each cell in a Mon-start calendar grid.
    private func daysInMonth(for month: Date) -> [Date?] {
        let start = firstDayOfMonth(month)
        guard let range = calendar.range(of: .day, in: .month, for: start) else { return [] }

        // weekday: 1=Sun, 2=Mon … 7=Sat  → convert to Mon=0 offset
        var weekday = calendar.component(.weekday, from: start)
        weekday = (weekday + 5) % 7  // Sun→6, Mon→0, Tue→1 …

        var cells: [Date?] = Array(repeating: nil, count: weekday)
        for day in range {
            if let date = calendar.date(byAdding: .day, value: day - 1, to: start) {
                cells.append(date)
            }
        }
        // Pad to complete the last row
        while cells.count % 7 != 0 { cells.append(nil) }
        return cells
    }

    private func monthYearString(from date: Date) -> String {
        let fmt = DateFormatter()
        fmt.dateFormat = "MMMM yyyy"
        return fmt.string(from: date)
    }
}
