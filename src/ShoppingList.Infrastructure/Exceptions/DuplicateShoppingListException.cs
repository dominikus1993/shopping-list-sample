namespace ShoppingList.Infrastructure.Exceptions;

public sealed class DuplicateShoppingListException : Exception
{
    public DuplicateShoppingListException(Guid shoppingListId, Exception? innerException) : base("cannot insert duplicate shopping list", innerException)
    {
        ShoppingListId = shoppingListId;
    }

    public Guid ShoppingListId { get; }

    public override string ToString()
    {
        return $"{base.ToString()}, {nameof(ShoppingListId)}: {ShoppingListId}";
    }
}