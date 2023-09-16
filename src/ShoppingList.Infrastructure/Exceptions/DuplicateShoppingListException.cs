namespace ShoppingList.Infrastructure.Exceptions;

public sealed class DuplicateShoppingListException(Guid shoppingListId, Exception? innerException) : Exception("cannot insert duplicate shopping list", innerException)
{
    public Guid ShoppingListId { get; } = shoppingListId;

    public override string ToString()
    {
        return $"{base.ToString()}, {nameof(ShoppingListId)}: {ShoppingListId}";
    }
}