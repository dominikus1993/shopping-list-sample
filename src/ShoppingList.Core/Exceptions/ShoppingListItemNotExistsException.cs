using ShoppingList.Core.Model;

namespace ShoppingList.Core.Exceptions;

public sealed class ShoppingListItemNotExistsException : Exception
{
    public ShoppingListItemId ShoppingListItemId { get; }
    public ShoppingListId ShoppingListId { get; }


    public ShoppingListItemNotExistsException(ShoppingListId shoppingListId, ShoppingListItemId shoppingListItemId) : base("ShoppingList item not exists")
    {
        ShoppingListId = shoppingListId;
        ShoppingListItemId = shoppingListItemId;
    }

    public override string ToString()
    {
        return $"{base.ToString()}, {nameof(ShoppingListItemId)}: {ShoppingListItemId}, {nameof(ShoppingListId)}: {ShoppingListId}";
    }
}