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
    public void BusinessType_HasLodgingTypes()
    {
        Assert.True(Enum.IsDefined(typeof(BusinessType), BusinessType.BedAndBreakfast));
        Assert.True(Enum.IsDefined(typeof(BusinessType), BusinessType.Hotel));
        Assert.Equal(12, (int)BusinessType.BedAndBreakfast);
        Assert.Equal(13, (int)BusinessType.Hotel);
    }

    [Fact]
    public void RoomType_HasExpectedValues()
    {
        Assert.Equal(1, (int)RoomType.Single);
        Assert.Equal(2, (int)RoomType.Double);
        Assert.Equal(3, (int)RoomType.Twin);
        Assert.Equal(4, (int)RoomType.Suite);
        Assert.Equal(5, (int)RoomType.Family);
        Assert.Equal(6, (int)RoomType.Studio);
        Assert.Equal(7, (int)RoomType.Dormitory);
        Assert.Equal(99, (int)RoomType.Other);
    }

    [Fact]
    public void AmenityType_HasCommonFacilities()
    {
        Assert.True(Enum.IsDefined(typeof(AmenityType), AmenityType.Pool));
        Assert.True(Enum.IsDefined(typeof(AmenityType), AmenityType.Gym));
        Assert.True(Enum.IsDefined(typeof(AmenityType), AmenityType.Sauna));
        Assert.True(Enum.IsDefined(typeof(AmenityType), AmenityType.Spa));
        Assert.True(Enum.IsDefined(typeof(AmenityType), AmenityType.WiFi));
        Assert.True(Enum.IsDefined(typeof(AmenityType), AmenityType.Parking));
        Assert.True(Enum.IsDefined(typeof(AmenityType), AmenityType.Breakfast));
        Assert.True(Enum.IsDefined(typeof(AmenityType), AmenityType.Restaurant));
        Assert.True(Enum.IsDefined(typeof(AmenityType), AmenityType.Bar));
        Assert.True(Enum.IsDefined(typeof(AmenityType), AmenityType.Laundry));
        Assert.True(Enum.IsDefined(typeof(AmenityType), AmenityType.Concierge));
        Assert.True(Enum.IsDefined(typeof(AmenityType), AmenityType.RoomService));
        Assert.True(Enum.IsDefined(typeof(AmenityType), AmenityType.PetFriendly));
        Assert.True(Enum.IsDefined(typeof(AmenityType), AmenityType.AirConditioning));
        Assert.True(Enum.IsDefined(typeof(AmenityType), AmenityType.Heating));
        Assert.True(Enum.IsDefined(typeof(AmenityType), AmenityType.HotTub));
        Assert.True(Enum.IsDefined(typeof(AmenityType), AmenityType.Garden));
        Assert.True(Enum.IsDefined(typeof(AmenityType), AmenityType.Terrace));
        Assert.True(Enum.IsDefined(typeof(AmenityType), AmenityType.SeaView));
        Assert.Equal(99, (int)AmenityType.Other);
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
