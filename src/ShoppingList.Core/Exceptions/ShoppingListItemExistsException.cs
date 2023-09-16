using ShoppingList.Core.Model;

namespace ShoppingList.Core.Exceptions;

public sealed class ShoppingListItemExistsException(ShoppingListId shoppingListId,
        ShoppingListItemId shoppingListItemId)
    : Exception("ShoppingList item aleready exists")
{
    public ShoppingListItemId ShoppingListItemId { get;  } = shoppingListItemId;
    public ShoppingListId ShoppingListId { get;  } = shoppingListId;

    public override string ToString()
    {
        return $"{base.ToString()}, {nameof(ShoppingListItemId)}: {ShoppingListItemId}, {nameof(ShoppingListId)}: {ShoppingListId}";
    }
}