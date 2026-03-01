using BookIt.Core.Entities;
using BookIt.Core.Enums;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookIt.API.Services;

public interface IDatabaseSeederService
{
    Task SeedDemoDataAsync();
    Task ClearDemoDataAsync();
    Task<bool> HasDemoDataAsync();
}

public class DatabaseSeederService : IDatabaseSeederService
{
    private readonly BookItDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    // ── MudBlazor icon strings for amenities ──────────────────────────────────
    private const string IconPool     = "<path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M22 21c-1.11 0-1.73-.37-2.18-.64-.37-.22-.6-.36-1.15-.36-.56 0-.78.13-1.15.36-.46.27-1.07.64-2.18.64s-1.73-.37-2.18-.64c-.37-.22-.6-.36-1.15-.36-.56 0-.78.13-1.15.36-.46.27-1.08.64-2.19.64-1.11 0-1.73-.37-2.18-.64-.37-.23-.6-.36-1.15-.36s-.78.13-1.15.36c-.46.27-1.08.64-2.19.64v-2c.56 0 .78-.13 1.15-.36.46-.27 1.08-.64 2.19-.64s1.73.37 2.18.64c.37.23.59.36 1.15.36.56 0 .78-.13 1.15-.36.46-.27 1.08-.64 2.19-.64 1.11 0 1.73.37 2.18.64.37.22.6.36 1.15.36s.78-.13 1.15-.36c.45-.27 1.07-.64 2.18-.64s1.73.37 2.18.64c.37.23.59.36 1.15.36v2zm0-4.5c-1.11 0-1.73-.37-2.18-.64-.37-.22-.6-.36-1.15-.36-.56 0-.78.13-1.15.36-.45.27-1.07.64-2.18.64s-1.73-.37-2.18-.64c-.37-.22-.6-.36-1.15-.36-.56 0-.78.13-1.15.36-.45.27-1.07.64-2.18.64s-1.73-.37-2.18-.64c-.37-.22-.6-.36-1.15-.36s-.78.13-1.15.36c-.47.27-1.09.64-2.2.64v-2c.56 0 .78-.13 1.15-.36.45-.27 1.07-.64 2.18-.64s1.73.37 2.18.64c.37.22.6.36 1.15.36.56 0 .78-.13 1.15-.36.45-.27 1.07-.64 2.18-.64s1.73.37 2.18.64c.37.22.6.36 1.15.36s.78-.13 1.15-.36c.45-.27 1.07-.64 2.18-.64s1.73.37 2.18.64c.37.22.6.36 1.15.36v2zM8.67 12c.56 0 .78-.13 1.15-.36.46-.27 1.08-.64 2.19-.64 1.11 0 1.73.37 2.18.64.37.22.6.36 1.15.36s.78-.13 1.15-.36c.12-.07.26-.15.41-.23L10.48 5C8.93 3.45 7.5 2.99 5 3v2.5c1.82-.01 2.89.39 4 1.5l1 1-3.25 3.25c.31.12.56.27.77.39.37.23.59.36 1.15.36z\"/><circle cx=\"16.5\" cy=\"5.5\" r=\"2.5\"/>";
    private const string IconSpa      = "<path d=\"M0 0h24v24H0V0zm13.97 21.49c-.63.23-1.29.4-1.97.51.68-.12 1.33-.29 1.97-.51zM12 22c-.68-.12-1.33-.29-1.97-.51.64.22 1.29.39 1.97.51z\" fill=\"none\"/><path d=\"M8.55 12c-1.07-.71-2.25-1.27-3.53-1.61 1.28.34 2.46.9 3.53 1.61zm10.43-1.61c-1.29.34-2.49.91-3.57 1.64 1.08-.73 2.28-1.3 3.57-1.64z\"/><path d=\"M15.49 9.63c-.18-2.79-1.31-5.51-3.43-7.63-2.14 2.14-3.32 4.86-3.55 7.63 1.28.68 2.46 1.56 3.49 2.63 1.03-1.06 2.21-1.94 3.49-2.63zm-6.5 2.65c-.14-.1-.3-.19-.45-.29.15.11.31.19.45.29zm6.42-.25c-.13.09-.27.16-.4.26.13-.1.27-.17.4-.26zM12 15.45C9.85 12.17 6.18 10 2 10c0 5.32 3.36 9.82 8.03 11.49.63.23 1.29.4 1.97.51.68-.12 1.33-.29 1.97-.51C18.64 19.82 22 15.32 22 10c-4.18 0-7.85 2.17-10 5.45z\"/>";
    private const string IconGym      = "<path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M20.57 14.86L22 13.43 20.57 12 17 15.57 8.43 7 12 3.43 10.57 2 9.14 3.43 7.71 2 5.57 4.14 4.14 2.71 2.71 4.14l1.43 1.43L2 7.71l1.43 1.43L2 10.57 3.43 12 7 8.43 15.57 17 12 20.57 13.43 22l1.43-1.43L16.29 22l2.14-2.14 1.43 1.43 1.43-1.43-1.43-1.43L22 16.29z\"/>";
    private const string IconWifi     = "<path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M1 9l2 2c4.97-4.97 13.03-4.97 18 0l2-2C16.93 2.93 7.08 2.93 1 9zm8 8l3 3 3-3c-1.65-1.66-4.34-1.66-6 0zm-4-4l2 2c2.76-2.76 7.24-2.76 10 0l2-2C15.14 9.14 8.87 9.14 5 13z\"/>";
    private const string IconParking  = "<path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M13 3H6v18h4v-6h3c3.31 0 6-2.69 6-6s-2.69-6-6-6zm.2 8H10V7h3.2c1.1 0 2 .9 2 2s-.9 2-2 2z\"/>";
    private const string IconRestaurant = "<path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M11 9H9V2H7v7H5V2H3v7c0 2.12 1.66 3.84 3.75 3.97V22h2.5v-9.03C11.34 12.84 13 11.12 13 9V2h-2v7zm5-3v8h2.5v8H21V2c-2.76 0-5 2.24-5 4z\"/>";
    private const string IconBar      = "<path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M21 5V3H3v2l8 9v5H6v2h12v-2h-5v-5l8-9zM7.43 7L5.66 5h12.69l-1.78 2H7.43z\"/>";
    private const string IconAC       = "<path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M22 11h-4.17l3.24-3.24-1.41-1.42L15 11h-2V9l4.66-4.66-1.42-1.41L13 6.17V2h-2v4.17L7.76 2.93 6.34 4.34 11 9v2H9L4.34 6.34 2.93 7.76 6.17 11H2v2h4.17l-3.24 3.24 1.41 1.42L9 13h2v2l-4.66 4.66 1.42 1.41L11 17.83V22h2v-4.17l3.24 3.24 1.42-1.41L13 15v-2h2l4.66 4.66 1.41-1.42L17.83 13H22z\"/>";
    private const string IconHotTub   = "<path d=\"M0 0h24v24H0z\" fill=\"none\"/><circle cx=\"7\" cy=\"6\" r=\"2\"/><path d=\"M11.15 12c-.31-.22-.59-.46-.82-.72l-1.4-1.55c-.19-.21-.43-.38-.69-.5-.29-.14-.62-.23-.96-.23h-.03C6.01 9 5 10.01 5 11.25V12H2v8c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2v-8H11.15zM7 20H5v-6h2v6zm4 0H9v-6h2v6zm4 0h-2v-6h2v6zm4 0h-2v-6h2v6zm-.35-14.14l-.07-.07c-.57-.62-.82-1.41-.67-2.2L18 3h-1.89l-.06.43c-.2 1.36.27 2.71 1.3 3.72l.07.06c.57.62.82 1.41.67 2.2l-.11.59h1.91l.06-.43c.21-1.36-.27-2.71-1.3-3.71zm-4 0l-.07-.07c-.57-.62-.82-1.41-.67-2.2L14 3h-1.89l-.06.43c-.2 1.36.27 2.71 1.3 3.72l.07.06c.57.62.82 1.41.67 2.2l-.11.59h1.91l.06-.43c.21-1.36-.27-2.71-1.3-3.71z\"/>";
    private const string IconGarden   = "<g><rect fill=\"none\" height=\"24\" width=\"24\"/></g><g><polygon points=\"17,12 19,12 12,2 5.05,12 7,12 3.1,18 10.02,18 10.02,22 13.98,22 13.98,18 21,18\"/></g>";
    private const string IconRoomSvc  = "<path d=\"M0 0h24v24H0V0z\" fill=\"none\"/><path d=\"M2 17h20v2H2zm11.84-9.21c.1-.24.16-.51.16-.79 0-1.1-.9-2-2-2s-2 .9-2 2c0 .28.06.55.16.79C6.25 8.6 3.27 11.93 3 16h18c-.27-4.07-3.25-7.4-7.16-8.21z\"/>";
    private const string IconPets     = "<path d=\"M0 0h24v24H0z\" fill=\"none\"/><circle cx=\"4.5\" cy=\"9.5\" r=\"2.5\"/><circle cx=\"9\" cy=\"5.5\" r=\"2.5\"/><circle cx=\"15\" cy=\"5.5\" r=\"2.5\"/><circle cx=\"19.5\" cy=\"9.5\" r=\"2.5\"/><path d=\"M17.34 14.86c-.87-1.02-1.6-1.89-2.48-2.91-.46-.54-1.05-1.08-1.75-1.32-.11-.04-.22-.07-.33-.09-.25-.04-.52-.04-.78-.04s-.53 0-.79.05c-.11.02-.22.05-.33.09-.7.24-1.28.78-1.75 1.32-.87 1.02-1.6 1.89-2.48 2.91-1.31 1.31-2.92 2.76-2.62 4.79.29 1.02 1.02 2.03 2.33 2.32.73.15 3.06-.44 5.54-.44h.18c2.48 0 4.81.58 5.54.44 1.31-.29 2.04-1.31 2.33-2.32.31-2.04-1.3-3.49-2.61-4.8z\"/>";
    private const string IconBreakfast = "<g><rect fill=\"none\" height=\"24\" width=\"24\"/></g><g><path d=\"M18,3H6C3.79,3,2,4.79,2,7c0,1.48,0.81,2.75,2,3.45V19c0,1.1,0.9,2,2,2h12c1.1,0,2-0.9,2-2v-8.55c1.19-0.69,2-1.97,2-3.45C22,4.79,20.21,3,18,3z M14,15h-4v-4h4V15z\" fill-rule=\"evenodd\"/></g>";
    private const string IconSauna    = "<g><rect fill=\"none\" height=\"24\" width=\"24\"/></g><g><g><g><path d=\"M12,12.9l-2.13,2.09C9.31,15.55,9,16.28,9,17.06C9,18.68,10.35,20,12,20s3-1.32,3-2.94c0-0.78-0.31-1.52-0.87-2.07 L12,12.9z\"/></g><g><path d=\"M16,6l-0.44,0.55C14.38,8.02,12,7.19,12,5.3V2c0,0-8,4-8,11c0,2.92,1.56,5.47,3.89,6.86C7.33,19.07,7,18.1,7,17.06 c0-1.32,0.52-2.56,1.47-3.5L12,10.1l3.53,3.47c0.95,0.93,1.47,2.17,1.47,3.5c0,1.02-0.31,1.96-0.85,2.75 c1.89-1.15,3.29-3.06,3.71-5.3C20.52,10.97,18.79,7.62,16,6z\"/></g></g></g>";
    private const string IconLaundry  = "<path d=\"M0 0h24v24H0z\" fill=\"none\"/><path d=\"M9.17 16.83c1.56 1.56 4.1 1.56 5.66 0 1.56-1.56 1.56-4.1 0-5.66l-5.66 5.66zM18 2.01L6 2c-1.11 0-2 .89-2 2v16c0 1.11.89 2 2 2h12c1.11 0 2-.89 2-2V4c0-1.11-.89-1.99-2-1.99zM10 4c.55 0 1 .45 1 1s-.45 1-1 1-1-.45-1-1 .45-1 1-1zM7 4c.55 0 1 .45 1 1s-.45 1-1 1-1-.45-1-1 .45-1 1-1zm5 16c-3.31 0-6-2.69-6-6s2.69-6 6-6 6 2.69 6 6-2.69 6-6 6z\"/>";
    private const string IconTerrace  = "<rect fill=\"none\" height=\"24\" width=\"24\"/><path d=\"M10,10v2H8v-2H10z M16,12v-2h-2v2H16z M21,14v8H3v-8h1v-4c0-4.42,3.58-8,8-8s8,3.58,8,8v4H21z M7,16H5v4h2V16z M11,16H9v4h2 V16z M11,4.08C8.16,4.56,6,7.03,6,10v4h5V4.08z M13,14h5v-4c0-2.97-2.16-5.44-5-5.92V14z M15,16h-2v4h2V16z M19,16h-2v4h2V16z\"/>";

    public DatabaseSeederService(BookItDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<bool> HasDemoDataAsync()
    {
        return await _context.Staff.AnyAsync() ||
               await _context.Customers.AnyAsync() ||
               await _context.Clients.AnyAsync() ||
               await _context.LodgingProperties.AnyAsync() ||
               await _context.ClassSessions.AnyAsync() ||
               await _context.Tenants.AnyAsync(t => t.Slug == "demo-hotel" || t.Slug == "demo-bb" || t.Slug == "demo-gym");
    }

    public async Task SeedDemoDataAsync()
    {
        // ── Barber shop demo data ──────────────────────────────────────────────
        var demoTenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == "demo-barber");
        if (demoTenant != null && !await _context.Staff.AnyAsync())
        {
            var client1Id = await SeedClient(demoTenant.Id,
                "Elite Hair Solutions", "Sarah Johnson", "sarah@elitehair.example", "555-0101");
            var client2Id = await SeedClient(demoTenant.Id,
                "Urban Style Group", "Michael Chen", "michael@urbanstyle.example", "555-0102");

            await SeedStaff(demoTenant.Id, client1Id, "James", "Martinez", "james@elitehair.example", "555-0201", "Master Barber with 10 years experience");
            await SeedStaff(demoTenant.Id, client1Id, "Emma", "Wilson", "emma@elitehair.example", "555-0202", "Specialist in modern cuts and styling");
            await SeedStaff(demoTenant.Id, client2Id, "Oliver", "Brown", "oliver@urbanstyle.example", "555-0203", "Expert in beard grooming and hot shaves");

            await SeedCustomers(demoTenant.Id, 20);
            await SeedAppointments(demoTenant.Id);
            await _context.SaveChangesAsync();
        }

        // ── Hotel demo tenant ──────────────────────────────────────────────────
        if (!await _context.Tenants.AnyAsync(t => t.Slug == "demo-hotel"))
        {
            var hotelId = await CreateExtraTenantAsync(
                "demo-hotel", "Grand Palace Hotel", BusinessType.Hotel,
                "admin@demo-hotel.example", "London", "SW1A 2AA", "United Kingdom");

            var hotelAmenities = await SeedAmenitiesAsync(hotelId, new[]
            {
                (AmenityType.Pool,        "Swimming Pool",    "Heated indoor and outdoor pools",     IconPool),
                (AmenityType.Spa,         "Spa & Wellness",   "Full-service spa with luxury treatments", IconSpa),
                (AmenityType.Gym,         "Fitness Centre",   "State-of-the-art gym open 24 hours",  IconGym),
                (AmenityType.WiFi,        "Free WiFi",        "High-speed WiFi throughout the hotel", IconWifi),
                (AmenityType.Parking,     "Valet Parking",    "Secure valet parking available",      IconParking),
                (AmenityType.Restaurant,  "Fine Dining",      "Award-winning on-site restaurant",    IconRestaurant),
                (AmenityType.Bar,         "Bar & Lounge",     "Stylish cocktail bar and lounge",     IconBar),
                (AmenityType.AirConditioning, "Air Conditioning", "Climate control in all rooms",   IconAC),
                (AmenityType.HotTub,      "Hot Tub",          "Private and shared hot tubs",         IconHotTub),
                (AmenityType.Garden,      "Garden & Terrace", "Landscaped gardens with terrace seating", IconGarden),
                (AmenityType.RoomService, "Room Service",     "24-hour in-room dining service",      IconRoomSvc),
                (AmenityType.PetFriendly, "Pet Friendly",     "Pets welcome with advance notice",    IconPets),
            });

            var hotelPropertyId = await SeedLodgingPropertyAsync(
                hotelId, "Grand Palace Hotel",
                "1 Palace Road", "London", "SW1A 2AA", "United Kingdom",
                "+44 20 7946 0000", "reservations@grandpalace.example.com");

            await SeedHotelRoomsAsync(hotelId, hotelPropertyId, hotelAmenities);
            await SeedCustomers(hotelId, 15);
            await _context.SaveChangesAsync();
        }

        // ── Bed & Breakfast demo tenant ────────────────────────────────────────
        if (!await _context.Tenants.AnyAsync(t => t.Slug == "demo-bb"))
        {
            var bbId = await CreateExtraTenantAsync(
                "demo-bb", "Rose Cottage B&B", BusinessType.BedAndBreakfast,
                "admin@demo-bb.example", "Bourton-on-the-Water", "GL54 2AN", "United Kingdom");

            var bbAmenities = await SeedAmenitiesAsync(bbId, new[]
            {
                (AmenityType.WiFi,        "Free WiFi",        "High-speed WiFi throughout",           IconWifi),
                (AmenityType.Breakfast,   "Full Breakfast",   "Traditional full English breakfast included", IconBreakfast),
                (AmenityType.Garden,      "Cottage Garden",   "Beautiful private cottage garden",    IconGarden),
                (AmenityType.Parking,     "Free Parking",     "Private off-road parking for guests", IconParking),
                (AmenityType.PetFriendly, "Pet Friendly",     "Well-behaved dogs welcome",           IconPets),
                (AmenityType.Terrace,     "Garden Terrace",   "Outdoor seating and BBQ area",        IconTerrace),
                (AmenityType.Laundry,     "Laundry Service",  "Laundry and ironing on request",      IconLaundry),
            });

            var bbPropertyId = await SeedLodgingPropertyAsync(
                bbId, "Rose Cottage",
                "12 Mill Lane", "Bourton-on-the-Water", "GL54 2AN", "United Kingdom",
                "+44 1451 820 123", "stay@rosecottage.example.com");

            await SeedBnBRoomsAsync(bbId, bbPropertyId, bbAmenities);
            await SeedCustomers(bbId, 10);
            await _context.SaveChangesAsync();
        }

        // ── Gym / Fitness Centre demo tenant ──────────────────────────────────
        if (!await _context.Tenants.AnyAsync(t => t.Slug == "demo-gym"))
        {
            var gymId = await CreateExtraTenantAsync(
                "demo-gym", "City Fitness Centre", BusinessType.Gym,
                "admin@demo-gym.example", "Manchester", "M1 1AB", "United Kingdom");

            var gymServices = await SeedGymServicesAsync(gymId);
            var gymStaff    = await SeedGymInstructorsAsync(gymId, gymServices);
            var gymCustomers = await SeedCustomers(gymId, 20);
            await _context.SaveChangesAsync();

            await SeedClassSessionsAsync(gymId, gymServices, gymStaff, gymCustomers);
            await _context.SaveChangesAsync();
        }
    }

    // ── Barber helpers ────────────────────────────────────────────────────────

    private async Task<Guid> SeedClient(Guid tenantId, string companyName, string contactName, string email, string phone)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        Guid userId;
        if (existingUser == null)
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                Email = email,
                UserName = email,
                FirstName = contactName.Split(' ').FirstOrDefault() ?? contactName,
                LastName = contactName.Split(' ').Skip(1).FirstOrDefault() ?? "",
                PhoneNumber = phone,
                Role = UserRole.Customer,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };
            await _userManager.CreateAsync(user, "Client123!");
            await _userManager.AddToRoleAsync(user, "Customer");
            userId = user.Id;
        }
        else
        {
            userId = existingUser.Id;
        }

        var client = new Client
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            UserId = userId,
            CompanyName = companyName,
            ContactName = contactName,
            Email = email,
            Phone = phone,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        return client.Id;
    }

    private async Task SeedStaff(Guid tenantId, Guid? clientId, string firstName, string lastName, string email, string phone, string bio)
    {
        var staff = new Staff
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            ClientId = clientId,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phone,
            Bio = bio,
            IsActive = true,
            SortOrder = 0,
            CreatedAt = DateTime.UtcNow
        };

        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser == null)
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                Email = email,
                UserName = email,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phone,
                Role = UserRole.Staff,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };
            var result = await _userManager.CreateAsync(user, "Staff123!");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Staff");
                staff.UserId = user.Id;
            }
        }
        else
        {
            staff.UserId = existingUser.Id;
        }

        _context.Staff.Add(staff);

        var services = await _context.Services
            .Where(s => s.TenantId == tenantId && !s.IsDeleted)
            .Take(3)
            .ToListAsync();

        foreach (var service in services)
        {
            _context.Set<StaffService>().Add(new StaffService
            {
                StaffId = staff.Id,
                ServiceId = service.Id
            });
        }
    }

    private async Task<List<Customer>> SeedCustomers(Guid tenantId, int count)
    {
        var firstNames = new[] { "John", "Mary", "David", "Lisa", "Robert", "Jennifer", "Michael", "Linda", "William", "Patricia",
            "Richard", "Susan", "Joseph", "Jessica", "Thomas", "Sarah", "Charles", "Karen", "Daniel", "Nancy" };
        var lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez",
            "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin" };

        var random = new Random(42);
        var customers = new List<Customer>();

        for (int i = 0; i < count; i++)
        {
            var firstName = firstNames[random.Next(firstNames.Length)];
            var lastName  = lastNames[random.Next(lastNames.Length)];
            var email     = $"{firstName.ToLower()}.{lastName.ToLower()}{i}@example.com";

            var customer = new Customer
            {
                Id        = Guid.NewGuid(),
                TenantId  = tenantId,
                FirstName = firstName,
                LastName  = lastName,
                Email     = email,
                Phone     = $"555-{random.Next(1000, 9999)}",
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 365))
            };

            _context.Customers.Add(customer);
            customers.Add(customer);
        }

        return customers;
    }

    private async Task SeedAppointments(Guid tenantId)
    {
        var staff = await _context.Staff
            .Where(s => s.TenantId == tenantId && !s.IsDeleted)
            .ToListAsync();

        var customers = await _context.Customers
            .Where(c => c.TenantId == tenantId && !c.IsDeleted)
            .Take(10)
            .ToListAsync();

        var services = await _context.Services
            .Where(s => s.TenantId == tenantId && !s.IsDeleted)
            .ToListAsync();

        if (!staff.Any() || !customers.Any() || !services.Any()) return;

        var random = new Random(42);
        var now    = DateTime.UtcNow;

        for (int i = 0; i < 15; i++)
        {
            var customer   = customers[random.Next(customers.Count)];
            var service    = services[random.Next(services.Count)];
            var staffMember = staff[random.Next(staff.Count)];
            var startTime  = now.AddDays(-random.Next(1, 90)).Date.AddHours(9 + random.Next(0, 8));

            var appt = new Appointment
            {
                Id = Guid.NewGuid(), TenantId = tenantId,
                CustomerId = customer.Id, StaffId = staffMember.Id,
                CustomerName = customer.FullName, CustomerEmail = customer.Email, CustomerPhone = customer.Phone,
                StartTime = startTime, EndTime = startTime.AddMinutes(service.DurationMinutes),
                Status = AppointmentStatus.Completed, TotalAmount = service.Price,
                CustomerNotes = "Demo appointment", CreatedAt = startTime.AddDays(-7)
            };
            _context.Appointments.Add(appt);
            _context.Set<AppointmentService>().Add(new AppointmentService
            {
                AppointmentId = appt.Id, ServiceId = service.Id, PriceAtBooking = service.Price
            });
        }

        for (int i = 0; i < 10; i++)
        {
            var customer   = customers[random.Next(customers.Count)];
            var service    = services[random.Next(services.Count)];
            var staffMember = staff[random.Next(staff.Count)];
            var startTime  = now.AddDays(random.Next(1, 30)).Date.AddHours(9 + random.Next(0, 8));

            var appt = new Appointment
            {
                Id = Guid.NewGuid(), TenantId = tenantId,
                CustomerId = customer.Id, StaffId = staffMember.Id,
                CustomerName = customer.FullName, CustomerEmail = customer.Email, CustomerPhone = customer.Phone,
                StartTime = startTime, EndTime = startTime.AddMinutes(service.DurationMinutes),
                Status = AppointmentStatus.Confirmed, TotalAmount = service.Price,
                CustomerNotes = "Demo future appointment", CreatedAt = DateTime.UtcNow
            };
            _context.Appointments.Add(appt);
            _context.Set<AppointmentService>().Add(new AppointmentService
            {
                AppointmentId = appt.Id, ServiceId = service.Id, PriceAtBooking = service.Price
            });
        }
    }

    // ── Hotel & B&B helpers ───────────────────────────────────────────────────

    private async Task<Guid> CreateExtraTenantAsync(string slug, string name, BusinessType businessType,
        string adminEmail, string city, string postCode, string country)
    {
        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Name = name,
            Slug = slug,
            BusinessType = businessType,
            ContactEmail = adminEmail,
            City = city,
            PostCode = postCode,
            Country = country,
            TimeZone = "Europe/London",
            Currency = "GBP",
            IsActive = true,
            AllowOnlineBooking = true,
            EnableSoftDelete = false,   // hard-delete so re-seed works cleanly
            CreatedAt = DateTime.UtcNow
        };
        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync();

        var adminUser = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            TenantId = tenant.Id,
            Email = adminEmail,
            UserName = adminEmail,
            FirstName = "Demo",
            LastName = "Admin",
            Role = UserRole.TenantAdmin,
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };
        var result = await _userManager.CreateAsync(adminUser, "Admin123!");
        if (result.Succeeded)
            await _userManager.AddToRoleAsync(adminUser, "TenantAdmin");

        return tenant.Id;
    }

    private async Task<Dictionary<AmenityType, Guid>> SeedAmenitiesAsync(
        Guid tenantId,
        IEnumerable<(AmenityType Type, string Name, string Description, string Icon)> defs)
    {
        var result = new Dictionary<AmenityType, Guid>();
        foreach (var (type, name, desc, icon) in defs)
        {
            var amenity = new Amenity
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                Name = name,
                AmenityType = type,
                Description = desc,
                Icon = icon,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            _context.Amenities.Add(amenity);
            result[type] = amenity.Id;
        }
        return result;
    }

    private async Task<Guid> SeedLodgingPropertyAsync(Guid tenantId, string name,
        string address, string city, string postCode, string country,
        string phone, string email)
    {
        var property = new LodgingProperty
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Name = name,
            Address = address,
            City = city,
            PostCode = postCode,
            Country = country,
            PhoneNumber = phone,
            Email = email,
            IsActive = true,
            SortOrder = 1,
            CreatedAt = DateTime.UtcNow
        };
        _context.LodgingProperties.Add(property);
        return property.Id;
    }

    private void AddRoomAmenities(Guid roomId, Dictionary<AmenityType, Guid> amenityMap, params AmenityType[] types)
    {
        foreach (var type in types)
        {
            if (amenityMap.TryGetValue(type, out var amenityId))
            {
                _context.RoomAmenities.Add(new RoomAmenity { RoomId = roomId, AmenityId = amenityId });
            }
        }
    }

    private async Task SeedHotelRoomsAsync(Guid tenantId, Guid propertyId, Dictionary<AmenityType, Guid> amenities)
    {
        var rooms = new[]
        {
            (Name: "Standard Double Room",  Desc: "Comfortable double room with city views",        Type: RoomType.Double,  Cap: 2, Rate: 120m, Sort: 1),
            (Name: "Superior King Room",    Desc: "Spacious king room with premium amenities",      Type: RoomType.Double,  Cap: 2, Rate: 175m, Sort: 2),
            (Name: "Executive Suite",       Desc: "Elegant suite with separate lounge and spa bath", Type: RoomType.Suite,  Cap: 2, Rate: 295m, Sort: 3),
            (Name: "Deluxe Twin Room",      Desc: "Bright twin room ideal for business travellers", Type: RoomType.Twin,    Cap: 2, Rate: 145m, Sort: 4),
            (Name: "Family Room",           Desc: "Spacious room sleeping up to 4 guests",          Type: RoomType.Family, Cap: 4, Rate: 220m, Sort: 5),
            (Name: "Penthouse Suite",       Desc: "Our most prestigious suite with panoramic views", Type: RoomType.Suite,  Cap: 3, Rate: 550m, Sort: 6),
        };

        foreach (var (rName, rDesc, rType, rCap, rRate, rSort) in rooms)
        {
            var room = new Room
            {
                Id = Guid.NewGuid(), TenantId = tenantId, LodgingPropertyId = propertyId,
                Name = rName, Description = rDesc, RoomType = rType,
                Capacity = rCap, BaseRate = rRate, IsActive = true, SortOrder = rSort,
                CreatedAt = DateTime.UtcNow
            };
            _context.Rooms.Add(room);

            // Assign amenities relevant to the room type
            if (rType == RoomType.Suite)
            {
                AddRoomAmenities(room.Id, amenities,
                    AmenityType.WiFi, AmenityType.AirConditioning, AmenityType.RoomService,
                    AmenityType.Pool, AmenityType.Spa, AmenityType.Gym,
                    AmenityType.Restaurant, AmenityType.Bar, AmenityType.HotTub);
            }
            else if (rType == RoomType.Family)
            {
                AddRoomAmenities(room.Id, amenities,
                    AmenityType.WiFi, AmenityType.AirConditioning, AmenityType.Pool,
                    AmenityType.Gym, AmenityType.PetFriendly, AmenityType.Parking);
            }
            else
            {
                AddRoomAmenities(room.Id, amenities,
                    AmenityType.WiFi, AmenityType.AirConditioning,
                    AmenityType.Pool, AmenityType.Gym, AmenityType.Restaurant);
            }
        }
    }

    private async Task SeedBnBRoomsAsync(Guid tenantId, Guid propertyId, Dictionary<AmenityType, Guid> amenities)
    {
        var rooms = new[]
        {
            (Name: "The Garden Room",   Desc: "Charming room overlooking the cottage garden",     Type: RoomType.Double, Cap: 2, Rate: 95m,  Sort: 1),
            (Name: "The Orchard Room",  Desc: "Cosy room with views of the orchard",              Type: RoomType.Double, Cap: 2, Rate: 85m,  Sort: 2),
            (Name: "The Village Room",  Desc: "Bright twin room with village views",              Type: RoomType.Twin,   Cap: 2, Rate: 80m,  Sort: 3),
            (Name: "The Heritage Suite", Desc: "Spacious suite with exposed beams and fireplace", Type: RoomType.Suite,  Cap: 2, Rate: 130m, Sort: 4),
        };

        foreach (var (rName, rDesc, rType, rCap, rRate, rSort) in rooms)
        {
            var room = new Room
            {
                Id = Guid.NewGuid(), TenantId = tenantId, LodgingPropertyId = propertyId,
                Name = rName, Description = rDesc, RoomType = rType,
                Capacity = rCap, BaseRate = rRate, IsActive = true, SortOrder = rSort,
                CreatedAt = DateTime.UtcNow
            };
            _context.Rooms.Add(room);

            AddRoomAmenities(room.Id, amenities,
                AmenityType.WiFi, AmenityType.Breakfast, AmenityType.Parking,
                AmenityType.PetFriendly, AmenityType.Garden);

            if (rType == RoomType.Suite)
                AddRoomAmenities(room.Id, amenities, AmenityType.Terrace, AmenityType.Laundry);
        }
    }

    // ── Gym helpers ───────────────────────────────────────────────────────────

    private async Task<List<Service>> SeedGymServicesAsync(Guid tenantId)
    {
        var cat1 = new ServiceCategory
        {
            Id = Guid.NewGuid(), TenantId = tenantId, Name = "Group Classes",
            SortOrder = 1, IsActive = true, CreatedAt = DateTime.UtcNow
        };
        var cat2 = new ServiceCategory
        {
            Id = Guid.NewGuid(), TenantId = tenantId, Name = "Aquatics",
            SortOrder = 2, IsActive = true, CreatedAt = DateTime.UtcNow
        };
        _context.ServiceCategories.AddRange(cat1, cat2);

        var services = new List<Service>
        {
            new Service { Id = Guid.NewGuid(), TenantId = tenantId, CategoryId = cat1.Id, Name = "Yoga",        Slug = "yoga",         Description = "Relaxing yoga session for all levels",       Price = 12m, DurationMinutes = 60, IsActive = true, AllowOnlineBooking = true, SortOrder = 1, CreatedAt = DateTime.UtcNow },
            new Service { Id = Guid.NewGuid(), TenantId = tenantId, CategoryId = cat1.Id, Name = "Spinning",    Slug = "spinning",     Description = "High-energy indoor cycling class",            Price = 10m, DurationMinutes = 45, IsActive = true, AllowOnlineBooking = true, SortOrder = 2, CreatedAt = DateTime.UtcNow },
            new Service { Id = Guid.NewGuid(), TenantId = tenantId, CategoryId = cat1.Id, Name = "HIIT",        Slug = "hiit",         Description = "High-intensity interval training",            Price = 12m, DurationMinutes = 45, IsActive = true, AllowOnlineBooking = true, SortOrder = 3, CreatedAt = DateTime.UtcNow },
            new Service { Id = Guid.NewGuid(), TenantId = tenantId, CategoryId = cat1.Id, Name = "Body Pump",   Slug = "body-pump",    Description = "Barbell-based full body resistance class",    Price = 10m, DurationMinutes = 60, IsActive = true, AllowOnlineBooking = true, SortOrder = 4, CreatedAt = DateTime.UtcNow },
            new Service { Id = Guid.NewGuid(), TenantId = tenantId, CategoryId = cat1.Id, Name = "Zumba",       Slug = "zumba",        Description = "Dance fitness combining Latin rhythms",       Price = 8m,  DurationMinutes = 60, IsActive = true, AllowOnlineBooking = true, SortOrder = 5, CreatedAt = DateTime.UtcNow },
            new Service { Id = Guid.NewGuid(), TenantId = tenantId, CategoryId = cat2.Id, Name = "Swimming",    Slug = "swimming",     Description = "Lane swimming — all abilities welcome",       Price = 6m,  DurationMinutes = 60, IsActive = true, AllowOnlineBooking = true, SortOrder = 6, CreatedAt = DateTime.UtcNow },
            new Service { Id = Guid.NewGuid(), TenantId = tenantId, CategoryId = cat2.Id, Name = "Aqua Aerobics", Slug = "aqua-aerobics", Description = "Low-impact water aerobics class",       Price = 8m,  DurationMinutes = 45, IsActive = true, AllowOnlineBooking = true, SortOrder = 7, CreatedAt = DateTime.UtcNow },
        };

        _context.Services.AddRange(services);
        return services;
    }

    private async Task<List<Staff>> SeedGymInstructorsAsync(Guid tenantId, List<Service> services)
    {
        var instructors = new[]
        {
            (First: "Sophie", Last: "Clarke",  Email: "sophie@demo-gym.example",  Phone: "555-3001", Bio: "Certified yoga and pilates instructor with 8 years experience"),
            (First: "Tom",    Last: "Hughes",   Email: "tom@demo-gym.example",     Phone: "555-3002", Bio: "Ex-professional cyclist and HIIT specialist"),
            (First: "Priya",  Last: "Sharma",   Email: "priya@demo-gym.example",   Phone: "555-3003", Bio: "Aquatics coach and swimming instructor"),
        };

        var staffList = new List<Staff>();
        // Sophie (group classes): Yoga, Spinning, HIIT
        // Tom   (cardio/cycle):   Spinning, HIIT, Body Pump
        // Priya (aquatics):       Swimming, Aqua Aerobics
        var serviceGroups = new[]
        {
            services.Take(3).ToList(),          // Sophie → Yoga, Spinning, HIIT
            services.Skip(1).Take(3).ToList(),  // Tom    → Spinning, HIIT, Body Pump
            services.Skip(4).ToList()           // Priya  → Zumba, Swimming, Aqua Aerobics
        };

        for (int i = 0; i < instructors.Length; i++)
        {
            var (first, last, email, phone, bio) = instructors[i];

            var existingUser = await _userManager.FindByEmailAsync(email);
            Guid? userId = existingUser?.Id;

            if (existingUser == null)
            {
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid(), TenantId = tenantId,
                    Email = email, UserName = email,
                    FirstName = first, LastName = last,
                    PhoneNumber = phone, Role = UserRole.Staff,
                    EmailConfirmed = true, CreatedAt = DateTime.UtcNow
                };
                var result = await _userManager.CreateAsync(user, "Staff123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Staff");
                    userId = user.Id;
                }
            }

            var staff = new Staff
            {
                Id = Guid.NewGuid(), TenantId = tenantId, UserId = userId,
                FirstName = first, LastName = last, Email = email, Phone = phone,
                Bio = bio, IsActive = true, SortOrder = i + 1, CreatedAt = DateTime.UtcNow
            };
            _context.Staff.Add(staff);

            foreach (var svc in serviceGroups[i])
            {
                _context.Set<StaffService>().Add(new StaffService { StaffId = staff.Id, ServiceId = svc.Id });
            }

            staffList.Add(staff);
        }

        return staffList;
    }

    private async Task SeedClassSessionsAsync(Guid tenantId, List<Service> services,
        List<Staff> instructors, List<Customer> customers)
    {
        if (!services.Any() || !instructors.Any()) return;

        var random = new Random(99);
        var now    = DateTime.UtcNow;

        // Weekly schedule template: (service index, instructor index, day offset from Monday, hour)
        var schedule = new[]
        {
            (SvcIdx: 0, InstrIdx: 0, DayOfWeek: DayOfWeek.Monday,    Hour: 9,  Cap: 20, ClassType: ClassType.Yoga),
            (SvcIdx: 1, InstrIdx: 1, DayOfWeek: DayOfWeek.Monday,    Hour: 18, Cap: 15, ClassType: ClassType.Spinning),
            (SvcIdx: 2, InstrIdx: 1, DayOfWeek: DayOfWeek.Tuesday,   Hour: 7,  Cap: 12, ClassType: ClassType.HiitCardio),
            (SvcIdx: 3, InstrIdx: 0, DayOfWeek: DayOfWeek.Wednesday, Hour: 10, Cap: 20, ClassType: ClassType.BodyPump),
            (SvcIdx: 4, InstrIdx: 0, DayOfWeek: DayOfWeek.Wednesday, Hour: 19, Cap: 25, ClassType: ClassType.Zumba),
            (SvcIdx: 5, InstrIdx: 2, DayOfWeek: DayOfWeek.Thursday,  Hour: 7,  Cap: 30, ClassType: ClassType.Swimming),
            (SvcIdx: 6, InstrIdx: 2, DayOfWeek: DayOfWeek.Thursday,  Hour: 11, Cap: 15, ClassType: ClassType.WaterAerobics),
            (SvcIdx: 1, InstrIdx: 1, DayOfWeek: DayOfWeek.Friday,    Hour: 18, Cap: 15, ClassType: ClassType.Spinning),
            (SvcIdx: 0, InstrIdx: 0, DayOfWeek: DayOfWeek.Saturday,  Hour: 10, Cap: 20, ClassType: ClassType.Yoga),
            (SvcIdx: 5, InstrIdx: 2, DayOfWeek: DayOfWeek.Saturday,  Hour: 14, Cap: 30, ClassType: ClassType.OpenSwim),
        };

        // Generate sessions for the past 2 weeks and next 3 weeks
        const int pastWeeks   = 2;
        const int futureWeeks = 3;
        var weeks = Enumerable.Range(-pastWeeks, pastWeeks + futureWeeks + 1).ToArray();

        foreach (var weekOffset in weeks)
        {
            var weekStart = now.StartOfWeek(DayOfWeek.Monday).AddDays(weekOffset * 7);

            foreach (var (svcIdx, instrIdx, dayOfWeek, hour, cap, classType) in schedule)
            {
                if (svcIdx >= services.Count || instrIdx >= instructors.Count) continue;

                var svc   = services[svcIdx];
                var instr = instructors[instrIdx];

                var daysAhead = ((int)dayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
                var sessionDate = weekStart.AddDays(daysAhead);
                var startTime   = new TimeOnly(hour, 0);
                var isPast      = sessionDate.AddHours(hour + svc.DurationMinutes / 60.0) < now;

                var bookingCount = isPast
                    ? random.Next(Math.Max(1, cap / 2), cap + 1)
                    : random.Next(0, cap / 2 + 1);

                var session = new ClassSession
                {
                    Id = Guid.NewGuid(), TenantId = tenantId,
                    ServiceId = svc.Id, StaffId = instr.Id,
                    Name = svc.Name, Description = svc.Description,
                    SessionDate = sessionDate, StartTime = startTime,
                    DurationMinutes = svc.DurationMinutes,
                    MaxCapacity = cap, CurrentBookings = bookingCount,
                    Price = svc.Price,
                    Status = isPast ? SessionStatus.Completed : SessionStatus.Scheduled,
                    ClassType = classType,
                    CreatedAt = DateTime.UtcNow
                };
                _context.ClassSessions.Add(session);

                // Create bookings for this session (link customers via appointments)
                var enrolledCustomers = customers
                    .OrderBy(_ => random.Next())
                    .Take(bookingCount)
                    .ToList();

                foreach (var customer in enrolledCustomers)
                {
                    var apptStart = sessionDate.AddHours(hour);
                    var appt = new Appointment
                    {
                        Id = Guid.NewGuid(), TenantId = tenantId,
                        CustomerId = customer.Id, StaffId = instr.Id,
                        CustomerName = customer.FullName, CustomerEmail = customer.Email,
                        CustomerPhone = customer.Phone,
                        StartTime = apptStart,
                        EndTime   = apptStart.AddMinutes(svc.DurationMinutes),
                        Status    = isPast ? AppointmentStatus.Completed : AppointmentStatus.Confirmed,
                        TotalAmount = svc.Price,
                        CustomerNotes = $"Booked into {svc.Name} class",
                        CreatedAt = apptStart.AddDays(-random.Next(1, 14))
                    };
                    _context.Appointments.Add(appt);

                    _context.Set<AppointmentService>().Add(new AppointmentService
                    {
                        AppointmentId = appt.Id, ServiceId = svc.Id, PriceAtBooking = svc.Price
                    });

                    _context.ClassSessionBookings.Add(new ClassSessionBooking
                    {
                        ClassSessionId = session.Id,
                        AppointmentId  = appt.Id,
                        BookedAt       = appt.CreatedAt,
                        ParticipantCount = 1
                    });
                }
            }
        }
    }

    // ── Clear demo data ───────────────────────────────────────────────────────

    public async Task ClearDemoDataAsync()
    {
        // Clear barber shop dynamic data
        var demoTenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == "demo-barber");
        if (demoTenant != null)
        {
            var appointments = await _context.Appointments
                .Where(a => a.TenantId == demoTenant.Id).ToListAsync();
            _context.Appointments.RemoveRange(appointments);

            var customers = await _context.Customers
                .Where(c => c.TenantId == demoTenant.Id).ToListAsync();
            _context.Customers.RemoveRange(customers);

            var staff = await _context.Staff
                .Where(s => s.TenantId == demoTenant.Id).ToListAsync();
            _context.Staff.RemoveRange(staff);

            var clients = await _context.Clients
                .Include(c => c.User)
                .Where(c => c.TenantId == demoTenant.Id).ToListAsync();
            foreach (var client in clients)
                if (client.User != null)
                    await _userManager.DeleteAsync(client.User);
            _context.Clients.RemoveRange(clients);

            await _context.SaveChangesAsync();
        }

        // Clear extra demo tenants entirely
        foreach (var slug in new[] { "demo-hotel", "demo-bb", "demo-gym" })
            await ClearExtraTenantAsync(slug);
    }

    private async Task ClearExtraTenantAsync(string slug)
    {
        // Use IgnoreQueryFilters in case tenant was previously soft-deleted
        var tenant = await _context.Tenants
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(t => t.Slug == slug);

        if (tenant == null) return;

        var tenantId = tenant.Id;

        // Class session bookings
        var sessionIds = await _context.ClassSessions
            .Where(cs => cs.TenantId == tenantId)
            .Select(cs => cs.Id)
            .ToListAsync();
        if (sessionIds.Any())
        {
            var bookings = await _context.ClassSessionBookings
                .Where(b => sessionIds.Contains(b.ClassSessionId))
                .ToListAsync();
            _context.ClassSessionBookings.RemoveRange(bookings);
        }

        // Appointments
        var appts = await _context.Appointments
            .Where(a => a.TenantId == tenantId).ToListAsync();
        _context.Appointments.RemoveRange(appts);

        // Customers
        var customers = await _context.Customers
            .Where(c => c.TenantId == tenantId).ToListAsync();
        _context.Customers.RemoveRange(customers);

        // Staff and their users
        var staffList = await _context.Staff
            .Where(s => s.TenantId == tenantId).ToListAsync();
        foreach (var s in staffList.Where(s => s.UserId.HasValue))
        {
            var user = await _userManager.FindByIdAsync(s.UserId!.Value.ToString());
            if (user != null) await _userManager.DeleteAsync(user);
        }
        _context.Staff.RemoveRange(staffList);

        // Class sessions
        var sessions = await _context.ClassSessions
            .Where(cs => cs.TenantId == tenantId).ToListAsync();
        _context.ClassSessions.RemoveRange(sessions);

        // Room amenities → rooms → lodging properties → amenities
        var roomIds = await _context.Rooms
            .Where(r => r.TenantId == tenantId)
            .Select(r => r.Id)
            .ToListAsync();
        if (roomIds.Any())
        {
            var roomAmenities = await _context.RoomAmenities
                .Where(ra => roomIds.Contains(ra.RoomId)).ToListAsync();
            _context.RoomAmenities.RemoveRange(roomAmenities);
        }

        var rooms = await _context.Rooms
            .Where(r => r.TenantId == tenantId).ToListAsync();
        _context.Rooms.RemoveRange(rooms);

        var properties = await _context.LodgingProperties
            .Where(p => p.TenantId == tenantId).ToListAsync();
        _context.LodgingProperties.RemoveRange(properties);

        var amenities = await _context.Amenities
            .Where(a => a.TenantId == tenantId).ToListAsync();
        _context.Amenities.RemoveRange(amenities);

        // Services and categories
        var services = await _context.Services
            .Where(s => s.TenantId == tenantId).ToListAsync();
        _context.Services.RemoveRange(services);

        var categories = await _context.ServiceCategories
            .Where(sc => sc.TenantId == tenantId).ToListAsync();
        _context.ServiceCategories.RemoveRange(categories);

        await _context.SaveChangesAsync();

        // Delete admin and other users for this tenant
        var tenantUsers = await _userManager.Users
            .Where(u => u.TenantId == tenantId)
            .ToListAsync();
        foreach (var user in tenantUsers)
            await _userManager.DeleteAsync(user);

        // Delete the tenant itself (hard delete since EnableSoftDelete = false)
        _context.Tenants.Remove(tenant);
        await _context.SaveChangesAsync();
    }
}

// ── Extension helpers ─────────────────────────────────────────────────────────
internal static class DateTimeExtensions
{
    internal static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
    {
        int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
        return dt.Date.AddDays(-diff);
    }
}
