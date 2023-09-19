namespace ShoppingList.Core.Exceptions;

public sealed class ShoppingListIsAlereadyUnactiveException(ShoppingListId shoppingListId) : Exception("ShoppingList is already unactive")
{
    public ShoppingListId ShoppingListId { get; } = shoppingListId;

    public override string ToString()
    {
        return $"{base.ToString()}, {nameof(ShoppingListId)}: {ShoppingListId}";
    }
}

public sealed class ShoppingListIsUnActiveException(ShoppingListId shoppingListId) : Exception("ShoppingList is unactive")
{
    public ShoppingListId ShoppingListId { get; } = shoppingListId;

    public override string ToString()
    {
        return $"{base.ToString()}, {nameof(ShoppingListId)}: {ShoppingListId}";
    }
}