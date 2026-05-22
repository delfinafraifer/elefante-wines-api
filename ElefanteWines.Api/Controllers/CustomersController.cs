using ElefanteWines.Application.Interfaces;
using ElefanteWines.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ElefanteWines.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;

    public CustomersController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    // GET /api/customers
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var customers = await _customerRepository.GetAllAsync(ct);
        return Ok(customers);
    }

    // GET /api/customers/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var customer = await _customerRepository.GetByIdAsync(id, ct);
        if (customer is null) return NotFound(new { message = $"Cliente con id {id} no encontrado." });
        return Ok(customer);
    }

    // POST /api/customers
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto, CancellationToken ct)
    {
        // Verificar que el email no esté ya registrado
        var existing = await _customerRepository.GetByEmailAsync(dto.Email, ct);
        if (existing is not null)
            return BadRequest(new { message = "Ya existe un cliente con ese email." });

        try
        {
            var customer = new Customer(dto.Email, dto.Name);
            await _customerRepository.AddAsync(customer, ct);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

public record CreateCustomerDto(string Email, string Name);