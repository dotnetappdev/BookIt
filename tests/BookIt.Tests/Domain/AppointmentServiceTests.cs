using BookIt.Core.Entities;
using BookIt.Core.Enums;
using BookIt.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AppointmentService = BookIt.Infrastructure.Services.AppointmentService;

namespace BookIt.Tests.Domain;

public class AppointmentServiceTests
{
    private static BookItDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<BookItDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new BookItDbContext(options);
    }

    [Fact]
    public async Task CreateAppointment_SetsConfirmationToken()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = new AppointmentService(context);
        var appointment = new Appointment
        {
            TenantId = Guid.NewGuid(),
            CustomerName = "John Doe",
            CustomerEmail = "john@test.com",
            StartTime = DateTime.UtcNow.AddDays(1),
            EndTime = DateTime.UtcNow.AddDays(1).AddHours(1),
            TotalAmount = 25.00m
        };

        // Act
        var result = await service.CreateAppointmentAsync(appointment);

        // Assert
        Assert.NotNull(result.ConfirmationToken);
        Assert.Equal(12, result.ConfirmationToken.Length);
    }

    [Fact]
    public async Task CancelAppointment_UpdatesStatus()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var tenantId = Guid.NewGuid();
        var appointment = new Appointment
        {
            TenantId = tenantId,
            CustomerName = "Jane Doe",
            CustomerEmail = "jane@test.com",
            StartTime = DateTime.UtcNow.AddDays(1),
            EndTime = DateTime.UtcNow.AddDays(1).AddHours(1),
            TotalAmount = 30.00m,
            Status = AppointmentStatus.Confirmed
        };
        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        var service = new AppointmentService(context);

        // Act
        await service.CancelAppointmentAsync(tenantId, appointment.Id, "Customer requested cancellation");

        // Assert
        var updated = await context.Appointments.FindAsync(appointment.Id);
        Assert.Equal(AppointmentStatus.Cancelled, updated!.Status);
        Assert.Equal("Customer requested cancellation", updated.CancellationReason);
    }

    [Fact]
    public async Task GetAvailableSlots_ReturnsEmpty_WhenNoBusinessHours()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var tenantId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();

        context.Services.Add(new Service
        {
            Id = serviceId,
            TenantId = tenantId,
            Name = "Test Service",
            Price = 20m,
            DurationMinutes = 60
        });
        await context.SaveChangesAsync();

        var service = new AppointmentService(context);

        // Act
        var slots = await service.GetAvailableSlotsAsync(tenantId, serviceId, null, DateOnly.FromDateTime(DateTime.Today.AddDays(1)));

        // Assert
        Assert.Empty(slots);
    }

    [Fact]
    public async Task GetAvailableSlots_ReturnsSlots_WhenBusinessHoursExist()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var tenantId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();

        // Get next Monday
        var nextMonday = DateOnly.FromDateTime(DateTime.Today);
        while (nextMonday.DayOfWeek != System.DayOfWeek.Monday)
            nextMonday = nextMonday.AddDays(1);
        if (nextMonday == DateOnly.FromDateTime(DateTime.Today))
            nextMonday = nextMonday.AddDays(7);

        context.Services.Add(new Service
        {
            Id = serviceId,
            TenantId = tenantId,
            Name = "Test Haircut",
            Price = 25m,
            DurationMinutes = 30
        });

        context.BusinessHours.Add(new BusinessHours
        {
            TenantId = tenantId,
            DayOfWeek = DayOfWeekFlag.Monday,
            OpenTime = new TimeOnly(9, 0),
            CloseTime = new TimeOnly(17, 0),
            SlotDurationMinutes = 30,
            IsClosed = false
        });
        await context.SaveChangesAsync();

        var service = new AppointmentService(context);

        // Act
        var slots = await service.GetAvailableSlotsAsync(tenantId, serviceId, null, nextMonday);

        // Assert
        Assert.NotEmpty(slots);
        Assert.All(slots, slot =>
        {
            Assert.Equal(nextMonday.Year, slot.Year);
            Assert.Equal(nextMonday.Month, slot.Month);
            Assert.Equal(nextMonday.Day, slot.Day);
        });
    }
}
