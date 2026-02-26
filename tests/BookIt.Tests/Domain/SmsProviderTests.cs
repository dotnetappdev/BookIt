using BookIt.Notifications.Sms;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Net;

namespace BookIt.Tests.Domain;

public class SmsProviderTests
{
    [Fact]
    public void SmsProviderFactory_DefaultsToClickSend()
    {
        var httpFactory = new Mock<IHttpClientFactory>();
        var clickSend = new ClickSendSmsProvider(httpFactory.Object, NullLogger<ClickSendSmsProvider>.Instance);
        var twilio = new TwilioSmsProvider(httpFactory.Object, NullLogger<TwilioSmsProvider>.Instance);
        var factory = new SmsProviderFactory(clickSend, twilio);

        var provider = factory.Get("unknown_provider");

        Assert.IsType<ClickSendSmsProvider>(provider);
    }

    [Fact]
    public void SmsProviderFactory_ReturnsTwilio_ForTwilioName()
    {
        var httpFactory = new Mock<IHttpClientFactory>();
        var clickSend = new ClickSendSmsProvider(httpFactory.Object, NullLogger<ClickSendSmsProvider>.Instance);
        var twilio = new TwilioSmsProvider(httpFactory.Object, NullLogger<TwilioSmsProvider>.Instance);
        var factory = new SmsProviderFactory(clickSend, twilio);

        var provider = factory.Get("twilio");

        Assert.IsType<TwilioSmsProvider>(provider);
    }

    [Fact]
    public void SmsProviderFactory_ReturnsTwilio_CaseInsensitive()
    {
        var httpFactory = new Mock<IHttpClientFactory>();
        var clickSend = new ClickSendSmsProvider(httpFactory.Object, NullLogger<ClickSendSmsProvider>.Instance);
        var twilio = new TwilioSmsProvider(httpFactory.Object, NullLogger<TwilioSmsProvider>.Instance);
        var factory = new SmsProviderFactory(clickSend, twilio);

        Assert.IsType<TwilioSmsProvider>(factory.Get("TWILIO"));
        Assert.IsType<TwilioSmsProvider>(factory.Get("Twilio"));
    }

    [Fact]
    public async Task ClickSendSmsProvider_ReturnsError_WhenInvalidCredentialFormat()
    {
        var httpFactory = new Mock<IHttpClientFactory>();
        var provider = new ClickSendSmsProvider(httpFactory.Object, NullLogger<ClickSendSmsProvider>.Instance);

        // fromNumber without colon → invalid format
        var result = await provider.SendAsync("+447700900000", "Hello", "INVALID_NO_COLON");

        Assert.False(result.Success);
        Assert.NotNull(result.ErrorMessage);
        Assert.Contains("ClickSend", result.ErrorMessage);
    }

    [Fact]
    public async Task TwilioSmsProvider_ReturnsError_WhenInvalidCredentialFormat()
    {
        var httpFactory = new Mock<IHttpClientFactory>();
        var provider = new TwilioSmsProvider(httpFactory.Object, NullLogger<TwilioSmsProvider>.Instance);

        // fromNumber with only two parts → invalid for Twilio
        var result = await provider.SendAsync("+447700900000", "Hello", "SID:TOKEN");

        Assert.False(result.Success);
        Assert.NotNull(result.ErrorMessage);
        Assert.Contains("Twilio", result.ErrorMessage);
    }

    [Fact]
    public void SmsSendResult_SuccessRecord()
    {
        var result = new SmsSendResult(true, "msg_id_123", null);
        Assert.True(result.Success);
        Assert.Equal("msg_id_123", result.MessageId);
        Assert.Null(result.ErrorMessage);
    }

    [Fact]
    public void SmsSendResult_FailureRecord()
    {
        var result = new SmsSendResult(false, null, "HTTP 401");
        Assert.False(result.Success);
        Assert.Null(result.MessageId);
        Assert.Equal("HTTP 401", result.ErrorMessage);
    }
}
