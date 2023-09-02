namespace ShoppingList.Core.Abstractions;

public interface IEvent
{
    public Guid Id { get; set; }
    public int Version { get; set; }
}