using BookIt.Core.DTOs;
using BookIt.Core.Entities;
using BookIt.Core.Enums;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookIt.API.Controllers;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = "SuperAdmin")]
public class ClientsController : ControllerBase
{
    private readonly BookItDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ClientsController(BookItDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientResponse>>> GetAllClients()
    {
        var clients = await _context.Clients
            .Include(c => c.Tenant)
            .Include(c => c.Staff)
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.CompanyName)
            .ToListAsync();

        return Ok(clients.Select(MapToResponse));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClientResponse>> GetClient(Guid id)
    {
        var client = await _context.Clients
            .Include(c => c.Tenant)
            .Include(c => c.Staff)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        if (client == null) return NotFound();
        return Ok(MapToResponse(client));
    }

    [HttpPost]
    public async Task<ActionResult<ClientResponse>> CreateClient([FromBody] CreateClientRequest request)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Id == request.TenantId && !t.IsDeleted);
        if (tenant == null)
            return BadRequest(new { message = "Tenant not found" });

        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            return Conflict(new { message = "Email already in use" });

        // Create ApplicationUser for the client
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            TenantId = tenant.Id,
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.ContactName.Split(' ').FirstOrDefault() ?? request.ContactName,
            LastName = request.ContactName.Split(' ').Skip(1).FirstOrDefault() ?? "",
            PhoneNumber = request.Phone,
            Role = UserRole.Customer, // Clients are customers with access to manage their staff
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors.Select(e => e.Description)) });

        await _userManager.AddToRoleAsync(user, "Customer");

        // Create Client record
        var client = new Client
        {
            Id = Guid.NewGuid(),
            TenantId = tenant.Id,
            UserId = user.Id,
            CompanyName = request.CompanyName,
            ContactName = request.ContactName,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
            Notes = request.Notes,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        // Reload with relationships
        client = await _context.Clients
            .Include(c => c.Tenant)
            .Include(c => c.Staff)
            .FirstAsync(c => c.Id == client.Id);

        return CreatedAtAction(nameof(GetClient), new { id = client.Id }, MapToResponse(client));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ClientResponse>> UpdateClient(Guid id, [FromBody] UpdateClientRequest request)
    {
        var client = await _context.Clients
            .Include(c => c.Tenant)
            .Include(c => c.User)
            .Include(c => c.Staff)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        if (client == null) return NotFound();

        // Update client info
        client.CompanyName = request.CompanyName;
        client.ContactName = request.ContactName;
        client.Phone = request.Phone;
        client.Address = request.Address;
        client.Notes = request.Notes;
        client.IsActive = request.IsActive;
        client.UpdatedAt = DateTime.UtcNow;

        // Update user email if changed
        if (client.Email != request.Email)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null && existingUser.Id != client.UserId)
                return Conflict(new { message = "Email already in use" });

            client.User.Email = request.Email;
            client.User.UserName = request.Email;
            await _userManager.UpdateAsync(client.User);
            client.Email = request.Email;
        }

        await _context.SaveChangesAsync();
        return Ok(MapToResponse(client));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(Guid id)
    {
        var client = await _context.Clients
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        if (client == null) return NotFound();

        client.IsDeleted = true;
        client.UpdatedAt = DateTime.UtcNow;

        // Also soft delete the user
        client.User.IsDeleted = true;
        client.User.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static ClientResponse MapToResponse(Client c) => new()
    {
        Id = c.Id,
        TenantId = c.TenantId,
        TenantName = c.Tenant.Name,
        CompanyName = c.CompanyName,
        ContactName = c.ContactName,
        Email = c.Email,
        Phone = c.Phone,
        Address = c.Address,
        Notes = c.Notes,
        IsActive = c.IsActive,
        StaffCount = c.Staff?.Count ?? 0
    };
}
