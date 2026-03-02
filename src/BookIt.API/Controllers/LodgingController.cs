using BookIt.Core.DTOs;
using BookIt.Core.Entities;
using BookIt.Core.Interfaces;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookIt.API.Controllers;

[ApiController]
[Route("api/tenants/{tenantSlug}/[controller]")]
public class LodgingController : ControllerBase
{
    private readonly BookItDbContext _context;
    private readonly ITenantService _tenantService;

    public LodgingController(BookItDbContext context, ITenantService tenantService)
    {
        _context = context;
        _tenantService = tenantService;
    }

    private async Task<Tenant?> GetTenantAsync(string tenantSlug)
        => await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);

    // ── Properties ──

    [HttpGet("properties")]
    public async Task<ActionResult<IEnumerable<LodgingPropertyResponse>>> GetProperties(string tenantSlug)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var props = await _context.LodgingProperties
            .Where(p => p.TenantId == tenant.Id && !p.IsDeleted)
            .OrderBy(p => p.SortOrder).ThenBy(p => p.Name)
            .Select(p => new LodgingPropertyResponse
            {
                Id = p.Id,
                TenantId = p.TenantId,
                Name = p.Name,
                Description = p.Description,
                Address = p.Address,
                City = p.City,
                PostCode = p.PostCode,
                Country = p.Country,
                PhoneNumber = p.PhoneNumber,
                Email = p.Email,
                Website = p.Website,
                IconUrl = p.IconUrl,
                IsActive = p.IsActive,
                SortOrder = p.SortOrder,
                MaxOccupancy = p.MaxOccupancy,
                RoomCount = p.Rooms.Count(r => !r.IsDeleted)
            })
            .ToListAsync();

        return Ok(props);
    }

    [HttpGet("properties/{id}")]
    public async Task<ActionResult<LodgingPropertyResponse>> GetProperty(string tenantSlug, Guid id)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var p = await _context.LodgingProperties
            .Include(x => x.Rooms)
            .FirstOrDefaultAsync(x => x.TenantId == tenant.Id && x.Id == id && !x.IsDeleted);
        if (p == null) return NotFound();

        return Ok(new LodgingPropertyResponse
        {
            Id = p.Id,
            TenantId = p.TenantId,
            Name = p.Name,
            Description = p.Description,
            Address = p.Address,
            City = p.City,
            PostCode = p.PostCode,
            Country = p.Country,
            PhoneNumber = p.PhoneNumber,
            Email = p.Email,
            Website = p.Website,
            IconUrl = p.IconUrl,
            IsActive = p.IsActive,
            SortOrder = p.SortOrder,
            MaxOccupancy = p.MaxOccupancy,
            RoomCount = p.Rooms.Count(r => !r.IsDeleted)
        });
    }

    [Authorize]
    [HttpPost("properties")]
    public async Task<ActionResult<LodgingPropertyResponse>> CreateProperty(string tenantSlug, [FromBody] CreateLodgingPropertyRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var prop = new LodgingProperty
        {
            TenantId = tenant.Id,
            Name = request.Name,
            Description = request.Description,
            Address = request.Address,
            City = request.City,
            PostCode = request.PostCode,
            Country = request.Country,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Website = request.Website,
            IconUrl = request.IconUrl,
            IsActive = request.IsActive,
            SortOrder = request.SortOrder,
            MaxOccupancy = request.MaxOccupancy
        };

        _context.LodgingProperties.Add(prop);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProperty), new { tenantSlug, id = prop.Id }, new LodgingPropertyResponse
        {
            Id = prop.Id,
            TenantId = prop.TenantId,
            Name = prop.Name,
            Description = prop.Description,
            Address = prop.Address,
            City = prop.City,
            PostCode = prop.PostCode,
            Country = prop.Country,
            PhoneNumber = prop.PhoneNumber,
            Email = prop.Email,
            Website = prop.Website,
            IconUrl = prop.IconUrl,
            IsActive = prop.IsActive,
            SortOrder = prop.SortOrder,
            MaxOccupancy = prop.MaxOccupancy
        });
    }

    [Authorize]
    [HttpPut("properties/{id}")]
    public async Task<IActionResult> UpdateProperty(string tenantSlug, Guid id, [FromBody] CreateLodgingPropertyRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var prop = await _context.LodgingProperties
            .FirstOrDefaultAsync(p => p.TenantId == tenant.Id && p.Id == id && !p.IsDeleted);
        if (prop == null) return NotFound();

        prop.Name = request.Name;
        prop.Description = request.Description;
        prop.Address = request.Address;
        prop.City = request.City;
        prop.PostCode = request.PostCode;
        prop.Country = request.Country;
        prop.PhoneNumber = request.PhoneNumber;
        prop.Email = request.Email;
        prop.Website = request.Website;
        prop.IconUrl = request.IconUrl;
        prop.IsActive = request.IsActive;
        prop.SortOrder = request.SortOrder;
        prop.MaxOccupancy = request.MaxOccupancy;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize]
    [HttpDelete("properties/{id}")]
    public async Task<IActionResult> DeleteProperty(string tenantSlug, Guid id)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var prop = await _context.LodgingProperties
            .FirstOrDefaultAsync(p => p.TenantId == tenant.Id && p.Id == id && !p.IsDeleted);
        if (prop == null) return NotFound();

        prop.IsDeleted = true;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // ── Rooms ──

    [HttpGet("rooms")]
    public async Task<ActionResult<IEnumerable<RoomResponse>>> GetRooms(string tenantSlug, [FromQuery] Guid? propertyId = null)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var query = _context.Rooms
            .Include(r => r.LodgingProperty)
            .Include(r => r.Photos)
            .Include(r => r.RoomAmenities).ThenInclude(ra => ra.Amenity)
            .Where(r => r.TenantId == tenant.Id && !r.IsDeleted);

        if (propertyId.HasValue)
            query = query.Where(r => r.LodgingPropertyId == propertyId);

        var rooms = await query.OrderBy(r => r.SortOrder).ThenBy(r => r.Name).ToListAsync();

        return Ok(rooms.Select(MapRoomResponse));
    }

    [HttpGet("rooms/{id}")]
    public async Task<ActionResult<RoomResponse>> GetRoom(string tenantSlug, Guid id)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var room = await _context.Rooms
            .Include(r => r.LodgingProperty)
            .Include(r => r.Photos)
            .Include(r => r.RoomAmenities).ThenInclude(ra => ra.Amenity)
            .FirstOrDefaultAsync(r => r.TenantId == tenant.Id && r.Id == id && !r.IsDeleted);

        if (room == null) return NotFound();
        return Ok(MapRoomResponse(room));
    }

    [Authorize]
    [HttpPost("rooms")]
    public async Task<ActionResult<RoomResponse>> CreateRoom(string tenantSlug, [FromBody] CreateRoomRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var room = new Room
        {
            TenantId = tenant.Id,
            LodgingPropertyId = request.LodgingPropertyId,
            Name = request.Name,
            Description = request.Description,
            RoomType = request.RoomType,
            Capacity = request.Capacity,
            BaseRate = request.BaseRate,
            IsActive = request.IsActive,
            SortOrder = request.SortOrder
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRoom), new { tenantSlug, id = room.Id }, MapRoomResponse(room));
    }

    [Authorize]
    [HttpPut("rooms/{id}")]
    public async Task<IActionResult> UpdateRoom(string tenantSlug, Guid id, [FromBody] CreateRoomRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var room = await _context.Rooms
            .FirstOrDefaultAsync(r => r.TenantId == tenant.Id && r.Id == id && !r.IsDeleted);
        if (room == null) return NotFound();

        room.LodgingPropertyId = request.LodgingPropertyId;
        room.Name = request.Name;
        room.Description = request.Description;
        room.RoomType = request.RoomType;
        room.Capacity = request.Capacity;
        room.BaseRate = request.BaseRate;
        room.IsActive = request.IsActive;
        room.SortOrder = request.SortOrder;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize]
    [HttpDelete("rooms/{id}")]
    public async Task<IActionResult> DeleteRoom(string tenantSlug, Guid id)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var room = await _context.Rooms
            .FirstOrDefaultAsync(r => r.TenantId == tenant.Id && r.Id == id && !r.IsDeleted);
        if (room == null) return NotFound();

        room.IsDeleted = true;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // ── Room Photos ──

    [Authorize]
    [HttpPost("rooms/{roomId}/photos")]
    public async Task<ActionResult<RoomPhotoResponse>> AddRoomPhoto(string tenantSlug, Guid roomId, [FromBody] AddRoomPhotoRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var room = await _context.Rooms
            .FirstOrDefaultAsync(r => r.TenantId == tenant.Id && r.Id == roomId && !r.IsDeleted);
        if (room == null) return NotFound();

        var photo = new RoomPhoto
        {
            RoomId = roomId,
            Url = request.Url,
            Caption = request.Caption,
            SortOrder = request.SortOrder,
            IsPrimary = request.IsPrimary
        };

        _context.RoomPhotos.Add(photo);
        await _context.SaveChangesAsync();

        return Ok(new RoomPhotoResponse
        {
            Id = photo.Id,
            Url = photo.Url,
            Caption = photo.Caption,
            SortOrder = photo.SortOrder,
            IsPrimary = photo.IsPrimary
        });
    }

    [Authorize]
    [HttpDelete("rooms/{roomId}/photos/{photoId}")]
    public async Task<IActionResult> DeleteRoomPhoto(string tenantSlug, Guid roomId, Guid photoId)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var room = await _context.Rooms
            .FirstOrDefaultAsync(r => r.TenantId == tenant.Id && r.Id == roomId && !r.IsDeleted);
        if (room == null) return NotFound();

        var photo = await _context.RoomPhotos.FirstOrDefaultAsync(p => p.Id == photoId && p.RoomId == roomId);
        if (photo == null) return NotFound();

        _context.RoomPhotos.Remove(photo);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // ── Amenities ──

    [HttpGet("amenities")]
    public async Task<ActionResult<IEnumerable<AmenityResponse>>> GetAmenities(string tenantSlug)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var amenities = await _context.Amenities
            .Where(a => a.TenantId == tenant.Id && !a.IsDeleted)
            .OrderBy(a => a.Name)
            .Select(a => new AmenityResponse
            {
                Id = a.Id,
                TenantId = a.TenantId,
                Name = a.Name,
                AmenityType = a.AmenityType,
                Icon = a.Icon,
                Description = a.Description,
                IsActive = a.IsActive
            })
            .ToListAsync();

        return Ok(amenities);
    }

    [Authorize]
    [HttpPost("amenities")]
    public async Task<ActionResult<AmenityResponse>> CreateAmenity(string tenantSlug, [FromBody] CreateAmenityRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var amenity = new Amenity
        {
            TenantId = tenant.Id,
            Name = request.Name,
            AmenityType = request.AmenityType,
            Icon = request.Icon,
            Description = request.Description,
            IsActive = request.IsActive
        };

        _context.Amenities.Add(amenity);
        await _context.SaveChangesAsync();

        return Ok(new AmenityResponse
        {
            Id = amenity.Id,
            TenantId = amenity.TenantId,
            Name = amenity.Name,
            AmenityType = amenity.AmenityType,
            Icon = amenity.Icon,
            Description = amenity.Description,
            IsActive = amenity.IsActive
        });
    }

    [Authorize]
    [HttpPut("amenities/{id}")]
    public async Task<IActionResult> UpdateAmenity(string tenantSlug, Guid id, [FromBody] CreateAmenityRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var amenity = await _context.Amenities
            .FirstOrDefaultAsync(a => a.TenantId == tenant.Id && a.Id == id && !a.IsDeleted);
        if (amenity == null) return NotFound();

        amenity.Name = request.Name;
        amenity.AmenityType = request.AmenityType;
        amenity.Icon = request.Icon;
        amenity.Description = request.Description;
        amenity.IsActive = request.IsActive;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize]
    [HttpDelete("amenities/{id}")]
    public async Task<IActionResult> DeleteAmenity(string tenantSlug, Guid id)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var amenity = await _context.Amenities
            .FirstOrDefaultAsync(a => a.TenantId == tenant.Id && a.Id == id && !a.IsDeleted);
        if (amenity == null) return NotFound();

        amenity.IsDeleted = true;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // ── Room Amenity Assignment ──

    [Authorize]
    [HttpPost("rooms/{roomId}/amenities")]
    public async Task<IActionResult> AssignRoomAmenities(string tenantSlug, Guid roomId, [FromBody] AssignRoomAmenitiesRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var room = await _context.Rooms
            .Include(r => r.RoomAmenities)
            .FirstOrDefaultAsync(r => r.TenantId == tenant.Id && r.Id == roomId && !r.IsDeleted);
        if (room == null) return NotFound();

        // Remove old assignments
        _context.RoomAmenities.RemoveRange(room.RoomAmenities);

        // Add new assignments
        foreach (var amenityId in request.AmenityIds.Distinct())
        {
            _context.RoomAmenities.Add(new RoomAmenity { RoomId = roomId, AmenityId = amenityId });
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // ── Room Rates ──

    [HttpGet("rooms/{roomId}/rates")]
    public async Task<ActionResult<IEnumerable<RoomRateResponse>>> GetRoomRates(string tenantSlug, Guid roomId)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var room = await _context.Rooms
            .FirstOrDefaultAsync(r => r.TenantId == tenant.Id && r.Id == roomId && !r.IsDeleted);
        if (room == null) return NotFound();

        var rates = await _context.RoomRates
            .Where(rr => rr.RoomId == roomId)
            .OrderBy(rr => rr.StartDate)
            .Select(rr => new RoomRateResponse
            {
                Id = rr.Id,
                RoomId = rr.RoomId,
                StartDate = rr.StartDate,
                EndDate = rr.EndDate,
                Rate = rr.Rate,
                Label = rr.Label
            })
            .ToListAsync();

        return Ok(rates);
    }

    [Authorize]
    [HttpPost("rooms/{roomId}/rates")]
    public async Task<ActionResult<RoomRateResponse>> CreateRoomRate(string tenantSlug, Guid roomId, [FromBody] CreateRoomRateRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var room = await _context.Rooms
            .FirstOrDefaultAsync(r => r.TenantId == tenant.Id && r.Id == roomId && !r.IsDeleted);
        if (room == null) return NotFound();

        if (request.EndDate < request.StartDate)
            return BadRequest("EndDate must be on or after StartDate.");

        var rate = new RoomRate
        {
            RoomId = roomId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Rate = request.Rate,
            Label = request.Label
        };

        _context.RoomRates.Add(rate);
        await _context.SaveChangesAsync();

        return Ok(new RoomRateResponse
        {
            Id = rate.Id,
            RoomId = rate.RoomId,
            StartDate = rate.StartDate,
            EndDate = rate.EndDate,
            Rate = rate.Rate,
            Label = rate.Label
        });
    }

    [Authorize]
    [HttpDelete("rooms/{roomId}/rates/{rateId}")]
    public async Task<IActionResult> DeleteRoomRate(string tenantSlug, Guid roomId, Guid rateId)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var room = await _context.Rooms
            .FirstOrDefaultAsync(r => r.TenantId == tenant.Id && r.Id == roomId && !r.IsDeleted);
        if (room == null) return NotFound();

        var rate = await _context.RoomRates.FirstOrDefaultAsync(r => r.Id == rateId && r.RoomId == roomId);
        if (rate == null) return NotFound();

        _context.RoomRates.Remove(rate);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // ── Helper ──

    private static RoomResponse MapRoomResponse(Room r) => new()
    {
        Id = r.Id,
        TenantId = r.TenantId,
        LodgingPropertyId = r.LodgingPropertyId,
        LodgingPropertyName = r.LodgingProperty?.Name,
        Name = r.Name,
        Description = r.Description,
        RoomType = r.RoomType,
        Capacity = r.Capacity,
        BaseRate = r.BaseRate,
        IsActive = r.IsActive,
        SortOrder = r.SortOrder,
        Photos = r.Photos.Select(p => new RoomPhotoResponse
        {
            Id = p.Id,
            Url = p.Url,
            Caption = p.Caption,
            SortOrder = p.SortOrder,
            IsPrimary = p.IsPrimary
        }).ToList(),
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
    };
}
