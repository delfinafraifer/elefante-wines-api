using ElefanteWines.Application.Features.Wines.Commands.CreateWine;
using ElefanteWines.Application.Features.Wines.Queries.GetAllWines;
using ElefanteWines.Application.Features.Wines.Queries.GetWineById;
using ElefanteWines.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ElefanteWines.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WinesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IWineRepository _wineRepository;  // para operaciones simples (update, delete)

    public WinesController(IMediator mediator, IWineRepository wineRepository)
    {
        _mediator = mediator;
        _wineRepository = wineRepository;
    }

    // GET /api/wines — usa Query
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var wines = await _mediator.Send(new GetAllWinesQuery(), ct);
        return Ok(wines);
    }

    // GET /api/wines/{id} — usa Query
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var wine = await _mediator.Send(new GetWineByIdQuery(id), ct);
        if (wine is null)
            return NotFound(new { message = $"Vino con id {id} no encontrado." });
        return Ok(wine);
    }

    // POST /api/wines — usa Command
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWineCommand command, CancellationToken ct)
    {
        try
        {
            var wineId = await _mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id = wineId }, new { id = wineId });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT /api/wines/{id}/price — operación simple, sigue usando Repository
    [HttpPut("{id:guid}/price")]
    public async Task<IActionResult> UpdatePrice(Guid id, [FromBody] UpdatePriceDto dto, CancellationToken ct)
    {
        var wine = await _wineRepository.GetByIdAsync(id, ct);
        if (wine is null)
            return NotFound(new { message = $"Vino con id {id} no encontrado." });

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

    // DELETE /api/wines/{id} — operación simple, sigue usando Repository
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var exists = await _wineRepository.ExistsAsync(id, ct);
        if (!exists)
            return NotFound(new { message = $"Vino con id {id} no encontrado." });

        await _wineRepository.DeleteAsync(id, ct);
        return NoContent();
    }
}

public record UpdatePriceDto(decimal NewPrice);