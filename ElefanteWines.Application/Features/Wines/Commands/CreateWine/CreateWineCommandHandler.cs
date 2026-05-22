using ElefanteWines.Application.Interfaces;
using ElefanteWines.Domain.Entities;
using MediatR;

namespace ElefanteWines.Application.Features.Wines.Commands.CreateWine;

// Esta clase ES el "Handler" (manejador).
// IRequestHandler<CreateWineCommand, Guid> le dice a MediatR:
// "yo soy el handler que sabe procesar CreateWineCommand y devolver un Guid"
public class CreateWineCommandHandler : IRequestHandler<CreateWineCommand, Guid>
{
    private readonly IWineRepository _wineRepository;

    // El constructor recibe el repositorio por inyección de dependencias.
    // El que se encarga de "pasarnos" el repo es el contenedor DI de .NET.
    public CreateWineCommandHandler(IWineRepository wineRepository)
    {
        _wineRepository = wineRepository;
    }

    // Este método se ejecuta cuando alguien llama a _mediator.Send(command)
    public async Task<Guid> Handle(CreateWineCommand request, CancellationToken cancellationToken)
    {
        // 1. Construimos la entidad del Domain con los datos del Command.
        //    El constructor de Wine valida las reglas de negocio (precio >= 0, etc.)
        var wine = new Wine(
            request.Name,
            request.Description,
            request.Price,
            request.Stock,
            request.CategoryId,
            request.Varietal,
            request.Year);

        // 2. Le pedimos al repositorio que la persista en la BD.
        await _wineRepository.AddAsync(wine, cancellationToken);

        // 3. Devolvemos el Id del vino recién creado.
        return wine.Id;
    }
}