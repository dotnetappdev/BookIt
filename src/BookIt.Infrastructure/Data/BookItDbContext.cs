using BookIt.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookIt.Infrastructure.Data;

public class BookItDbContext : DbContext
{
    private readonly Guid? _tenantId;

    public BookItDbContext(DbContextOptions<BookItDbContext> options, ITenantContext? tenantContext = null)
        : base(options)
    {
        _tenantId = tenantContext?.TenantId;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
    public DbSet<ServiceCategory> ServiceCategories => Set<ServiceCategory>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<StaffService> StaffServices => Set<StaffService>();
    public DbSet<StaffAvailability> StaffAvailabilities => Set<StaffAvailability>();
    public DbSet<BusinessHours> BusinessHours => Set<BusinessHours>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<AppointmentService> AppointmentServices => Set<AppointmentService>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<BookingForm> BookingForms => Set<BookingForm>();
    public DbSet<BookingFormField> BookingFormFields => Set<BookingFormField>();
    public DbSet<AppConfiguration> AppConfigurations => Set<AppConfiguration>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Global query filter for soft delete
        modelBuilder.Entity<Tenant>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ApplicationUser>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Service>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ServiceCategory>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Staff>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Appointment>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Payment>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<BookingForm>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<BookingFormField>().HasQueryFilter(e => !e.IsDeleted);

        // StaffService composite key
        modelBuilder.Entity<StaffService>()
            .HasKey(ss => new { ss.StaffId, ss.ServiceId });

        // AppointmentService composite key
        modelBuilder.Entity<AppointmentService>()
            .HasKey(aps => new { aps.AppointmentId, aps.ServiceId });

        // Tenant
        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasIndex(t => t.Slug).IsUnique();
            entity.Property(t => t.Name).HasMaxLength(200).IsRequired();
            entity.Property(t => t.Slug).HasMaxLength(100).IsRequired();
        });

        // ApplicationUser
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.HasIndex(u => new { u.TenantId, u.Email }).IsUnique();
            entity.Property(u => u.Email).HasMaxLength(256).IsRequired();
            entity.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(u => u.LastName).HasMaxLength(100).IsRequired();
        });

        // Service
        modelBuilder.Entity<Service>(entity =>
        {
            entity.Property(s => s.Price).HasColumnType("decimal(18,2)");
            entity.Property(s => s.Name).HasMaxLength(200).IsRequired();
        });

        // Appointment
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.Property(a => a.TotalAmount).HasColumnType("decimal(18,2)");
            entity.Property(a => a.CustomerName).HasMaxLength(200).IsRequired();
            entity.Property(a => a.CustomerEmail).HasMaxLength(256).IsRequired();
            entity.HasIndex(a => new { a.TenantId, a.StartTime });
            entity.HasIndex(a => a.ConfirmationToken);
        });

        // Payment
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(p => p.Amount).HasColumnType("decimal(18,2)");
            entity.Property(p => p.RefundAmount).HasColumnType("decimal(18,2)");
        });

        // AppConfiguration
        modelBuilder.Entity<AppConfiguration>(entity =>
        {
            entity.HasIndex(c => new { c.TenantId, c.Key }).IsUnique();
            entity.Property(c => c.Key).HasMaxLength(200).IsRequired();
        });

        // BusinessHours - handle TimeOnly for SQL Server
        modelBuilder.Entity<BusinessHours>(entity =>
        {
            entity.Property(b => b.OpenTime).HasConversion(
                v => v.ToTimeSpan(),
                v => TimeOnly.FromTimeSpan(v));
            entity.Property(b => b.CloseTime).HasConversion(
                v => v.ToTimeSpan(),
                v => TimeOnly.FromTimeSpan(v));
        });

        // StaffAvailability - handle TimeOnly for SQL Server
        modelBuilder.Entity<StaffAvailability>(entity =>
        {
            entity.Property(b => b.StartTime).HasConversion(
                v => v.ToTimeSpan(),
                v => TimeOnly.FromTimeSpan(v));
            entity.Property(b => b.EndTime).HasConversion(
                v => v.ToTimeSpan(),
                v => TimeOnly.FromTimeSpan(v));
        });

        // Seed data
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var demoTenantId = new Guid("11111111-1111-1111-1111-111111111111");

        modelBuilder.Entity<Tenant>().HasData(new Tenant
        {
            Id = demoTenantId,
            Name = "Demo Barber Shop",
            Slug = "demo-barber",
            BusinessType = Core.Enums.BusinessType.Barber,
            ContactEmail = "demo@bookit.app",
            TimeZone = "Europe/London",
            Currency = "GBP",
            IsActive = true,
            AllowOnlineBooking = true,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });

        // Business hours for demo tenant (Mon-Sat 9am-6pm)
        var days = new[]
        {
            Core.Enums.DayOfWeekFlag.Monday,
            Core.Enums.DayOfWeekFlag.Tuesday,
            Core.Enums.DayOfWeekFlag.Wednesday,
            Core.Enums.DayOfWeekFlag.Thursday,
            Core.Enums.DayOfWeekFlag.Friday,
            Core.Enums.DayOfWeekFlag.Saturday
        };

        var bhIds = new[]
        {
            new Guid("22222222-2222-2222-2222-222222222201"),
            new Guid("22222222-2222-2222-2222-222222222202"),
            new Guid("22222222-2222-2222-2222-222222222203"),
            new Guid("22222222-2222-2222-2222-222222222204"),
            new Guid("22222222-2222-2222-2222-222222222205"),
            new Guid("22222222-2222-2222-2222-222222222206"),
        };

        for (int i = 0; i < days.Length; i++)
        {
            modelBuilder.Entity<BusinessHours>().HasData(new BusinessHours
            {
                Id = bhIds[i],
                TenantId = demoTenantId,
                DayOfWeek = days[i],
                OpenTime = new TimeOnly(9, 0),
                CloseTime = new TimeOnly(18, 0),
                SlotDurationMinutes = 30,
                IsClosed = false,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });
        }

        // Service categories for barber
        var catId1 = new Guid("33333333-3333-3333-3333-333333333301");
        var catId2 = new Guid("33333333-3333-3333-3333-333333333302");

        modelBuilder.Entity<ServiceCategory>().HasData(
            new ServiceCategory { Id = catId1, TenantId = demoTenantId, Name = "Haircuts", SortOrder = 1, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new ServiceCategory { Id = catId2, TenantId = demoTenantId, Name = "Beard & Shave", SortOrder = 2, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        // Services for barber
        var svc1Id = new Guid("44444444-4444-4444-4444-444444444401");
        var svc2Id = new Guid("44444444-4444-4444-4444-444444444402");
        var svc3Id = new Guid("44444444-4444-4444-4444-444444444403");
        var svc4Id = new Guid("44444444-4444-4444-4444-444444444404");

        modelBuilder.Entity<Service>().HasData(
            new Service { Id = svc1Id, TenantId = demoTenantId, CategoryId = catId1, Name = "Mens Haircut", Description = "Classic mens haircut with styling", Price = 25.00m, DurationMinutes = 30, IsActive = true, AllowOnlineBooking = true, SortOrder = 1, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Service { Id = svc2Id, TenantId = demoTenantId, CategoryId = catId1, Name = "Hair & Beard Combo", Description = "Full haircut and beard trim combo", Price = 40.00m, DurationMinutes = 60, IsActive = true, AllowOnlineBooking = true, SortOrder = 2, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Service { Id = svc3Id, TenantId = demoTenantId, CategoryId = catId2, Name = "Beard Trim", Description = "Precision beard trimming and shaping", Price = 15.00m, DurationMinutes = 20, IsActive = true, AllowOnlineBooking = true, SortOrder = 1, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Service { Id = svc4Id, TenantId = demoTenantId, CategoryId = catId2, Name = "Hot Towel Shave", Description = "Traditional hot towel straight razor shave", Price = 35.00m, DurationMinutes = 45, IsActive = true, AllowOnlineBooking = true, SortOrder = 2, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        // Default booking form
        var formId = new Guid("55555555-5555-5555-5555-555555555501");
        modelBuilder.Entity<BookingForm>().HasData(new BookingForm
        {
            Id = formId,
            TenantId = demoTenantId,
            Name = "Standard Booking Form",
            IsDefault = true,
            IsActive = true,
            WelcomeMessage = "Book your appointment online",
            ConfirmationMessage = "Your appointment has been confirmed! We'll send you a reminder before your appointment.",
            CollectPhone = true,
            CollectNotes = true,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BookIt.Core.Entities.BaseEntity>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
