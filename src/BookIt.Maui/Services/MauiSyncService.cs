using BookIt.Core.DTOs;
using SQLite;

namespace BookIt.Maui.Services;

/// <summary>
/// Syncs appointment data from the API to a local SQLite database for offline access.
/// </summary>
public class MauiSyncService
{
    private readonly SQLiteAsyncConnection _db;

    public MauiSyncService()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "bookit.db");
        _db = new SQLiteAsyncConnection(dbPath);
        _ = InitAsync();
    }

    private async Task InitAsync()
    {
        await _db.CreateTableAsync<LocalAppointment>();
    }

    /// <summary>Upserts the given appointments into local SQLite storage.</summary>
    public async Task SyncAppointmentsAsync(IEnumerable<AppointmentResponse> appointments)
    {
        foreach (var apt in appointments)
        {
            var local = new LocalAppointment
            {
                Id = apt.Id.ToString(),
                CustomerName = apt.CustomerName,
                CustomerEmail = apt.CustomerEmail,
                StartTime = apt.StartTime,
                EndTime = apt.EndTime,
                Status = apt.Status.ToString(),
                TotalAmount = apt.TotalAmount,
                BookingPin = apt.BookingPin,
                ServicesJson = System.Text.Json.JsonSerializer.Serialize(apt.Services),
                SyncedAt = DateTime.UtcNow
            };
            await _db.InsertOrReplaceAsync(local);
        }
    }

    /// <summary>Returns all cached appointments from SQLite (for offline use).</summary>
    public Task<List<LocalAppointment>> GetCachedAppointmentsAsync() =>
        _db.Table<LocalAppointment>().OrderByDescending(a => a.StartTime).ToListAsync();
}

[Table("Appointments")]
public class LocalAppointment
{
    [PrimaryKey]
    public string Id { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string? BookingPin { get; set; }
    public string? ServicesJson { get; set; }
    public DateTime SyncedAt { get; set; }
}
