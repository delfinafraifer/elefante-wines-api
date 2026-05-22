using ElefanteWines.Application.Interfaces;
using ElefanteWines.Domain.Entities;
using MediatR;

namespace ElefanteWines.Application.Features.Wines.Queries.GetAllWines;

public class GetAllWinesQueryHandler : IRequestHandler<GetAllWinesQuery, IEnumerable<Wine>>
{
    private readonly IWineRepository _wineRepository;

    public GetAllWinesQueryHandler(IWineRepository wineRepository)
    {
        _wineRepository = wineRepository;
    }

    public async Task<IEnumerable<Wine>> Handle(GetAllWinesQuery request, CancellationToken cancellationToken)
    {
        // Simplemente delegamos al repositorio.
        // En sistemas más complejos acá podrías mapear a un DTO, aplicar filtros, etc.
        return await _wineRepository.GetAllAsync(cancellationToken);
    }
}