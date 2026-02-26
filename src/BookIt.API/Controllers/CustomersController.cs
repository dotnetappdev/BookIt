using BookIt.Core.DTOs;
using BookIt.Core.Entities;
using BookIt.Core.Interfaces;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookIt.API.Controllers;

[Authorize]
[ApiController]
[Route("api/tenants/{tenantSlug}/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly BookItDbContext _context;
    private readonly ITenantService _tenantService;
    private readonly IWebhookService _webhookService;

    public CustomersController(BookItDbContext context, ITenantService tenantService, IWebhookService webhookService)
    {
        _context = context;
        _tenantService = tenantService;
        _webhookService = webhookService;
    }

    private async Task<Tenant?> GetTenantAsync(string tenantSlug)
        => await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerResponse>>> GetCustomers(string tenantSlug)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var customers = await _context.Customers
            .Where(c => c.TenantId == tenant.Id)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        return Ok(customers.Select(MapToResponse));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponse>> GetCustomer(string tenantSlug, Guid id)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id && c.TenantId == tenant.Id);
        if (customer == null) return NotFound();

        return Ok(MapToResponse(customer));
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> CreateCustomer(string tenantSlug, [FromBody] CreateCustomerRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var customer = new Customer
        {
            TenantId = tenant.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            Mobile = request.Mobile,
            Address = request.Address,
            City = request.City,
            PostCode = request.PostCode,
            Country = request.Country,
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            Notes = request.Notes,
            Tags = request.Tags,
            MembershipNumber = request.MembershipNumber,
            MarketingOptIn = request.MarketingOptIn,
            SmsOptIn = request.SmsOptIn
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        await _webhookService.FireAsync(tenant.Id, "customer.created", new
        {
            customer.Id,
            customer.Email,
            FullName = $"{customer.FirstName} {customer.LastName}".Trim()
        });

        return CreatedAtAction(nameof(GetCustomer), new { tenantSlug, id = customer.Id }, MapToResponse(customer));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CustomerResponse>> UpdateCustomer(string tenantSlug, Guid id, [FromBody] UpdateCustomerRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id && c.TenantId == tenant.Id);
        if (customer == null) return NotFound();

        customer.FirstName = request.FirstName;
        customer.LastName = request.LastName;
        customer.Email = request.Email;
        customer.Phone = request.Phone;
        customer.Mobile = request.Mobile;
        customer.Address = request.Address;
        customer.City = request.City;
        customer.PostCode = request.PostCode;
        customer.Country = request.Country;
        customer.DateOfBirth = request.DateOfBirth;
        customer.Gender = request.Gender;
        customer.Notes = request.Notes;
        customer.Tags = request.Tags;
        customer.MembershipNumber = request.MembershipNumber;
        customer.MarketingOptIn = request.MarketingOptIn;
        customer.SmsOptIn = request.SmsOptIn;
        customer.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await _webhookService.FireAsync(tenant.Id, "customer.updated", new
        {
            customer.Id,
            customer.Email,
            FullName = $"{customer.FirstName} {customer.LastName}".Trim()
        });

        return Ok(MapToResponse(customer));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(string tenantSlug, Guid id)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id && c.TenantId == tenant.Id);
        if (customer == null) return NotFound();

        customer.IsDeleted = true;
        customer.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        await _webhookService.FireAsync(tenant.Id, "customer.deleted", new { customer.Id });

        return NoContent();
    }

    private static CustomerResponse MapToResponse(Customer c) => new()
    {
        Id = c.Id,
        TenantId = c.TenantId,
        FirstName = c.FirstName,
        LastName = c.LastName,
        Email = c.Email,
        Phone = c.Phone,
        Mobile = c.Mobile,
        Address = c.Address,
        City = c.City,
        PostCode = c.PostCode,
        Country = c.Country,
        DateOfBirth = c.DateOfBirth,
        Gender = c.Gender,
        Notes = c.Notes,
        Tags = c.Tags,
        MembershipNumber = c.MembershipNumber,
        MarketingOptIn = c.MarketingOptIn,
        SmsOptIn = c.SmsOptIn,
        TotalBookings = c.TotalBookings,
        TotalSpent = c.TotalSpent,
        LastVisit = c.LastVisit,
        CreatedAt = c.CreatedAt
    };
}
