namespace ShoppingList.Core.Exceptions;

public sealed class ShoppingListNotExistsException : Exception
{
    public ShoppingListId ShoppingListId { get; }
    
    public ShoppingListNotExistsException(ShoppingListId shoppingListId) : base("ShoppingList not exists")
    {
        ShoppingListId = shoppingListId;
    }

    public override string ToString()
    {
        return $"{base.ToString()}, {nameof(ShoppingListId)}: {ShoppingListId}";
    }
}