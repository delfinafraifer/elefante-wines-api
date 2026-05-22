namespace ElefanteWines.Domain.Entities;

public class Customer
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private Customer() { }

    public Customer(string email, string name)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email es obligatorio.");
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre es obligatorio.");

        Id        = Guid.NewGuid();
        Email     = email;
        Name      = name;
        CreatedAt = DateTime.UtcNow;
    }
}