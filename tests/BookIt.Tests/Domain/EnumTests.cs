using BookIt.Core.Enums;

namespace BookIt.Tests.Domain;

public class EnumTests
{
    [Fact]
    public void BusinessType_HasExpectedValues()
    {
        Assert.Equal(1, (int)BusinessType.Barber);
        Assert.Equal(2, (int)BusinessType.HairDresser);
        Assert.Equal(3, (int)BusinessType.Salon);
        Assert.Equal(4, (int)BusinessType.MassageTherapy);
        Assert.Equal(5, (int)BusinessType.Spa);
        Assert.Equal(6, (int)BusinessType.Gym);
        Assert.Equal(99, (int)BusinessType.Other);
    }

    [Fact]
    public void AppointmentStatus_HasExpectedValues()
    {
        Assert.Equal(1, (int)AppointmentStatus.Pending);
        Assert.Equal(2, (int)AppointmentStatus.Confirmed);
        Assert.Equal(3, (int)AppointmentStatus.Cancelled);
        Assert.Equal(4, (int)AppointmentStatus.Completed);
    }

    [Fact]
    public void PaymentProvider_HasAllThreePaymentMethods()
    {
        Assert.True(Enum.IsDefined(typeof(PaymentProvider), PaymentProvider.Stripe));
        Assert.True(Enum.IsDefined(typeof(PaymentProvider), PaymentProvider.PayPal));
        Assert.True(Enum.IsDefined(typeof(PaymentProvider), PaymentProvider.ApplePay));
    }

    [Fact]
    public void MeetingType_HasVirtualOptions()
    {
        Assert.True(Enum.IsDefined(typeof(MeetingType), MeetingType.Zoom));
        Assert.True(Enum.IsDefined(typeof(MeetingType), MeetingType.MicrosoftTeams));
        Assert.True(Enum.IsDefined(typeof(MeetingType), MeetingType.GoogleMeet));
        Assert.True(Enum.IsDefined(typeof(MeetingType), MeetingType.InPerson));
    }
}
