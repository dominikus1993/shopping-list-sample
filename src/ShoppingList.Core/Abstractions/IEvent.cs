namespace ShoppingList.Core.Abstractions;

public interface IEvent
{
    public Guid Id { get; init; }
    public int Version { get; set; }
}