using ElefanteWines.Application.Interfaces;
using ElefanteWines.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ElefanteWines.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WinesController : ControllerBase
{
    private readonly IWineRepository _wineRepository;

    public WinesController(IWineRepository wineRepository)
    {
        _wineRepository = wineRepository;
    }

    // GET /api/wines
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var wines = await _wineRepository.GetAllAsync(ct);
        return Ok(wines);
    }

    // GET /api/wines/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var wine = await _wineRepository.GetByIdAsync(id, ct);
        if (wine is null) return NotFound(new { message = $"Vino con id {id} no encontrado." });
        return Ok(wine);
    }

    // GET /api/wines/search?term=malbec
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string term, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(term))
            return BadRequest(new { message = "El término de búsqueda es obligatorio." });

        var wines = await _wineRepository.SearchByNameAsync(term, ct);
        return Ok(wines);
    }

    // POST /api/wines
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWineDto dto, CancellationToken ct)
    {
        try
        {
            var wine = new Wine(
                dto.Name,
                dto.Description,
                dto.Price,
                dto.Stock,
                dto.CategoryId,
                dto.Varietal,
                dto.Year);

            await _wineRepository.AddAsync(wine, ct);
            return CreatedAtAction(nameof(GetById), new { id = wine.Id }, wine);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT /api/wines/{id}/price
    [HttpPut("{id:guid}/price")]
    public async Task<IActionResult> UpdatePrice(Guid id, [FromBody] UpdatePriceDto dto, CancellationToken ct)
    {
        var wine = await _wineRepository.GetByIdAsync(id, ct);
        if (wine is null) return NotFound(new { message = $"Vino con id {id} no encontrado." });

        try
        {
            wine.UpdatePrice(dto.NewPrice);
            await _wineRepository.UpdateAsync(wine, ct);
            return Ok(wine);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE /api/wines/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var exists = await _wineRepository.ExistsAsync(id, ct);
        if (!exists) return NotFound(new { message = $"Vino con id {id} no encontrado." });

        await _wineRepository.DeleteAsync(id, ct);
        return NoContent();
    }
}

// DTOs (Data Transfer Objects) — clases planas para recibir datos del cliente.
// Los DTOs van acá porque son contratos del API, no del dominio.
public record CreateWineDto(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    Guid CategoryId,
    string? Varietal,
    int? Year);

public record UpdatePriceDto(decimal NewPrice);