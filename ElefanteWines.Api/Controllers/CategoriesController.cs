using ElefanteWines.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ElefanteWines.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    // GET /api/categories
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var categories = await _categoryRepository.GetAllAsync(ct);
        return Ok(categories);
    }

    // GET /api/categories/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var category = await _categoryRepository.GetByIdAsync(id, ct);
        if (category is null) return NotFound(new { message = $"Categoría con id {id} no encontrada." });
        return Ok(category);
    }
}