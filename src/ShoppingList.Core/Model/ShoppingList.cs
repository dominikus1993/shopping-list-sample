using ShoppingList.Core.Abstractions;

namespace ShoppingList.Core.Model;

public sealed record ShoppingListItemName(string Value);
public sealed record ShoppingListItemId(Guid Value);

public sealed record UserId(Guid Value);

public sealed record ShoppingListName(string Value);

public sealed record ShoppingListItem(ShoppingListItemId Id, ShoppingListItemName Name);

public readonly record struct ShoppingListId(Guid Value);

public sealed class ShoppingList : AggregateRoot
{
    public ShoppingList(ShoppingListId Id, UserId UserId, ShoppingListName ShoppingListName)
    {
        ApplyChange(new ShoppingListCreated(Id, UserId, ShoppingListName, DateTimeOffset.UtcNow));
    }

    public static ShoppingList CreateNew(ShoppingListCreated evt)
    {
        return new ShoppingList(evt.Id, evt.UserId, evt.ShoppingListName);
    }


    public ShoppingListId Id { get; private set; }
    public UserId UserId { get; private set; }
    public ShoppingListName ShoppingListName { get; private set; }

    public void Deconstruct(out ShoppingListId Id, out UserId UserId, out ShoppingListName ShoppingListName)
    {
        Id = this.Id;
        UserId = this.UserId;
        ShoppingListName = this.ShoppingListName;
    }

    protected override void Apply(IEvent @event)
    {
        switch (@event)
        {
            case ShoppingListCreated created:
                Id = created.Id;
                UserId = created.UserId;
                ShoppingListName = created.ShoppingListName;
                break;
        }
    }
}


public sealed record ShoppingListCreated(ShoppingListId Id, UserId UserId, ShoppingListName ShoppingListName, DateTimeOffset Time) : IEvent
{
    private Guid _id;

    Guid IEvent.Id
    {
        get => _id;
        init => _id = value;
    }

    public int Version { get; set; }
}