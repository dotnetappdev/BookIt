namespace BookIt.Notifications.Email;

/// <summary>
/// HTML email templates for booking notifications.
/// </summary>
internal static class EmailTemplates
{
    public static string BookingConfirmation(
        string customerName,
        string businessName,
        string serviceName,
        DateTime start,
        DateTime end,
        string? location,
        string? meetingLink,
        string? bookingPin)
    {
        var locationHtml = !string.IsNullOrEmpty(meetingLink)
            ? $"<p><strong>Meeting Link:</strong> <a href=\"{meetingLink}\">{meetingLink}</a></p>"
            : !string.IsNullOrEmpty(location)
                ? $"<p><strong>Location:</strong> {location}</p>"
                : "";

        var pinHtml = !string.IsNullOrEmpty(bookingPin)
            ? $@"<div style=""margin:20px 0;padding:16px;background:#f0eeff;border-radius:8px;text-align:center;"">
                   <div style=""font-size:12px;color:#6c5ce7;font-weight:600;text-transform:uppercase;letter-spacing:1px;"">Your Booking PIN</div>
                   <div style=""font-size:32px;font-weight:800;color:#2d3748;letter-spacing:6px;margin-top:8px;"">{bookingPin}</div>
                   <div style=""font-size:12px;color:#718096;margin-top:4px;"">Show this PIN when you arrive</div>
                 </div>"
            : "";

        return $@"<!DOCTYPE html>
<html>
<head><meta charset=""utf-8""><meta name=""viewport"" content=""width=device-width,initial-scale=1""></head>
<body style=""margin:0;padding:0;background:#f7f8fc;font-family:-apple-system,BlinkMacSystemFont,'Segoe UI',sans-serif;"">
  <div style=""max-width:600px;margin:32px auto;background:white;border-radius:12px;overflow:hidden;box-shadow:0 2px 8px rgba(0,0,0,.08);"">
    <div style=""background:linear-gradient(135deg,#6c5ce7,#a29bfe);padding:32px 40px;"">
      <div style=""font-size:24px;font-weight:800;color:white;"">{businessName}</div>
      <div style=""font-size:14px;color:rgba(255,255,255,.8);margin-top:4px;"">Booking Confirmation ✅</div>
    </div>
    <div style=""padding:32px 40px;"">
      <p style=""font-size:16px;color:#2d3748;"">Hi {customerName},</p>
      <p style=""color:#4a5568;"">Your booking has been confirmed. Here are your appointment details:</p>
      <div style=""background:#f7f8fc;border-radius:8px;padding:20px;margin:24px 0;"">
        <table style=""width:100%;border-collapse:collapse;"">
          <tr><td style=""padding:8px 0;color:#718096;font-size:14px;width:120px;"">Service</td>
              <td style=""padding:8px 0;color:#2d3748;font-weight:600;"">{serviceName}</td></tr>
          <tr><td style=""padding:8px 0;color:#718096;font-size:14px;"">Date</td>
              <td style=""padding:8px 0;color:#2d3748;font-weight:600;"">{start:dddd, d MMMM yyyy}</td></tr>
          <tr><td style=""padding:8px 0;color:#718096;font-size:14px;"">Time</td>
              <td style=""padding:8px 0;color:#2d3748;font-weight:600;"">{start:h:mm tt} – {end:h:mm tt}</td></tr>
        </table>
      </div>
      {locationHtml}
      {pinHtml}
      <p style=""color:#718096;font-size:14px;"">If you need to change or cancel your appointment, please contact us.</p>
    </div>
    <div style=""padding:20px 40px;background:#f7f8fc;border-top:1px solid #eee;text-align:center;font-size:12px;color:#a0aec0;"">
      Powered by <strong>BookIt</strong> — The Smart Booking Platform
    </div>
  </div>
</body>
</html>";
    }

    public static string AppointmentReminder(
        string customerName,
        string businessName,
        string serviceName,
        DateTime start,
        int minutesBefore,
        string? location,
        string? meetingLink)
    {
        var whenText = minutesBefore switch
        {
            <= 60 => $"{minutesBefore} minutes",
            <= 120 => "1 hour",
            < 1440 => $"{minutesBefore / 60} hours",
            1440 => "1 day",
            _ => $"{minutesBefore / 1440} days"
        };

        var locationHtml = !string.IsNullOrEmpty(meetingLink)
            ? $"<p><a href=\"{meetingLink}\" style=\"color:#6c5ce7;\">Join Meeting</a></p>"
            : !string.IsNullOrEmpty(location)
                ? $"<p><strong>Location:</strong> {location}</p>"
                : "";

        return $@"<!DOCTYPE html>
<html>
<head><meta charset=""utf-8""><meta name=""viewport"" content=""width=device-width,initial-scale=1""></head>
<body style=""margin:0;padding:0;background:#f7f8fc;font-family:-apple-system,BlinkMacSystemFont,'Segoe UI',sans-serif;"">
  <div style=""max-width:600px;margin:32px auto;background:white;border-radius:12px;overflow:hidden;box-shadow:0 2px 8px rgba(0,0,0,.08);"">
    <div style=""background:linear-gradient(135deg,#fd79a8,#e17055);padding:32px 40px;"">
      <div style=""font-size:24px;font-weight:800;color:white;"">{businessName}</div>
      <div style=""font-size:14px;color:rgba(255,255,255,.8);margin-top:4px;"">⏰ Appointment Reminder</div>
    </div>
    <div style=""padding:32px 40px;"">
      <p style=""font-size:16px;color:#2d3748;"">Hi {customerName},</p>
      <p style=""color:#4a5568;"">This is a reminder that your appointment is in <strong>{whenText}</strong>.</p>
      <div style=""background:#fff3f3;border-left:4px solid #e17055;border-radius:0 8px 8px 0;padding:16px 20px;margin:20px 0;"">
        <div style=""font-weight:700;color:#2d3748;margin-bottom:8px;"">{serviceName}</div>
        <div style=""color:#4a5568;font-size:14px;"">{start:dddd, d MMMM yyyy} at {start:h:mm tt}</div>
      </div>
      {locationHtml}
    </div>
    <div style=""padding:20px 40px;background:#f7f8fc;border-top:1px solid #eee;text-align:center;font-size:12px;color:#a0aec0;"">
      Powered by <strong>BookIt</strong> — The Smart Booking Platform
    </div>
  </div>
</body>
</html>";
    }

    public static string BookingCancellation(
        string customerName,
        string businessName,
        string serviceName,
        DateTime start,
        string? reason)
    {
        var reasonHtml = !string.IsNullOrEmpty(reason)
            ? $"<p style=\"color:#4a5568;\"><strong>Reason:</strong> {reason}</p>"
            : "";

        return $@"<!DOCTYPE html>
<html>
<head><meta charset=""utf-8""><meta name=""viewport"" content=""width=device-width,initial-scale=1""></head>
<body style=""margin:0;padding:0;background:#f7f8fc;font-family:-apple-system,BlinkMacSystemFont,'Segoe UI',sans-serif;"">
  <div style=""max-width:600px;margin:32px auto;background:white;border-radius:12px;overflow:hidden;box-shadow:0 2px 8px rgba(0,0,0,.08);"">
    <div style=""background:linear-gradient(135deg,#636e72,#2d3436);padding:32px 40px;"">
      <div style=""font-size:24px;font-weight:800;color:white;"">{businessName}</div>
      <div style=""font-size:14px;color:rgba(255,255,255,.8);margin-top:4px;"">❌ Booking Cancelled</div>
    </div>
    <div style=""padding:32px 40px;"">
      <p style=""font-size:16px;color:#2d3748;"">Hi {customerName},</p>
      <p style=""color:#4a5568;"">Your appointment has been cancelled.</p>
      <div style=""background:#f7f8fc;border-radius:8px;padding:16px 20px;margin:20px 0;"">
        <div style=""font-weight:700;color:#2d3748;margin-bottom:4px;"">{serviceName}</div>
        <div style=""color:#718096;font-size:14px;"">{start:dddd, d MMMM yyyy} at {start:h:mm tt}</div>
      </div>
      {reasonHtml}
      <p style=""color:#718096;font-size:14px;"">If you believe this is an error, please contact us.</p>
    </div>
    <div style=""padding:20px 40px;background:#f7f8fc;border-top:1px solid #eee;text-align:center;font-size:12px;color:#a0aec0;"">
      Powered by <strong>BookIt</strong> — The Smart Booking Platform
    </div>
  </div>
</body>
</html>";
    }
}
