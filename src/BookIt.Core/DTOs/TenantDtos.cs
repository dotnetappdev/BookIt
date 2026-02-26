using BookIt.Core.Enums;

namespace BookIt.Core.DTOs;

public class TenantResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public BusinessType BusinessType { get; set; }
    public string? LogoUrl { get; set; }
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostCode { get; set; }
    public string? Country { get; set; }
    public string? Website { get; set; }
    public string? TimeZone { get; set; }
    public string? Currency { get; set; }
    public decimal VatRate { get; set; }
    public bool AllowOnlineBooking { get; set; }
    public bool RequirePaymentUpfront { get; set; }
    public bool AllowPayAtShop { get; set; }
    public bool AllowPayInCash { get; set; }
    public bool EnableStripe { get; set; }
    public bool EnablePayPal { get; set; }
    public bool EnableApplePay { get; set; }
    public string? StripePublishableKey { get; set; }
    public string? PayPalClientId { get; set; }
    public TenantTheme Theme { get; set; } = TenantTheme.Indigo;
    public string? BookingPageTitle { get; set; }
    public string? BannerImageUrl { get; set; }
    // File upload
    public string? AllowedFileExtensions { get; set; }  // e.g. ".pdf,.doc,.docx"
    // Accommodation (B&B / Hotel)
    public int? TotalRooms { get; set; }
    public string? AmenitiesJson { get; set; }  // JSON array of amenity strings
    // SMS notifications
    public SmsProvider SmsProvider { get; set; } = SmsProvider.None;
    public string? ClickSendUsername { get; set; }
    public string? ClickSendFromNumber { get; set; }
    public string? TwilioAccountSid { get; set; }
    public string? TwilioFromNumber { get; set; }
    public bool EnableSmsNotifications { get; set; }
    // SendGrid email notifications
    public string? SendGridFromEmail { get; set; }
    public string? SendGridFromName { get; set; }
    public bool EnableEmailNotifications { get; set; }
    // Reminder settings
    public string? ReminderAlerts { get; set; }
    public bool EnableEmailReminders { get; set; }
    public bool EnableSmsReminders { get; set; }
}

public class UpdateTenantRequest
{
    public string Name { get; set; } = string.Empty;
    public BusinessType BusinessType { get; set; }
    public string? LogoUrl { get; set; }
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostCode { get; set; }
    public string? Country { get; set; }
    public string? Website { get; set; }
    public string? TimeZone { get; set; }
    public string? Currency { get; set; }
    public decimal VatRate { get; set; }
    public bool AllowOnlineBooking { get; set; }
    public bool RequirePaymentUpfront { get; set; }
    public bool AllowPayAtShop { get; set; }
    public bool AllowPayInCash { get; set; }
    public bool SendReminders { get; set; }
    public int ReminderHoursBefore { get; set; }
    public bool EnableStripe { get; set; }
    public bool EnablePayPal { get; set; }
    public bool EnableApplePay { get; set; }
    public string? StripePublishableKey { get; set; }
    public string? StripeSecretKey { get; set; }
    public string? PayPalClientId { get; set; }
    public string? PayPalClientSecret { get; set; }
    public string? AllowedEmbedDomains { get; set; }
    public string? CustomCss { get; set; }
    public string? DefaultMeetingLink { get; set; }
    public string? OpenAiApiKey { get; set; }
    public string? ElevenLabsApiKey { get; set; }
    public string? ElevenLabsVoiceId { get; set; }
    public string? VapiPublicKey { get; set; }
    public bool EnableAiChat { get; set; } = true;
    public TenantTheme Theme { get; set; } = TenantTheme.Indigo;
    public string? BookingPageTitle { get; set; }
    public string? BannerImageUrl { get; set; }
    // File upload
    public string? AllowedFileExtensions { get; set; }  // comma-separated, e.g. ".pdf,.doc,.docx"
    // Accommodation (B&B / Hotel)
    public int? TotalRooms { get; set; }
    public string? AmenitiesJson { get; set; }
    // SMS notifications
    public SmsProvider SmsProvider { get; set; } = SmsProvider.None;
    public string? ClickSendUsername { get; set; }
    public string? ClickSendApiKey { get; set; }
    public string? ClickSendFromNumber { get; set; }
    public string? TwilioAccountSid { get; set; }
    public string? TwilioAuthToken { get; set; }
    public string? TwilioFromNumber { get; set; }
    public bool EnableSmsNotifications { get; set; }
    // SendGrid email notifications
    public string? SendGridApiKey { get; set; }
    public string? SendGridFromEmail { get; set; }
    public string? SendGridFromName { get; set; }
    public bool EnableEmailNotifications { get; set; }
    // Reminder alerts
    public string? ReminderAlerts { get; set; }
    public bool EnableEmailReminders { get; set; }
    public bool EnableSmsReminders { get; set; }
}
