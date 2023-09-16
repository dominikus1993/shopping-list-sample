namespace ShoppingList.Core.Exceptions;

public sealed class ShoppingListNotExistsException(ShoppingListId shoppingListId) : Exception("ShoppingList not exists")
{
    public ShoppingListId ShoppingListId { get; } = shoppingListId;

    public override string ToString()
    {
        return $"{base.ToString()}, {nameof(ShoppingListId)}: {ShoppingListId}";
    }
}