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

public readonly record struct ShoppingListId(Guid Value);

public sealed class ShoppingList : AggregateRoot
{
    private List<ShoppingListItem> _items;
    public ShoppingList(ShoppingListId Id, UserId UserId, ShoppingListName ShoppingListName)
    {
        ApplyChange(new ShoppingListCreated(Id, UserId, ShoppingListName));
    }

    public ShoppingList()
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
    
    public ShoppingListId Id { get; private set; }
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
        }
    }
}


public sealed record ShoppingListCreated(ShoppingListId ShoppingListIdId, UserId UserId,
    ShoppingListName ShoppingListName) : Event;
public sealed record ShoppingListItemAdded(ShoppingListItem Item) : Event;