using MediatR;

namespace ElefanteWines.Application.Features.Orders.Commands.CreateOrder;

// El Command recibe el customerId y la lista de items.
public record CreateOrderCommand(
    Guid CustomerId,
    List<CreateOrderItemDto> Items
) : IRequest<Guid>;

// DTO para representar cada item de la orden.
public record CreateOrderItemDto(Guid WineId, int Quantity);