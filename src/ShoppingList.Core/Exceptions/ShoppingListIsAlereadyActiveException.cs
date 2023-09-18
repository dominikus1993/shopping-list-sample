namespace ShoppingList.Core.Exceptions;

public sealed class ShoppingListIsAlereadyActiveException(ShoppingListId shoppingListId) : Exception("ShoppingList is already active")
{
    public ShoppingListId ShoppingListId { get; } = shoppingListId;

    public override string ToString()
    {
        return $"{base.ToString()}, {nameof(ShoppingListId)}: {ShoppingListId}";
    }
}