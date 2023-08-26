namespace ShoppingList.Core.Model;

public sealed class ShoppingListItem
{
    
}

public readonly record struct ShoppingListId(Guid Value);

public sealed record ShoppingList(ShoppingListId Id)
{
    public static ShoppingList CreateNew(ShoppingListCreated evt) => new ShoppingList(evt.Id);
}


public sealed record ShoppingListCreated(ShoppingListId Id)
{
    
}