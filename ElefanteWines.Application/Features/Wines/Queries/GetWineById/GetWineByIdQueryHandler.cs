using ElefanteWines.Application.Interfaces;
using ElefanteWines.Domain.Entities;
using MediatR;

namespace ElefanteWines.Application.Features.Wines.Queries.GetWineById;

public class GetWineByIdQueryHandler : IRequestHandler<GetWineByIdQuery, Wine?>
{
    private readonly IWineRepository _wineRepository;

    public GetWineByIdQueryHandler(IWineRepository wineRepository)
    {
        _wineRepository = wineRepository;
    }

    public async Task<Wine?> Handle(GetWineByIdQuery request, CancellationToken cancellationToken)
    {
        return await _wineRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}