namespace BookIt.Core.Interfaces;

public interface IPayPalService
{
    Task<string> CreateOrderAsync(Guid tenantId, Guid appointmentId, decimal amount, string currency);
    Task<bool> CaptureOrderAsync(Guid tenantId, string orderId);
}
