using System.ComponentModel.DataAnnotations;
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
    public List<PropertyPhotoResponse> Photos { get; set; } = new();
    public List<AmenityResponse> Amenities { get; set; } = new();
}

public class CreateLodgingPropertyRequest
{
    [Required(ErrorMessage = "Property name is required.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Property name must be between 2 and 200 characters.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Description must not exceed 2000 characters.")]
    public string? Description { get; set; }

    [StringLength(500, ErrorMessage = "Address must not exceed 500 characters.")]
    public string? Address { get; set; }

    [StringLength(100, ErrorMessage = "City must not exceed 100 characters.")]
    public string? City { get; set; }

    [StringLength(20, ErrorMessage = "Post code must not exceed 20 characters.")]
    public string? PostCode { get; set; }

    [StringLength(100, ErrorMessage = "Country must not exceed 100 characters.")]
    public string? Country { get; set; }

    [Phone(ErrorMessage = "A valid phone number is required.")]
    [StringLength(30, ErrorMessage = "Phone must not exceed 30 characters.")]
    public string? PhoneNumber { get; set; }

    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [StringLength(254, ErrorMessage = "Email must not exceed 254 characters.")]
    public string? Email { get; set; }

    [Url(ErrorMessage = "Website must be a valid URL.")]
    [StringLength(2000, ErrorMessage = "Website must not exceed 2000 characters.")]
    public string? Website { get; set; }

    [Url(ErrorMessage = "Icon URL must be a valid URL.")]
    [StringLength(2000, ErrorMessage = "Icon URL must not exceed 2000 characters.")]
    public string? IconUrl { get; set; }

    public bool IsActive { get; set; } = true;

    [Range(0, 9999, ErrorMessage = "Sort order must be between 0 and 9999.")]
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
    public int NumberOfBeds { get; set; }
    public BedType BedType { get; set; }
    public bool PetFriendly { get; set; }
    public bool WheelchairAccessible { get; set; }
    public decimal BaseRate { get; set; }
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
    public List<RoomPhotoResponse> Photos { get; set; } = new();
    public List<AmenityResponse> Amenities { get; set; } = new();
}

public class CreateRoomRequest
{
    public Guid? LodgingPropertyId { get; set; }

    [Required(ErrorMessage = "Room name is required.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Room name must be between 2 and 200 characters.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Description must not exceed 2000 characters.")]
    public string? Description { get; set; }

    public RoomType RoomType { get; set; } = RoomType.Double;

    [Range(1, 1000, ErrorMessage = "Capacity must be between 1 and 1000.")]
    public int Capacity { get; set; } = 2;

    [Range(0, 100, ErrorMessage = "Number of beds must be between 0 and 100.")]
    public int NumberOfBeds { get; set; } = 1;

    public BedType BedType { get; set; } = BedType.Double;

    public bool PetFriendly { get; set; } = false;

    public bool WheelchairAccessible { get; set; } = false;

    [Range(0, 1000000, ErrorMessage = "Base rate must be between 0 and 1,000,000.")]
    public decimal BaseRate { get; set; }

    public bool IsActive { get; set; } = true;

    [Range(0, 9999, ErrorMessage = "Sort order must be between 0 and 9999.")]
    public int SortOrder { get; set; }

    public List<Guid> AmenityIds { get; set; } = new();
}

// ── Property Photos ──

public class PropertyPhotoResponse
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; }
}

public class AddPropertyPhotoRequest
{
    [Required(ErrorMessage = "Photo URL is required.")]
    [Url(ErrorMessage = "Photo URL must be a valid URL.")]
    [StringLength(2000, ErrorMessage = "Photo URL must not exceed 2000 characters.")]
    public string Url { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Caption must not exceed 500 characters.")]
    public string? Caption { get; set; }

    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; } = false;
}

// ── Assign amenities to property ──

public class AssignPropertyAmenitiesRequest
{
    public List<Guid> AmenityIds { get; set; } = new();
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
    [Required(ErrorMessage = "Photo URL is required.")]
    [Url(ErrorMessage = "Photo URL must be a valid URL.")]
    [StringLength(2000, ErrorMessage = "Photo URL must not exceed 2000 characters.")]
    public string Url { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Caption must not exceed 500 characters.")]
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
    [Required(ErrorMessage = "Amenity name is required.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Amenity name must be between 2 and 200 characters.")]
    public string Name { get; set; } = string.Empty;

    public AmenityType AmenityType { get; set; } = AmenityType.Other;

    [StringLength(100, ErrorMessage = "Icon must not exceed 100 characters.")]
    public string? Icon { get; set; }

    [StringLength(500, ErrorMessage = "Description must not exceed 500 characters.")]
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
    [Required(ErrorMessage = "Start date is required.")]
    public DateOnly StartDate { get; set; }

    [Required(ErrorMessage = "End date is required.")]
    public DateOnly EndDate { get; set; }

    [Range(0, 1000000, ErrorMessage = "Rate must be between 0 and 1,000,000.")]
    public decimal Rate { get; set; }

    [StringLength(200, ErrorMessage = "Label must not exceed 200 characters.")]
    public string? Label { get; set; }
}

// ── Assign amenities to room ──

public class AssignRoomAmenitiesRequest
{
    public List<Guid> AmenityIds { get; set; } = new();
}

// ── Public Listings ──

public class PublicPropertyListingResponse
{
    public Guid TenantId { get; set; }
    public string TenantSlug { get; set; } = string.Empty;
    public string TenantName { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? IconUrl { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public int RoomCount { get; set; }
    public decimal MinRate { get; set; }
    public decimal MaxRate { get; set; }
    public List<string> AmenityNames { get; set; } = new();
    public List<string> AmenityIcons { get; set; } = new();
    public List<PropertyPhotoResponse> Photos { get; set; } = new();
    public List<RoomListingResponse> Rooms { get; set; } = new();
}

public class RoomListingResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public RoomType RoomType { get; set; }
    public int Capacity { get; set; }
    public int NumberOfBeds { get; set; }
    public BedType BedType { get; set; }
    public bool PetFriendly { get; set; }
    public bool WheelchairAccessible { get; set; }
    public decimal BaseRate { get; set; }
    public string? PrimaryPhotoUrl { get; set; }
    public List<AmenityResponse> Amenities { get; set; } = new();
}
