namespace ElefanteWines.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid WineId { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    // Calculado en C#, NO se persiste en BD (lo configuramos con .Ignore() en Fluent API)
    public decimal Subtotal => UnitPrice * Quantity;

    private OrderItem() { }

    public OrderItem(Guid orderId, Guid wineId, decimal unitPrice, int quantity)
    {
        Id        = Guid.NewGuid();
        OrderId   = orderId;
        WineId    = wineId;
        UnitPrice = unitPrice;
        Quantity  = quantity;
    }
}