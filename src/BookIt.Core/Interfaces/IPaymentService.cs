using BookIt.Core.Enums;

namespace BookIt.Core.Interfaces;

public interface IPaymentService
{
    Task<string> CreatePaymentIntentAsync(Guid tenantId, Guid appointmentId, decimal amount, string currency, PaymentProvider provider);
    Task<bool> ConfirmPaymentAsync(Guid tenantId, string paymentIntentId, PaymentProvider provider);
    Task<bool> RefundPaymentAsync(Guid tenantId, Guid paymentId, decimal? amount = null);
}
