using ShoppingList.Core.Abstractions;
using ShoppingList.Core.Exceptions;

namespace ShoppingList.Core.Model;

public sealed record ShoppingListItemName(string Value);
public readonly record struct ShoppingListItemId(Guid Value);

public record struct UserId(Guid Value);

public sealed record ShoppingListName(string Value);

public sealed record ShoppingListItem(ShoppingListItemId Id, ShoppingListItemName Name) : Entity<ShoppingListItemId>(Id);

public enum CustomerShoppingListState
{
    NoActive = 0,
    Active = 1,
}

public sealed class CustomerShoppingList : AggregateRoot
{
    private Dictionary<ShoppingListItemId, ShoppingListItem> _items;
    
    public CustomerShoppingList(ShoppingListId Id, UserId UserId, ShoppingListName ShoppingListName)
    {
        ApplyChange(new ShoppingListCreated(Id, UserId, ShoppingListName));
    }

    public CustomerShoppingList()
    {
        
    }

    public void AddItem(ShoppingListItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (_items.ContainsKey(item.Id))
        {
            throw new ShoppingListItemExistsException(Id, item.Id);
        }
        
        ApplyChange(new ShoppingListItemAdded(item));
    }
    
    public void RemoveItem(ShoppingListItemId itemId)
    {
        if (!_items.TryGetValue(itemId, out var item))
        {
            throw new ShoppingListItemNotExistsException(Id, itemId);
        }
        
        ApplyChange(new ShoppingListItemRemoved(item));
    }

    public void MarkAsUnActive()
    {
        if (State is CustomerShoppingListState.NoActive)
        {
            throw new ShoppingListIsAlereadyUnactiveException(Id);
        }
        ApplyChange(new ShoppingListMarkedAsNoActive(Id));
    }
    
    public Guid Id { get; set; }
    public UserId UserId { get; set; }
    public ShoppingListName ShoppingListName { get; set; }
    public CustomerShoppingListState State { get; set; }
    public IReadOnlyCollection<ShoppingListItem> Items => _items.Values;
    

    protected override void Apply(Event @event)
    {
        switch (@event)
        {
            case ShoppingListCreated created:
                Id = created.ShoppingListIdId;
                UserId = created.UserId;
                ShoppingListName = created.ShoppingListName;
                State = CustomerShoppingListState.Active;
                _items = new Dictionary<ShoppingListItemId, ShoppingListItem>();
                break;
            case ShoppingListItemAdded added:
                _items.Add(added.Item.Id, added.Item);
                break;
            case ShoppingListItemRemoved removed:
                _items.Remove(removed.Item.Id);
                break;
            case ShoppingListMarkedAsNoActive:
                State = CustomerShoppingListState.NoActive;
                break;
        }
    }
}


public sealed record ShoppingListCreated(Guid ShoppingListIdId, UserId UserId,
    ShoppingListName ShoppingListName) : Event;
public sealed record ShoppingListItemAdded(ShoppingListItem Item) : Event;

public sealed record ShoppingListItemRemoved(ShoppingListItem Item) : Event;

public sealed record ShoppingListMarkedAsNoActive(ShoppingListId ShoppingListId) : Event;