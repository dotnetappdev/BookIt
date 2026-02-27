namespace BookIt.Core.Enums;

public enum BusinessType
{
    Barber = 1,
    HairDresser = 2,
    Salon = 3,
    MassageTherapy = 4,
    Spa = 5,
    Gym = 6,
    NailSalon = 7,
    Tattoo = 8,
    Physiotherapy = 9,
    PersonalTrainer = 10,
    Recruitment = 11,
    BedAndBreakfast = 12,
    Hotel = 13,
    Other = 99
}

public enum AppointmentStatus
{
    Pending = 1,
    Confirmed = 2,
    Cancelled = 3,
    Completed = 4,
    NoShow = 5,
    Rescheduled = 6
}

public enum PaymentStatus
{
    Unpaid = 1,
    Pending = 2,
    Paid = 3,
    Refunded = 4,
    Failed = 5,
    PartiallyRefunded = 6
}

public enum PaymentProvider
{
    Stripe = 1,
    PayPal = 2,
    ApplePay = 3,
    Manual = 4
}

public enum PaymentMethod
{
    Online = 1,
    PayAtShop = 2,
    Cash = 3
}

public enum MeetingType
{
    InPerson = 1,
    Zoom = 2,
    MicrosoftTeams = 3,
    GoogleMeet = 4,
    PhoneCall = 5,
    Other = 6
}

public enum UserRole
{
    SuperAdmin = 1,
    TenantAdmin = 2,
    Manager = 3,
    Staff = 4,
    Customer = 5
}

public enum DayOfWeekFlag
{
    Monday = 1,
    Tuesday = 2,
    Wednesday = 3,
    Thursday = 4,
    Friday = 5,
    Saturday = 6,
    Sunday = 7
}

public enum SlotDuration
{
    FifteenMinutes = 15,
    ThirtyMinutes = 30,
    FortyFiveMinutes = 45,
    OneHour = 60,
    NinetyMinutes = 90,
    TwoHours = 120
}

public enum BookingFormFieldType
{
    Text = 1,
    Email = 2,
    Phone = 3,
    TextArea = 4,
    Select = 5,
    Checkbox = 6,
    Date = 7,
    Number = 8,
    Radio = 9,
    FileUpload = 10,
    Rating = 11,
    Signature = 12,
    Heading = 13,
    Paragraph = 14,
    ServicesList = 15
}

public enum ClassType
{
    General = 1,
    Yoga = 2,
    Pilates = 3,
    Spinning = 4,
    Swimming = 5,
    Aerobics = 6,
    BodyPump = 7,
    Zumba = 8,
    CrossFit = 9,
    HiitCardio = 10,
    WaterAerobics = 11,
    OpenSwim = 12,
    KidsSwim = 13,
    PersonalTraining = 14,
    Other = 99
}

public enum SessionStatus
{
    Scheduled = 1,
    InProgress = 2,
    Completed = 3,
    Cancelled = 4,
    Full = 5
}

public enum SubscriptionPlan
{
    Free = 0,
    Starter = 1,
    Pro = 2,
    Enterprise = 3
}

public enum SubscriptionStatus
{
    Active = 1,
    Trialing = 2,
    PastDue = 3,
    Cancelled = 4,
    Expired = 5
}

public enum TenantTheme
{
    Indigo = 0,   // default purple/indigo
    Ocean = 1,    // teal/cyan
    Forest = 2,   // green
    Sunset = 3,   // orange/amber
    Rose = 4,     // pink/rose
    Midnight = 5  // deep slate/navy
}

public enum SmsProvider
{
    None = 0,
    ClickSend = 1,
    Twilio = 2
}

public enum EmailTemplateType
{
    BookingConfirmation = 0,
    AppointmentReminder = 1,
    BookingCancellation = 2
}

public enum VideoConferenceProvider
{
    None = 0,
    MicrosoftTeams = 1,
    Zoom = 2,
    GoogleMeet = 3,
    Webex = 4,
    GoToMeeting = 5,
    Jitsi = 6,
    Whereby = 7,
    Other = 8
}

public enum RoomType
{
    Single = 1,
    Double = 2,
    Twin = 3,
    Suite = 4,
    Family = 5,
    Studio = 6,
    Dormitory = 7,
    Other = 99
}

public enum AmenityType
{
    Pool = 1,
    Gym = 2,
    Sauna = 3,
    Spa = 4,
    WiFi = 5,
    Parking = 6,
    Restaurant = 7,
    Bar = 8,
    Breakfast = 9,
    Laundry = 10,
    Concierge = 11,
    RoomService = 12,
    PetFriendly = 13,
    AirConditioning = 14,
    Heating = 15,
    HotTub = 16,
    Garden = 17,
    Terrace = 18,
    SeaView = 19,
    Other = 99
}
