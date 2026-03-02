using BookIt.Core.DTOs;
using BookIt.Core.Enums;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookIt.API.Controllers;

[ApiController]
[Route("api/listings")]
public class PublicListingsController : ControllerBase
{
    private readonly BookItDbContext _context;

    public PublicListingsController(BookItDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all public property listings for Hotel and BedAndBreakfast tenants.
    /// Supports optional filtering by city and price range.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PublicPropertyListingResponse>>> GetListings(
        [FromQuery] string? city = null,
        [FromQuery] decimal? minRate = null,
        [FromQuery] decimal? maxRate = null,
        [FromQuery] string? amenity = null,
        [FromQuery] bool? petFriendly = null,
        [FromQuery] bool? wheelchairAccessible = null,
        [FromQuery] string sortBy = "name")
    {
        var lodgingTypes = new[] { BusinessType.Hotel, BusinessType.BedAndBreakfast };

        var tenantsQuery = _context.Tenants
            .Where(t => !t.IsDeleted && t.IsActive && lodgingTypes.Contains(t.BusinessType));

        var tenants = await tenantsQuery.ToListAsync();

        var results = new List<PublicPropertyListingResponse>();

        foreach (var tenant in tenants)
        {
            var properties = await _context.LodgingProperties
                .Where(p => p.TenantId == tenant.Id && !p.IsDeleted && p.IsActive)
                .Include(p => p.Rooms.Where(r => !r.IsDeleted && r.IsActive))
                    .ThenInclude(r => r.Photos)
                .Include(p => p.Rooms.Where(r => !r.IsDeleted && r.IsActive))
                    .ThenInclude(r => r.RoomAmenities)
                        .ThenInclude(ra => ra.Amenity)
                .ToListAsync();

            foreach (var prop in properties)
            {
                var activeRooms = prop.Rooms.Where(r => !r.IsDeleted && r.IsActive).ToList();
                if (!activeRooms.Any()) continue;

                // Apply city filter
                if (!string.IsNullOrWhiteSpace(city) &&
                    !string.Equals(prop.City, city, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(tenant.City, city, StringComparison.OrdinalIgnoreCase))
                    continue;

                var minBaseRate = activeRooms.Min(r => r.BaseRate);
                var maxBaseRate = activeRooms.Max(r => r.BaseRate);

                // Apply rate filters
                if (minRate.HasValue && maxBaseRate < minRate.Value) continue;
                if (maxRate.HasValue && minBaseRate > maxRate.Value) continue;

                // Apply pet-friendly filter
                if (petFriendly == true && !activeRooms.Any(r => r.PetFriendly)) continue;

                // Apply wheelchair filter
                if (wheelchairAccessible == true && !activeRooms.Any(r => r.WheelchairAccessible)) continue;

                // Collect all amenities across rooms
                var allAmenities = activeRooms
                    .SelectMany(r => r.RoomAmenities)
                    .Where(ra => !ra.Amenity.IsDeleted)
                    .Select(ra => ra.Amenity)
                    .DistinctBy(a => a.Id)
                    .ToList();

                // Apply amenity name filter
                if (!string.IsNullOrWhiteSpace(amenity) &&
                    !allAmenities.Any(a => a.Name.Contains(amenity, StringComparison.OrdinalIgnoreCase)))
                    continue;

                var roomListings = activeRooms.Select(r => new RoomListingResponse
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    RoomType = r.RoomType,
                    Capacity = r.Capacity,
                    NumberOfBeds = r.NumberOfBeds,
                    BedType = r.BedType,
                    PetFriendly = r.PetFriendly,
                    WheelchairAccessible = r.WheelchairAccessible,
                    BaseRate = r.BaseRate,
                    PrimaryPhotoUrl = (r.Photos.FirstOrDefault(p => p.IsPrimary) ?? r.Photos.FirstOrDefault())?.Url,
                    Amenities = r.RoomAmenities.Where(ra => !ra.Amenity.IsDeleted).Select(ra => new AmenityResponse
                    {
                        Id = ra.Amenity.Id,
                        TenantId = ra.Amenity.TenantId,
                        Name = ra.Amenity.Name,
                        AmenityType = ra.Amenity.AmenityType,
                        Icon = ra.Amenity.Icon,
                        Description = ra.Amenity.Description,
                        IsActive = ra.Amenity.IsActive
                    }).ToList()
                }).OrderBy(r => r.BaseRate).ToList();

                results.Add(new PublicPropertyListingResponse
                {
                    TenantId = tenant.Id,
                    TenantSlug = tenant.Slug,
                    TenantName = prop.Name.Length > 0 ? prop.Name : tenant.Name,
                    City = prop.City ?? tenant.City,
                    Country = prop.Country ?? tenant.Country,
                    IconUrl = prop.IconUrl ?? tenant.LogoUrl,
                    Description = prop.Description ?? tenant.Name,
                    Address = prop.Address ?? tenant.Address,
                    PhoneNumber = prop.PhoneNumber ?? tenant.ContactPhone,
                    Email = prop.Email ?? tenant.ContactEmail,
                    Website = prop.Website ?? tenant.Website,
                    RoomCount = activeRooms.Count,
                    MinRate = minBaseRate,
                    MaxRate = maxBaseRate,
                    AmenityNames = allAmenities.Select(a => a.Name).ToList(),
                    AmenityIcons = allAmenities.Where(a => !string.IsNullOrEmpty(a.Icon)).Select(a => a.Icon!).ToList(),
                    Rooms = roomListings
                });
            }
        }

        // Sorting
        results = sortBy?.ToLower() switch
        {
            "price_asc" => results.OrderBy(r => r.MinRate).ToList(),
            "price_desc" => results.OrderByDescending(r => r.MinRate).ToList(),
            "rooms_desc" => results.OrderByDescending(r => r.RoomCount).ToList(),
            _ => results.OrderBy(r => r.TenantName).ToList()
        };

        return Ok(results);
    }
}
