using BookIt.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BookIt.Infrastructure.Data;

public class BookItDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    private readonly Guid? _tenantId;

    public BookItDbContext(DbContextOptions<BookItDbContext> options, ITenantContext? tenantContext = null)
        : base(options)
    {
        _tenantId = tenantContext?.TenantId;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public new DbSet<ApplicationUser> Users => Set<ApplicationUser>();
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
    public DbSet<ClassSession> ClassSessions => Set<ClassSession>();
    public DbSet<ClassSessionBooking> ClassSessionBookings => Set<ClassSessionBooking>();
    public DbSet<InterviewSlot> InterviewSlots => Set<InterviewSlot>();
    public DbSet<CandidateInvitation> CandidateInvitations => Set<CandidateInvitation>();
    public DbSet<EmailTemplate> EmailTemplates => Set<EmailTemplate>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Webhook> Webhooks => Set<Webhook>();
    public DbSet<WebhookDelivery> WebhookDeliveries => Set<WebhookDelivery>();
    public DbSet<StaffInvitation> StaffInvitations => Set<StaffInvitation>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<LodgingProperty> LodgingProperties => Set<LodgingProperty>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<RoomPhoto> RoomPhotos => Set<RoomPhoto>();
    public DbSet<Amenity> Amenities => Set<Amenity>();
    public DbSet<RoomAmenity> RoomAmenities => Set<RoomAmenity>();
    public DbSet<RoomRate> RoomRates => Set<RoomRate>();
    public DbSet<ChatSession> ChatSessions => Set<ChatSession>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Rename Identity tables
        modelBuilder.Entity<ApplicationUser>().ToTable("Users");
        modelBuilder.Entity<IdentityRole<Guid>>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

        // Global query filter for soft delete
        modelBuilder.Entity<ApplicationUser>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Tenant>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Service>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ServiceCategory>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Staff>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Appointment>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Payment>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<BookingForm>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<BookingFormField>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<EmailTemplate>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Webhook>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Customer>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<StaffInvitation>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Client>().HasQueryFilter(e => !e.IsDeleted);

        // StaffService composite key
        modelBuilder.Entity<StaffService>().HasKey(ss => new { ss.StaffId, ss.ServiceId });

        // AppointmentService composite key
        modelBuilder.Entity<AppointmentService>().HasKey(aps => new { aps.AppointmentId, aps.ServiceId });

        // ClassSessionBooking composite key
        modelBuilder.Entity<ClassSessionBooking>().HasKey(csb => new { csb.ClassSessionId, csb.AppointmentId });

        // Tenant
        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasIndex(t => t.Slug).IsUnique();
            entity.Property(t => t.Name).HasMaxLength(200).IsRequired();
            entity.Property(t => t.Slug).HasMaxLength(100).IsRequired();
        });

        // ApplicationUser (Identity-based)
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.HasIndex(u => new { u.TenantId, u.Email }).IsUnique();
            entity.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(u => u.LastName).HasMaxLength(100).IsRequired();
        });

        // Service
        modelBuilder.Entity<Service>(entity =>
        {
            entity.Property(s => s.Price).HasColumnType("decimal(18,2)");
            entity.Property(s => s.Name).HasMaxLength(200).IsRequired();
            entity.Property(s => s.Slug).HasMaxLength(200);
            entity.HasIndex(s => new { s.TenantId, s.Slug }).IsUnique().HasFilter("[Slug] IS NOT NULL");
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

        // AppointmentService price
        modelBuilder.Entity<AppointmentService>(entity =>
        {
            entity.Property(a => a.PriceAtBooking).HasColumnType("decimal(18,2)");
        });

        // AppConfiguration
        modelBuilder.Entity<AppConfiguration>(entity =>
        {
            entity.HasIndex(c => new { c.TenantId, c.Key }).IsUnique();
            entity.Property(c => c.Key).HasMaxLength(200).IsRequired();
        });

        // BusinessHours - handle TimeOnly for SQL Server/SQLite
        modelBuilder.Entity<BusinessHours>(entity =>
        {
            entity.Property(b => b.OpenTime).HasConversion(
                v => v.ToTimeSpan(), v => TimeOnly.FromTimeSpan(v));
            entity.Property(b => b.CloseTime).HasConversion(
                v => v.ToTimeSpan(), v => TimeOnly.FromTimeSpan(v));
        });

        // StaffAvailability - handle TimeOnly
        modelBuilder.Entity<StaffAvailability>(entity =>
        {
            entity.Property(b => b.StartTime).HasConversion(
                v => v.ToTimeSpan(), v => TimeOnly.FromTimeSpan(v));
            entity.Property(b => b.EndTime).HasConversion(
                v => v.ToTimeSpan(), v => TimeOnly.FromTimeSpan(v));
        });

        // ClassSession - handle TimeOnly
        modelBuilder.Entity<ClassSession>(entity =>
        {
            entity.Property(cs => cs.StartTime).HasConversion(
                v => v.ToTimeSpan(), v => TimeOnly.FromTimeSpan(v));
            entity.Property(cs => cs.Price).HasColumnType("decimal(18,2)");
        });

        // InterviewSlot
        modelBuilder.Entity<InterviewSlot>(entity =>
        {
            entity.HasIndex(s => new { s.TenantId, s.SlotStart });
            entity.HasIndex(s => s.IsBooked);
        });

        // CandidateInvitation
        modelBuilder.Entity<CandidateInvitation>(entity =>
        {
            entity.HasIndex(c => c.Token).IsUnique();
            entity.HasIndex(c => new { c.TenantId, c.CandidateEmail });
        });

        // StaffInvitation
        modelBuilder.Entity<StaffInvitation>(entity =>
        {
            entity.HasIndex(s => s.Token).IsUnique();
            entity.HasIndex(s => new { s.TenantId, s.Email });
            entity.HasOne(s => s.Staff)
                  .WithMany()
                  .HasForeignKey(s => s.StaffId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // StaffService - configure to handle filtered entities
        modelBuilder.Entity<StaffService>(entity =>
        {
            entity.HasOne(ss => ss.Staff)
                  .WithMany(s => s.Services)
                  .HasForeignKey(ss => ss.StaffId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(ss => ss.Service)
                  .WithMany()
                  .HasForeignKey(ss => ss.ServiceId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // StaffAvailability - configure relationship
        modelBuilder.Entity<StaffAvailability>(entity =>
        {
            entity.HasOne(sa => sa.Staff)
                  .WithMany(s => s.Availability)
                  .HasForeignKey(sa => sa.StaffId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // InterviewSlot - configure relationship
        modelBuilder.Entity<InterviewSlot>(entity =>
        {
            entity.HasOne(i => i.Service)
                  .WithMany()
                  .HasForeignKey(i => i.ServiceId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // WebhookDelivery - configure relationship
        modelBuilder.Entity<WebhookDelivery>(entity =>
        {
            entity.HasOne(wd => wd.Webhook)
                  .WithMany()
                  .HasForeignKey(wd => wd.WebhookId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // Client
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasIndex(c => new { c.TenantId, c.Email });
            entity.Property(c => c.CompanyName).HasMaxLength(200).IsRequired();
            entity.Property(c => c.ContactName).HasMaxLength(200).IsRequired();
            entity.Property(c => c.Email).HasMaxLength(256).IsRequired();
        });

        // Customer
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(c => c.TotalSpent).HasColumnType("decimal(18,2)");
        });

        // AuditLog
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasIndex(a => new { a.TenantId, a.ChangedAt });
            entity.HasIndex(a => new { a.EntityName, a.EntityId });
            entity.Property(a => a.EntityName).HasMaxLength(200).IsRequired();
            entity.Property(a => a.EntityId).HasMaxLength(200).IsRequired();
            entity.Property(a => a.Action).HasMaxLength(50).IsRequired();
            entity.Property(a => a.ChangedBy).HasMaxLength(256);
        });

        // LodgingProperty
        modelBuilder.Entity<LodgingProperty>(entity =>
        {
            entity.HasQueryFilter(e => !e.IsDeleted);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
        });

        // Room
        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasQueryFilter(e => !e.IsDeleted);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.BaseRate).HasColumnType("decimal(18,2)");
        });

        // RoomRate
        modelBuilder.Entity<RoomRate>(entity =>
        {
            entity.Property(e => e.Rate).HasColumnType("decimal(18,2)");
            entity.Property(e => e.StartDate).HasConversion(
                v => v.ToDateTime(TimeOnly.MinValue), v => DateOnly.FromDateTime(v));
            entity.Property(e => e.EndDate).HasConversion(
                v => v.ToDateTime(TimeOnly.MinValue), v => DateOnly.FromDateTime(v));
        });

        // Amenity
        modelBuilder.Entity<Amenity>(entity =>
        {
            entity.HasQueryFilter(e => !e.IsDeleted);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
        });

        // RoomAmenity composite key
        modelBuilder.Entity<RoomAmenity>().HasKey(ra => new { ra.RoomId, ra.AmenityId });
        modelBuilder.Entity<RoomAmenity>(entity =>
        {
            entity.HasOne(ra => ra.Room)
                  .WithMany(r => r.RoomAmenities)
                  .HasForeignKey(ra => ra.RoomId)
                  .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(ra => ra.Amenity)
                  .WithMany(a => a.RoomAmenities)
                  .HasForeignKey(ra => ra.AmenityId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // ChatSession
        modelBuilder.Entity<ChatSession>(entity =>
        {
            entity.HasIndex(c => new { c.TenantId, c.SessionId }).IsUnique();
            entity.HasIndex(c => c.LastMessageAt);
            entity.Property(c => c.SessionId).HasMaxLength(100).IsRequired();
            entity.Property(c => c.CustomerName).HasMaxLength(200);
            entity.Property(c => c.CustomerEmail).HasMaxLength(256);
        });

        // ChatMessage
        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasIndex(m => m.ChatSessionId);
            entity.Property(m => m.Role).HasMaxLength(20).IsRequired();
            entity.HasOne(m => m.ChatSession)
                  .WithMany(s => s.Messages)
                  .HasForeignKey(m => m.ChatSessionId)
                  .OnDelete(DeleteBehavior.Cascade);
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

        // Seed Identity Roles
        var superAdminRoleId = new Guid("aa000000-0000-0000-0000-000000000001");
        var tenantAdminRoleId = new Guid("aa000000-0000-0000-0000-000000000002");
        var managerRoleId = new Guid("aa000000-0000-0000-0000-000000000005");
        var staffRoleId = new Guid("aa000000-0000-0000-0000-000000000003");
        var customerRoleId = new Guid("aa000000-0000-0000-0000-000000000004");

        modelBuilder.Entity<IdentityRole<Guid>>().HasData(
            new IdentityRole<Guid> { Id = superAdminRoleId, Name = "SuperAdmin", NormalizedName = "SUPERADMIN", ConcurrencyStamp = "1" },
            new IdentityRole<Guid> { Id = tenantAdminRoleId, Name = "TenantAdmin", NormalizedName = "TENANTADMIN", ConcurrencyStamp = "2" },
            new IdentityRole<Guid> { Id = managerRoleId, Name = "Manager", NormalizedName = "MANAGER", ConcurrencyStamp = "5" },
            new IdentityRole<Guid> { Id = staffRoleId, Name = "Staff", NormalizedName = "STAFF", ConcurrencyStamp = "3" },
            new IdentityRole<Guid> { Id = customerRoleId, Name = "Customer", NormalizedName = "CUSTOMER", ConcurrencyStamp = "4" }
        );

        // Seed admin user for demo tenant (password: Admin123!)
        var adminUserId = new Guid("bb000000-0000-0000-0000-000000000001");
        var managerUserId = new Guid("bb000000-0000-0000-0000-000000000004");
        var staffUserId = new Guid("bb000000-0000-0000-0000-000000000002");
        var customerUserId = new Guid("bb000000-0000-0000-0000-000000000003");

        var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<ApplicationUser>();

        var adminUser = new ApplicationUser
        {
            Id = adminUserId,
            TenantId = demoTenantId,
            Email = "admin@demo-barber.com",
            NormalizedEmail = "ADMIN@DEMO-BARBER.COM",
            UserName = "admin@demo-barber.com",
            NormalizedUserName = "ADMIN@DEMO-BARBER.COM",
            FirstName = "Demo",
            LastName = "Admin",
            Role = Core.Enums.UserRole.TenantAdmin,
            IsDeleted = false,
            EmailConfirmed = true,
            SecurityStamp = "admin-security-stamp-1",
            ConcurrencyStamp = "admin-concurrency-stamp-1",
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };
        adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123!");

        var managerUser = new ApplicationUser
        {
            Id = managerUserId,
            TenantId = demoTenantId,
            Email = "manager@demo-barber.com",
            NormalizedEmail = "MANAGER@DEMO-BARBER.COM",
            UserName = "manager@demo-barber.com",
            NormalizedUserName = "MANAGER@DEMO-BARBER.COM",
            FirstName = "Sarah",
            LastName = "Manager",
            Role = Core.Enums.UserRole.Manager,
            IsDeleted = false,
            EmailConfirmed = true,
            SecurityStamp = "manager-security-stamp-1",
            ConcurrencyStamp = "manager-concurrency-stamp-1",
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };
        managerUser.PasswordHash = hasher.HashPassword(managerUser, "Manager123!");

        var staffUser = new ApplicationUser
        {
            Id = staffUserId,
            TenantId = demoTenantId,
            Email = "staff@demo-barber.com",
            NormalizedEmail = "STAFF@DEMO-BARBER.COM",
            UserName = "staff@demo-barber.com",
            NormalizedUserName = "STAFF@DEMO-BARBER.COM",
            FirstName = "John",
            LastName = "Barber",
            Role = Core.Enums.UserRole.Staff,
            IsDeleted = false,
            EmailConfirmed = true,
            SecurityStamp = "staff-security-stamp-1",
            ConcurrencyStamp = "staff-concurrency-stamp-1",
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };
        staffUser.PasswordHash = hasher.HashPassword(staffUser, "Staff123!");

        var customerUser = new ApplicationUser
        {
            Id = customerUserId,
            TenantId = demoTenantId,
            Email = "customer@example.com",
            NormalizedEmail = "CUSTOMER@EXAMPLE.COM",
            UserName = "customer@example.com",
            NormalizedUserName = "CUSTOMER@EXAMPLE.COM",
            FirstName = "Jane",
            LastName = "Customer",
            Role = Core.Enums.UserRole.Customer,
            IsDeleted = false,
            EmailConfirmed = true,
            SecurityStamp = "customer-security-stamp-1",
            ConcurrencyStamp = "customer-concurrency-stamp-1",
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };
        customerUser.PasswordHash = hasher.HashPassword(customerUser, "Customer123!");

        modelBuilder.Entity<ApplicationUser>().HasData(adminUser, managerUser, staffUser, customerUser);

        // Seed UserRoles
        modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid> { UserId = adminUserId, RoleId = tenantAdminRoleId },
            new IdentityUserRole<Guid> { UserId = managerUserId, RoleId = managerRoleId },
            new IdentityUserRole<Guid> { UserId = staffUserId, RoleId = staffRoleId },
            new IdentityUserRole<Guid> { UserId = customerUserId, RoleId = customerRoleId }
        );

        // Business hours
        var days = new[] {
            Core.Enums.DayOfWeekFlag.Monday, Core.Enums.DayOfWeekFlag.Tuesday, Core.Enums.DayOfWeekFlag.Wednesday,
            Core.Enums.DayOfWeekFlag.Thursday, Core.Enums.DayOfWeekFlag.Friday, Core.Enums.DayOfWeekFlag.Saturday
        };
        var bhIds = new[]
        {
            new Guid("22222222-2222-2222-2222-222222222201"), new Guid("22222222-2222-2222-2222-222222222202"),
            new Guid("22222222-2222-2222-2222-222222222203"), new Guid("22222222-2222-2222-2222-222222222204"),
            new Guid("22222222-2222-2222-2222-222222222205"), new Guid("22222222-2222-2222-2222-222222222206"),
        };
        for (int i = 0; i < days.Length; i++)
        {
            modelBuilder.Entity<BusinessHours>().HasData(new BusinessHours
            {
                Id = bhIds[i], TenantId = demoTenantId, DayOfWeek = days[i],
                OpenTime = new TimeOnly(9, 0), CloseTime = new TimeOnly(18, 0),
                SlotDurationMinutes = 30, IsClosed = false,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });
        }

        // Categories and services
        var catId1 = new Guid("33333333-3333-3333-3333-333333333301");
        var catId2 = new Guid("33333333-3333-3333-3333-333333333302");
        modelBuilder.Entity<ServiceCategory>().HasData(
            new ServiceCategory { Id = catId1, TenantId = demoTenantId, Name = "Haircuts", SortOrder = 1, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new ServiceCategory { Id = catId2, TenantId = demoTenantId, Name = "Beard & Shave", SortOrder = 2, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        var svc1Id = new Guid("44444444-4444-4444-4444-444444444401");
        var svc2Id = new Guid("44444444-4444-4444-4444-444444444402");
        var svc3Id = new Guid("44444444-4444-4444-4444-444444444403");
        var svc4Id = new Guid("44444444-4444-4444-4444-444444444404");
        modelBuilder.Entity<Service>().HasData(
            new Service { Id = svc1Id, TenantId = demoTenantId, CategoryId = catId1, Name = "Mens Haircut", Slug = "mens-haircut", Description = "Classic mens haircut with styling", Price = 25.00m, DurationMinutes = 30, IsActive = true, AllowOnlineBooking = true, SortOrder = 1, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Service { Id = svc2Id, TenantId = demoTenantId, CategoryId = catId1, Name = "Hair & Beard Combo", Slug = "hair-beard-combo", Description = "Full haircut and beard trim combo", Price = 40.00m, DurationMinutes = 60, IsActive = true, AllowOnlineBooking = true, SortOrder = 2, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Service { Id = svc3Id, TenantId = demoTenantId, CategoryId = catId2, Name = "Beard Trim", Slug = "beard-trim", Description = "Precision beard trimming and shaping", Price = 15.00m, DurationMinutes = 20, IsActive = true, AllowOnlineBooking = true, SortOrder = 1, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Service { Id = svc4Id, TenantId = demoTenantId, CategoryId = catId2, Name = "Hot Towel Shave", Slug = "hot-towel-shave", Description = "Traditional hot towel straight razor shave", Price = 35.00m, DurationMinutes = 45, IsActive = true, AllowOnlineBooking = true, SortOrder = 2, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        // Default booking form
        var formId = new Guid("55555555-5555-5555-5555-555555555501");
        modelBuilder.Entity<BookingForm>().HasData(new BookingForm
        {
            Id = formId, TenantId = demoTenantId, Name = "Standard Booking Form", IsDefault = true, IsActive = true,
            WelcomeMessage = "Book your appointment online",
            ConfirmationMessage = "Your appointment has been confirmed! We'll send you a reminder before your appointment.",
            CollectPhone = true, CollectNotes = true,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }

    public string? CurrentUserEmail { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var auditEntries = new List<AuditLog>();

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.CreatedBy = CurrentUserEmail;
                auditEntries.Add(BuildAuditLog(entry, "Created", now));
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
                entry.Entity.UpdatedBy = CurrentUserEmail;

                // If IsDeleted just flipped to true, stamp DeletedAt/DeletedBy
                var isDeletedProp = entry.Property(nameof(BaseEntity.IsDeleted));
                if (isDeletedProp.IsModified && entry.Entity.IsDeleted)
                {
                    entry.Entity.DeletedAt = now;
                    entry.Entity.DeletedBy = CurrentUserEmail;
                    auditEntries.Add(BuildAuditLog(entry, "Deleted", now));
                }
                else
                {
                    entry.Entity.EditedAt = now;
                    entry.Entity.EditedBy = CurrentUserEmail;
                    auditEntries.Add(BuildAuditLog(entry, "Updated", now));
                }
            }
        }

        if (auditEntries.Any())
            AuditLogs.AddRange(auditEntries);

        return base.SaveChangesAsync(cancellationToken);
    }

    private AuditLog BuildAuditLog(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<BaseEntity> entry, string action, DateTime now)
    {
        var entityName = entry.Metadata.GetTableName() ?? entry.Metadata.ClrType.Name;
        var entityId = entry.Entity.Id.ToString();

        var newValues = new Dictionary<string, object?>();
        Dictionary<string, object?>? oldValues = null;

        foreach (var prop in entry.Properties)
        {
            if (prop.Metadata.IsKey()) continue;
            if (action == "Updated" && !prop.IsModified) continue;

            newValues[prop.Metadata.Name] = prop.CurrentValue;
            if (action == "Updated")
            {
                oldValues ??= new Dictionary<string, object?>();
                oldValues[prop.Metadata.Name] = prop.OriginalValue;
            }
        }

        // Resolve TenantId if entity exposes one
        Guid? tenantId = null;
        var tenantProp = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "TenantId");
        if (tenantProp?.CurrentValue is Guid tid)
            tenantId = tid;

        return new AuditLog
        {
            TenantId = tenantId,
            EntityName = entityName,
            EntityId = entityId,
            Action = action,
            ChangedBy = CurrentUserEmail,
            ChangedAt = now,
            OldValues = oldValues == null ? null : JsonSerializer.Serialize(oldValues),
            NewValues = JsonSerializer.Serialize(newValues)
        };
    }
}
