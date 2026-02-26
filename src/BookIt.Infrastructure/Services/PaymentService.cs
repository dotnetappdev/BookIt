using BookIt.Core.Enums;
using BookIt.Core.Interfaces;
using BookIt.Infrastructure.Data;
using BookIt.Payments.Stripe;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookIt.Infrastructure.Services;

public class StripePaymentService : IPaymentService
{
    private readonly BookItDbContext _context;
    private readonly IStripeProvider _stripeProvider;
    private readonly ILogger<StripePaymentService> _logger;

    public StripePaymentService(BookItDbContext context, IStripeProvider stripeProvider, ILogger<StripePaymentService> logger)
    {
        _context = context;
        _stripeProvider = stripeProvider;
        _logger = logger;
    }

    public async Task<string> CreatePaymentIntentAsync(Guid tenantId, Guid appointmentId, decimal amount, string currency, PaymentProvider provider)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.TenantId == tenantId && a.Id == appointmentId);

        if (appointment == null)
            throw new InvalidOperationException("Appointment not found");

        var tenant = await _context.Tenants.FindAsync(tenantId);
        if (tenant == null)
            throw new InvalidOperationException("Tenant not found");

        if (provider == PaymentProvider.Stripe && !string.IsNullOrEmpty(tenant.StripeSecretKey))
        {
            var metadata = new Dictionary<string, string>
            {
                { "appointment_id", appointmentId.ToString() },
                { "tenant_id", tenantId.ToString() }
            };

            var result = await _stripeProvider.CreatePaymentIntentAsync(
                tenant.StripeSecretKey,
                (long)(amount * 100),
                currency,
                metadata);

            var payment = new Core.Entities.Payment
            {
                TenantId = tenantId,
                AppointmentId = appointmentId,
                Amount = amount,
                Currency = currency,
                Provider = PaymentProvider.Stripe,
                Status = PaymentStatus.Pending,
                ProviderPaymentIntentId = result.PaymentIntentId
            };
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return result.ClientSecret;
        }

        // For PayPal/Apple Pay - return order ID placeholder
        var paymentRecord = new Core.Entities.Payment
        {
            TenantId = tenantId,
            AppointmentId = appointmentId,
            Amount = amount,
            Currency = currency,
            Provider = provider,
            Status = PaymentStatus.Pending
        };
        _context.Payments.Add(paymentRecord);
        await _context.SaveChangesAsync();

        return paymentRecord.Id.ToString();
    }

    public async Task<bool> ConfirmPaymentAsync(Guid tenantId, string paymentIntentId, PaymentProvider provider)
    {
        var payment = await _context.Payments
            .Include(p => p.Appointment)
            .FirstOrDefaultAsync(p => p.TenantId == tenantId && p.ProviderPaymentIntentId == paymentIntentId);

        if (payment == null) return false;

        payment.Status = PaymentStatus.Paid;
        payment.PaidAt = DateTime.UtcNow;
        payment.Appointment.PaymentStatus = PaymentStatus.Paid;
        payment.Appointment.PaymentReference = paymentIntentId;
        payment.Appointment.PaymentProvider = provider;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RefundPaymentAsync(Guid tenantId, Guid paymentId, decimal? amount = null)
    {
        var payment = await _context.Payments
            .Include(p => p.Appointment)
            .FirstOrDefaultAsync(p => p.TenantId == tenantId && p.Id == paymentId);

        if (payment == null) return false;

        if (payment.Provider == PaymentProvider.Stripe && !string.IsNullOrEmpty(payment.ProviderPaymentIntentId))
        {
            var tenant = await _context.Tenants.FindAsync(tenantId);
            if (tenant != null && !string.IsNullOrEmpty(tenant.StripeSecretKey))
            {
                long? amountInPence = amount.HasValue ? (long)(amount.Value * 100) : null;
                await _stripeProvider.RefundAsync(tenant.StripeSecretKey, payment.ProviderPaymentIntentId, amountInPence);
            }
        }

        payment.Status = amount.HasValue ? PaymentStatus.PartiallyRefunded : PaymentStatus.Refunded;
        payment.RefundedAt = DateTime.UtcNow;
        payment.RefundAmount = amount ?? payment.Amount;
        payment.Appointment.PaymentStatus = amount.HasValue ? PaymentStatus.PartiallyRefunded : PaymentStatus.Refunded;

        await _context.SaveChangesAsync();
        return true;
    }
}
