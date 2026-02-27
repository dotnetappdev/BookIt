using BookIt.Core.Enums;

namespace BookIt.Core.DTOs;

// ── Lodging Properties ──

public class LodgingPropertyResponse
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostCode { get; set; }
    public string? Country { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? IconUrl { get; set; }
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
    public int RoomCount { get; set; }
}

public class CreateLodgingPropertyRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostCode { get; set; }
    public string? Country { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? IconUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
}

// ── Rooms ──

public class RoomResponse
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid? LodgingPropertyId { get; set; }
    public string? LodgingPropertyName { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public RoomType RoomType { get; set; }
    public int Capacity { get; set; }
    public decimal BaseRate { get; set; }
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
    public List<RoomPhotoResponse> Photos { get; set; } = new();
    public List<AmenityResponse> Amenities { get; set; } = new();
}

public class CreateRoomRequest
{
    public Guid? LodgingPropertyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public RoomType RoomType { get; set; } = RoomType.Double;
    public int Capacity { get; set; } = 2;
    public decimal BaseRate { get; set; }
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
}

// ── Room Photos ──

public class RoomPhotoResponse
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; }
}

public class AddRoomPhotoRequest
{
    public string Url { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; } = false;
}

// ── Amenities ──

public class AmenityResponse
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public AmenityType AmenityType { get; set; }
    public string? Icon { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class CreateAmenityRequest
{
    public string Name { get; set; } = string.Empty;
    public AmenityType AmenityType { get; set; } = AmenityType.Other;
    public string? Icon { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}

// ── Room Rates ──

public class RoomRateResponse
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal Rate { get; set; }
    public string? Label { get; set; }
}

public class CreateRoomRateRequest
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal Rate { get; set; }
    public string? Label { get; set; }
}

// ── Assign amenities to room ──

public class AssignRoomAmenitiesRequest
{
    public List<Guid> AmenityIds { get; set; } = new();
}
