using ShoppingList.Core.Abstractions;
using ShoppingList.Core.Exceptions;

namespace ShoppingList.Core.Model;

public sealed record ShoppingListItemName(string Value);
public sealed record ShoppingListItemId(Guid Value);

public sealed record UserId(Guid Value);

public sealed record ShoppingListName(string Value);

public sealed record ShoppingListItem(ShoppingListItemId Id, ShoppingListItemName Name)
{
    public bool Equals(ShoppingListItem? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}


public sealed class CustomerShoppingList : AggregateRoot
{
    private List<ShoppingListItem> _items;
    public CustomerShoppingList(Guid Id, UserId UserId, ShoppingListName ShoppingListName)
    {
        ApplyChange(new ShoppingListCreated(Id, UserId, ShoppingListName));
    }

    public CustomerShoppingList()
    {
        
    }

    public void AddItem(ShoppingListItem item)
    {
        if (_items.Contains(item))
        {
            throw new ShoppingListItemExistsException(item.Id);
        }
        
        ApplyChange(new ShoppingListItemAdded(item));
    }
    
    public void RemoveItem(ShoppingListItemId itemId)
    {
        var element = _items.Find(it => it.Id == itemId);
        if (element is null)
        {
            throw new ShoppingListItemNotExistsException(itemId);
        }
        
        ApplyChange(new ShoppingListItemRemoved(element));
    }
    
    public Guid Id { get; private set; }
    public UserId UserId { get; private set; }
    public ShoppingListName ShoppingListName { get; private set; }

    public IReadOnlyCollection<ShoppingListItem> Items => _items.AsReadOnly();
    

    protected override void Apply(Event @event)
    {
        switch (@event)
        {
            case ShoppingListCreated created:
                Id = created.ShoppingListIdId;
                UserId = created.UserId;
                ShoppingListName = created.ShoppingListName;
                _items = new List<ShoppingListItem>();
                break;
            case ShoppingListItemAdded added: 
                _items.Add(added.Item);
                break;
            case ShoppingListItemRemoved removed:
                _items.Remove(removed.Item);
                break;
        }
    }
}


public sealed record ShoppingListCreated(Guid ShoppingListIdId, UserId UserId,
    ShoppingListName ShoppingListName) : Event;
public sealed record ShoppingListItemAdded(ShoppingListItem Item) : Event;

public sealed record ShoppingListItemRemoved(ShoppingListItem Item) : Event;