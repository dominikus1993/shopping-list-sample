namespace ShoppingList.Core.Model;

public sealed record ShoppingListItemName(string Value);
public sealed record ShoppingListItemId(Guid Value);

public sealed record UserId(Guid Value);

public sealed record ShoppingListName(string Value);

public sealed record ShoppingListItem(ShoppingListItemId Id, ShoppingListItemName Name);

public readonly record struct ShoppingListId(Guid Value);

public abstract record ShoppingList
{

    public sealed record Empty(ShoppingListId Id, UserId UserId, ShoppingListName ShoppingListName) : ShoppingList;
    
    public static ShoppingList CreateNew(ShoppingListCreated evt) => new Empty(evt.Id, evt.UserId, evt.ShoppingListName);
    
    
}


public sealed record ShoppingListCreated(ShoppingListId Id, UserId UserId, ShoppingListName ShoppingListName, DateTimeOffset Time)
{
    
}