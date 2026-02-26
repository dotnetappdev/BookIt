using BookIt.Core.Enums;

namespace BookIt.Core.Entities;

public class Tenant : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty; // URL-friendly identifier
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
    public string? TimeZone { get; set; } = "UTC";
    public string? Currency { get; set; } = "GBP";
    public bool IsActive { get; set; } = true;
    public bool AllowOnlineBooking { get; set; } = true;
    public bool RequirePaymentUpfront { get; set; } = false;
    public bool SendReminders { get; set; } = true;
    public int ReminderHoursBefore { get; set; } = 24;

    // Payment gateway settings
    public string? StripePublishableKey { get; set; }
    public string? StripeSecretKey { get; set; }
    public string? PayPalClientId { get; set; }
    public string? PayPalClientSecret { get; set; }
    public bool EnableStripe { get; set; }
    public bool EnablePayPal { get; set; }
    public bool EnableApplePay { get; set; }

    // RevenueCat subscription management
    public string? RevenueCatApiKey { get; set; }
    public string? RevenueCatEntitlementId { get; set; } = "premium";

    // Virtual meeting settings
    public string? ZoomApiKey { get; set; }
    public string? ZoomApiSecret { get; set; }
    public string? DefaultMeetingLink { get; set; }

    // Embedding settings
    public string? AllowedEmbedDomains { get; set; } // comma-separated
    public string? CustomCss { get; set; }

    // AI chat & voice
    public string? OpenAiApiKey { get; set; }
    public string? ElevenLabsApiKey { get; set; }
    public string? ElevenLabsVoiceId { get; set; }
    public string? VapiPublicKey { get; set; }
    public bool EnableAiChat { get; set; } = true;

    // Navigation properties
    public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    public ICollection<Service> Services { get; set; } = new List<Service>();
    public ICollection<ServiceCategory> ServiceCategories { get; set; } = new List<ServiceCategory>();
    public ICollection<Staff> StaffMembers { get; set; } = new List<Staff>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<BusinessHours> BusinessHours { get; set; } = new List<BusinessHours>();
    public ICollection<BookingForm> BookingForms { get; set; } = new List<BookingForm>();
}
