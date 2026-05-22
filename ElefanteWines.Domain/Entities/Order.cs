namespace ElefanteWines.Domain.Entities;

public enum OrderStatus
{
    Pending,
    Confirmed,
    Shipped,
    Delivered,
    Cancelled
}

public class Order
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal Total { get; private set; }

    // Colección de items: relación 1:N con OrderItem.
    // Privada para que solo se modifique a través de AddItem() (encapsulamiento).
    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() { }  // para EF Core

    public Order(Guid customerId)
    {
        Id         = Guid.NewGuid();
        CustomerId = customerId;
        CreatedAt  = DateTime.UtcNow;
        Status     = OrderStatus.Pending;
        Total      = 0;
    }

    // Agregar un vino a la orden: dispara reglas de negocio
    public void AddItem(Wine wine, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser positiva.");

        // Regla de negocio: descontar stock del vino
        wine.ReduceStock(quantity);

        var item = new OrderItem(Id, wine.Id, wine.Price, quantity);
        _items.Add(item);
        Total += item.Subtotal;
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Solo se pueden confirmar órdenes pendientes.");
        Status = OrderStatus.Confirmed;
    }
}