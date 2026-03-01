using System.ComponentModel.DataAnnotations;
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
    public string? AllowedFileExtensions { get; set; }
    // Accommodation (B&B / Hotel)
    public int? TotalRooms { get; set; }
    public string? AmenitiesJson { get; set; }
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
    public bool EnableSoftDelete { get; set; } = true;
    // Subdomain routing
    public string? Subdomain { get; set; }
    public bool SubdomainApproved { get; set; }
}

public class UpdateTenantRequest
{
    [Required(ErrorMessage = "Business name is required.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Business name must be between 2 and 200 characters.")]
    public string Name { get; set; } = string.Empty;

    public BusinessType BusinessType { get; set; }

    [Url(ErrorMessage = "Logo URL must be a valid URL.")]
    [StringLength(2000, ErrorMessage = "Logo URL must not exceed 2000 characters.")]
    public string? LogoUrl { get; set; }

    [StringLength(20, ErrorMessage = "Primary colour must not exceed 20 characters.")]
    public string? PrimaryColor { get; set; }

    [StringLength(20, ErrorMessage = "Secondary colour must not exceed 20 characters.")]
    public string? SecondaryColor { get; set; }

    [EmailAddress(ErrorMessage = "A valid contact email is required.")]
    [StringLength(254, ErrorMessage = "Contact email must not exceed 254 characters.")]
    public string? ContactEmail { get; set; }

    [Phone(ErrorMessage = "A valid contact phone number is required.")]
    [StringLength(30, ErrorMessage = "Contact phone must not exceed 30 characters.")]
    public string? ContactPhone { get; set; }

    [StringLength(500, ErrorMessage = "Address must not exceed 500 characters.")]
    public string? Address { get; set; }

    [StringLength(100, ErrorMessage = "City must not exceed 100 characters.")]
    public string? City { get; set; }

    [StringLength(20, ErrorMessage = "Post code must not exceed 20 characters.")]
    public string? PostCode { get; set; }

    [StringLength(100, ErrorMessage = "Country must not exceed 100 characters.")]
    public string? Country { get; set; }

    [Url(ErrorMessage = "Website must be a valid URL.")]
    [StringLength(2000, ErrorMessage = "Website must not exceed 2000 characters.")]
    public string? Website { get; set; }

    public string? TimeZone { get; set; }
    public string? Currency { get; set; }

    [Range(0, 100, ErrorMessage = "VAT rate must be between 0 and 100.")]
    public decimal VatRate { get; set; }

    public bool AllowOnlineBooking { get; set; }
    public bool RequirePaymentUpfront { get; set; }
    public bool AllowPayAtShop { get; set; }
    public bool AllowPayInCash { get; set; }
    public bool SendReminders { get; set; }

    [Range(0, 8760, ErrorMessage = "Reminder hours must be between 0 and 8760.")]
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

    [Url(ErrorMessage = "Default meeting link must be a valid URL.")]
    [StringLength(2000, ErrorMessage = "Default meeting link must not exceed 2000 characters.")]
    public string? DefaultMeetingLink { get; set; }

    public string? OpenAiApiKey { get; set; }
    public string? ElevenLabsApiKey { get; set; }
    public string? ElevenLabsVoiceId { get; set; }
    public string? VapiPublicKey { get; set; }
    public bool EnableAiChat { get; set; } = true;
    public TenantTheme Theme { get; set; } = TenantTheme.Indigo;

    [StringLength(200, ErrorMessage = "Booking page title must not exceed 200 characters.")]
    public string? BookingPageTitle { get; set; }

    [Url(ErrorMessage = "Banner image URL must be a valid URL.")]
    [StringLength(2000, ErrorMessage = "Banner image URL must not exceed 2000 characters.")]
    public string? BannerImageUrl { get; set; }

    // File upload
    [StringLength(500, ErrorMessage = "Allowed file extensions must not exceed 500 characters.")]
    public string? AllowedFileExtensions { get; set; }

    // Accommodation (B&B / Hotel)
    [Range(0, 10000, ErrorMessage = "Total rooms must be between 0 and 10,000.")]
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

    [EmailAddress(ErrorMessage = "SendGrid from email must be a valid email address.")]
    [StringLength(254, ErrorMessage = "SendGrid from email must not exceed 254 characters.")]
    public string? SendGridFromEmail { get; set; }

    [StringLength(200, ErrorMessage = "SendGrid from name must not exceed 200 characters.")]
    public string? SendGridFromName { get; set; }

    public bool EnableEmailNotifications { get; set; }

    // Reminder alerts
    public string? ReminderAlerts { get; set; }
    public bool EnableEmailReminders { get; set; }
    public bool EnableSmsReminders { get; set; }
    public bool EnableSoftDelete { get; set; } = true;

    // Subdomain routing
    [RegularExpression(@"^[a-z0-9\-]{2,100}$", ErrorMessage = "Subdomain may only contain lowercase letters, digits and hyphens (2–100 characters).")]
    [StringLength(100, ErrorMessage = "Subdomain must not exceed 100 characters.")]
    public string? Subdomain { get; set; }

    public bool SubdomainApproved { get; set; }
}
