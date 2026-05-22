using ElefanteWines.Application.Interfaces;
using ElefanteWines.Domain.Entities;
using MediatR;

namespace ElefanteWines.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IWineRepository _wineRepository;
    private readonly ICustomerRepository _customerRepository;

    // Inyectamos los 3 repositorios que necesitamos.
    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IWineRepository wineRepository,
        ICustomerRepository customerRepository)
    {
        _orderRepository = orderRepository;
        _wineRepository = wineRepository;
        _customerRepository = customerRepository;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // 1. Verificamos que el cliente exista.
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer is null)
            throw new InvalidOperationException("El cliente no existe.");

        // 2. Creamos la orden vacía.
        var order = new Order(request.CustomerId);

        // 3. Por cada item, buscamos el vino y lo agregamos a la orden.
        //    AddItem internamente descuenta stock vía wine.ReduceStock() (regla del Domain).
        foreach (var itemDto in request.Items)
        {
            var wine = await _wineRepository.GetByIdAsync(itemDto.WineId, cancellationToken);
            if (wine is null)
                throw new InvalidOperationException($"El vino con id {itemDto.WineId} no existe.");

            order.AddItem(wine, itemDto.Quantity);

            // Persistimos el cambio de stock del vino.
            await _wineRepository.UpdateAsync(wine, cancellationToken);
        }

        // 4. Guardamos la orden completa.
        await _orderRepository.AddAsync(order, cancellationToken);

        return order.Id;
    }
}