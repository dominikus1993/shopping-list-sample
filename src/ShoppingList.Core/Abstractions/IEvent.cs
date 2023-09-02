namespace ShoppingList.Core.Abstractions;

public abstract record Event
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Version { get; set; } = 1;
}