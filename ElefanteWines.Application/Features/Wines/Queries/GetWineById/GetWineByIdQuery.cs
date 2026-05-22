using ElefanteWines.Domain.Entities;
using MediatR;

namespace ElefanteWines.Application.Features.Wines.Queries.GetWineById;

// Recibe un Guid (el Id del vino) y devuelve el Wine o null si no existe.
public record GetWineByIdQuery(Guid Id) : IRequest<Wine?>;