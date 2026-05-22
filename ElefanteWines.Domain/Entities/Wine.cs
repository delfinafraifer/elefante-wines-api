namespace ElefanteWines.Domain.Entities;

public class Wine
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public string? Varietal { get; private set; }   // ej: "Malbec", "Cabernet Sauvignon"
    public int? Year { get; private set; }          // ej: 2021
    public Guid CategoryId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Constructor privado para que EF Core pueda materializar la entidad desde la BD
    private Wine() { }

    // Constructor de negocio: valida las reglas del dominio
    public Wine(string name, string description, decimal price, int stock,
                Guid categoryId, string? varietal = null, int? year = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del vino es obligatorio.");
        if (price < 0)
            throw new ArgumentException("El precio no puede ser negativo.");
        if (stock < 0)
            throw new ArgumentException("El stock no puede ser negativo.");

        Id          = Guid.NewGuid();
        Name        = name;
        Description = description;
        Price       = price;
        Stock       = stock;
        Varietal    = varietal;
        Year        = year;
        CategoryId  = categoryId;
        CreatedAt   = DateTime.UtcNow;
    }

    // Métodos de negocio (NO setters públicos: encapsulamos las reglas)
    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new ArgumentException("El precio no puede ser negativo.");
        Price = newPrice;
    }

    public void ReduceStock(int quantity)
    {
        if (quantity > Stock)
            throw new InvalidOperationException("Stock insuficiente.");
        Stock -= quantity;
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser positiva.");
        Stock += quantity;
    }
}