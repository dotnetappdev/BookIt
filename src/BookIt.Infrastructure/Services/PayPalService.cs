using BookIt.Core.Enums;
using BookIt.Core.Interfaces;
using BookIt.Infrastructure.Data;
using BookIt.Payments.PayPal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookIt.Infrastructure.Services;

public class PayPalService : IPayPalService
{
    private readonly BookItDbContext _context;
    private readonly ILogger<PayPalService> _logger;
    private readonly IPayPalProvider _payPalProvider;

    public PayPalService(BookItDbContext context, ILogger<PayPalService> logger, IPayPalProvider payPalProvider)
    {
        _context = context;
        _logger = logger;
        _payPalProvider = payPalProvider;
    }

    public async Task<string> CreateOrderAsync(Guid tenantId, Guid appointmentId, decimal amount, string currency)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.TenantId == tenantId && a.Id == appointmentId);

        if (appointment == null)
            throw new InvalidOperationException("Appointment not found");

        var tenant = await _context.Tenants.FindAsync(tenantId);
        if (tenant == null)
            throw new InvalidOperationException("Tenant not found");

        if (string.IsNullOrEmpty(tenant.PayPalClientId) || string.IsNullOrEmpty(tenant.PayPalClientSecret))
            throw new InvalidOperationException("PayPal is not configured for this tenant");

        try
        {
            var orderId = await _payPalProvider.CreateOrderAsync(
                tenant.PayPalClientId,
                tenant.PayPalClientSecret,
                amount,
                currency,
                appointmentId.ToString(),
                $"BookIt Appointment - {appointment.CustomerName}",
                useSandbox: true);

            var payment = new Core.Entities.Payment
            {
                TenantId = tenantId,
                AppointmentId = appointmentId,
                Amount = amount,
                Currency = currency,
                Provider = PaymentProvider.PayPal,
                Status = PaymentStatus.Pending,
                ProviderPaymentIntentId = orderId
            };
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return orderId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create PayPal order for appointment {AppointmentId}", appointmentId);
            throw;
        }
    }

    public async Task<bool> CaptureOrderAsync(Guid tenantId, string orderId)
    {
        var payment = await _context.Payments
            .Include(p => p.Appointment)
            .FirstOrDefaultAsync(p => p.TenantId == tenantId && p.ProviderPaymentIntentId == orderId);

        if (payment == null) return false;

        var tenant = await _context.Tenants.FindAsync(tenantId);
        if (tenant == null || string.IsNullOrEmpty(tenant.PayPalClientId)) return false;

        try
        {
            var captured = await _payPalProvider.CaptureOrderAsync(
                tenant.PayPalClientId,
                tenant.PayPalClientSecret!,
                orderId,
                useSandbox: true);

            if (captured)
            {
                payment.Status = PaymentStatus.Paid;
                payment.PaidAt = DateTime.UtcNow;
                payment.Appointment.PaymentStatus = PaymentStatus.Paid;
                payment.Appointment.PaymentReference = orderId;
                payment.Appointment.PaymentProvider = PaymentProvider.PayPal;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to capture PayPal order {OrderId}", orderId);
            return false;
        }
    }
}
