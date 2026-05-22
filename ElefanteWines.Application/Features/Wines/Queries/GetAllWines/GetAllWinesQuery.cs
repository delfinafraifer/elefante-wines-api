using ElefanteWines.Domain.Entities;
using MediatR;

namespace ElefanteWines.Application.Features.Wines.Queries.GetAllWines;

// Este Query no necesita datos de entrada (es solo "dame todos los vinos").
// Devuelve una colección de Wine.
public record GetAllWinesQuery() : IRequest<IEnumerable<Wine>>;