using BookIt.Core.Enums;
using BookIt.Payments.ApplePay;
using BookIt.Payments.Stripe;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace BookIt.Tests.Domain;

public class ApplePayProviderTests
{
    [Fact]
    public async Task CreateApplePayIntentAsync_DelegatesToStripeProvider()
    {
        // Arrange
        var stripeProviderMock = new Mock<IStripeProvider>();
        stripeProviderMock
            .Setup(p => p.CreatePaymentIntentAsync(
                It.IsAny<string>(),
                It.IsAny<long>(),
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>?>()))
            .ReturnsAsync(new PaymentIntentResult("pi_test_123", "secret_abc"));

        var provider = new ApplePayProvider(
            stripeProviderMock.Object,
            NullLogger<ApplePayProvider>.Instance);

        // Act
        var result = await provider.CreateApplePayIntentAsync(
            stripeSecretKey: "sk_test_key",
            amountInSmallestUnit: 1999,
            currency: "GBP");

        // Assert
        Assert.Equal("pi_test_123", result.PaymentIntentId);
        Assert.Equal("secret_abc", result.ClientSecret);

        stripeProviderMock.Verify(p => p.CreatePaymentIntentAsync(
            "sk_test_key",
            1999,
            "GBP",
            It.Is<Dictionary<string, string>>(m => m["payment_method"] == "apple_pay")),
            Times.Once);
    }

    [Fact]
    public async Task RefundAsync_DelegatesToStripeProvider()
    {
        // Arrange
        var stripeProviderMock = new Mock<IStripeProvider>();
        stripeProviderMock
            .Setup(p => p.RefundAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long?>()))
            .Returns(Task.CompletedTask);

        var provider = new ApplePayProvider(
            stripeProviderMock.Object,
            NullLogger<ApplePayProvider>.Instance);

        // Act
        await provider.RefundAsync("sk_test_key", "pi_test_123", 500);

        // Assert
        stripeProviderMock.Verify(p => p.RefundAsync("sk_test_key", "pi_test_123", 500L), Times.Once);
    }

    [Fact]
    public async Task CreateApplePayIntentAsync_IncludesApplePayMetadata()
    {
        // Arrange
        var stripeProviderMock = new Mock<IStripeProvider>();
        Dictionary<string, string>? capturedMeta = null;

        stripeProviderMock
            .Setup(p => p.CreatePaymentIntentAsync(
                It.IsAny<string>(),
                It.IsAny<long>(),
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>?>()))
            .Callback<string, long, string, Dictionary<string, string>?>(
                (_, _, _, meta) => capturedMeta = meta)
            .ReturnsAsync(new PaymentIntentResult("pi_1", "secret_1"));

        var provider = new ApplePayProvider(
            stripeProviderMock.Object,
            NullLogger<ApplePayProvider>.Instance);

        // Act
        await provider.CreateApplePayIntentAsync("sk_key", 500, "GBP");

        // Assert
        Assert.NotNull(capturedMeta);
        Assert.Equal("apple_pay", capturedMeta!["payment_method"]);
    }
}
