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

    public DatabaseSeederService(BookItDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<bool> HasDemoDataAsync()
    {
        return await _context.Staff.AnyAsync() || 
               await _context.Customers.AnyAsync() ||
               await _context.Clients.AnyAsync();
    }

    public async Task SeedDemoDataAsync()
    {
        // Get demo tenant
        var demoTenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Slug == "demo-barber");

        if (demoTenant == null) return;

        // Check if already seeded
        if (await HasDemoDataAsync()) return;

        // Seed Clients
        var client1Id = await SeedClient(demoTenant.Id, 
            "Elite Hair Solutions", "Sarah Johnson", "sarah@elitehair.com", "555-0101");
        var client2Id = await SeedClient(demoTenant.Id,
            "Urban Style Group", "Michael Chen", "michael@urbanstyle.com", "555-0102");

        // Seed Staff (also creates ApplicationUser accounts with Staff role)
        await SeedStaff(demoTenant.Id, client1Id, "James", "Martinez", "james@elitehair.com", "555-0201", "Master Barber with 10 years experience");
        await SeedStaff(demoTenant.Id, client1Id, "Emma", "Wilson", "emma@elitehair.com", "555-0202", "Specialist in modern cuts and styling");
        await SeedStaff(demoTenant.Id, client2Id, "Oliver", "Brown", "oliver@urbanstyle.com", "555-0203", "Expert in beard grooming and hot shaves");

        // Seed Customers
        await SeedCustomers(demoTenant.Id, 20);

        // Seed Appointments
        await SeedAppointments(demoTenant.Id);

        await _context.SaveChangesAsync();
    }

    private async Task<Guid> SeedClient(Guid tenantId, string companyName, string contactName, string email, string phone)
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

        var client = new Client
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            UserId = user.Id,
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

        // Create linked ApplicationUser account so the staff member can log in
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

        // Assign services (get first 2-3 services)
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

    private async Task SeedCustomers(Guid tenantId, int count)
    {
        var firstNames = new[] { "John", "Mary", "David", "Lisa", "Robert", "Jennifer", "Michael", "Linda", "William", "Patricia",
            "Richard", "Susan", "Joseph", "Jessica", "Thomas", "Sarah", "Charles", "Karen", "Daniel", "Nancy" };
        var lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez",
            "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin" };

        var random = new Random(42); // Fixed seed for reproducibility

        for (int i = 0; i < count; i++)
        {
            var firstName = firstNames[random.Next(firstNames.Length)];
            var lastName = lastNames[random.Next(lastNames.Length)];
            var email = $"{firstName.ToLower()}.{lastName.ToLower()}{i}@example.com";

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = $"555-{random.Next(1000, 9999)}",
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 365))
            };

            _context.Customers.Add(customer);
        }
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
        var now = DateTime.UtcNow;

        // Create some past appointments
        for (int i = 0; i < 15; i++)
        {
            var customer = customers[random.Next(customers.Count)];
            var service = services[random.Next(services.Count)];
            var staffMember = staff[random.Next(staff.Count)];
            
            var daysAgo = random.Next(1, 90);
            var startTime = now.AddDays(-daysAgo).Date.AddHours(9 + random.Next(0, 8));

            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                CustomerId = customer.Id,
                StaffId = staffMember.Id,
                CustomerName = customer.FullName,
                CustomerEmail = customer.Email,
                CustomerPhone = customer.Phone,
                StartTime = startTime,
                EndTime = startTime.AddMinutes(service.DurationMinutes),
                Status = AppointmentStatus.Completed,
                TotalAmount = service.Price,
                CustomerNotes = "Demo appointment",
                CreatedAt = startTime.AddDays(-7)
            };

            _context.Appointments.Add(appointment);

            _context.Set<AppointmentService>().Add(new AppointmentService
            {
                AppointmentId = appointment.Id,
                ServiceId = service.Id,
                PriceAtBooking = service.Price
            });
        }

        // Create some future appointments
        for (int i = 0; i < 10; i++)
        {
            var customer = customers[random.Next(customers.Count)];
            var service = services[random.Next(services.Count)];
            var staffMember = staff[random.Next(staff.Count)];
            
            var daysAhead = random.Next(1, 30);
            var startTime = now.AddDays(daysAhead).Date.AddHours(9 + random.Next(0, 8));

            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                CustomerId = customer.Id,
                StaffId = staffMember.Id,
                CustomerName = customer.FullName,
                CustomerEmail = customer.Email,
                CustomerPhone = customer.Phone,
                StartTime = startTime,
                EndTime = startTime.AddMinutes(service.DurationMinutes),
                Status = AppointmentStatus.Confirmed,
                TotalAmount = service.Price,
                CustomerNotes = "Demo future appointment",
                CreatedAt = DateTime.UtcNow
            };

            _context.Appointments.Add(appointment);

            _context.Set<AppointmentService>().Add(new AppointmentService
            {
                AppointmentId = appointment.Id,
                ServiceId = service.Id,
                PriceAtBooking = service.Price
            });
        }
    }

    public async Task ClearDemoDataAsync()
    {
        // Get demo tenant
        var demoTenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Slug == "demo-barber");

        if (demoTenant == null) return;

        // Delete appointments and related data
        var appointments = await _context.Appointments
            .Where(a => a.TenantId == demoTenant.Id)
            .ToListAsync();
        _context.Appointments.RemoveRange(appointments);

        // Delete customers (but not the demo customer user)
        var customers = await _context.Customers
            .Where(c => c.TenantId == demoTenant.Id)
            .ToListAsync();
        _context.Customers.RemoveRange(customers);

        // Delete staff
        var staff = await _context.Staff
            .Where(s => s.TenantId == demoTenant.Id)
            .ToListAsync();
        _context.Staff.RemoveRange(staff);

        // Delete clients and their users
        var clients = await _context.Clients
            .Include(c => c.User)
            .Where(c => c.TenantId == demoTenant.Id)
            .ToListAsync();

        foreach (var client in clients)
        {
            if (client.User != null)
            {
                await _userManager.DeleteAsync(client.User);
            }
        }
        _context.Clients.RemoveRange(clients);

        await _context.SaveChangesAsync();
    }
}
