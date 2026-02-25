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
    Staff = 3,
    Customer = 4
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
    Radio = 9
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
