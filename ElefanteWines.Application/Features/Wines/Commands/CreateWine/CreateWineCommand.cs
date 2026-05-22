using MediatR;

namespace ElefanteWines.Application.Features.Wines.Commands.CreateWine;

// Esta clase ES el "Command". Tiene los datos para crear un vino.
// El IRequest<Guid> dice: "este command, cuando se ejecuta, devuelve un Guid"
// (el Id del vino recién creado).
public record CreateWineCommand(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    Guid CategoryId,
    string? Varietal,
    int? Year
) : IRequest<Guid>;