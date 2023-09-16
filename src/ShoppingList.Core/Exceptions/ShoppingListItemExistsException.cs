using ShoppingList.Core.Model;

namespace ShoppingList.Core.Exceptions;

public sealed class ShoppingListItemExistsException : Exception
{
    public ShoppingListItemId ShoppingListItemId { get;  }
    public ShoppingListId ShoppingListId { get;  }
    public ShoppingListItemExistsException(ShoppingListId shoppingListId, ShoppingListItemId shoppingListItemId) : base("ShoppingList item aleready exists")
    {
        ShoppingListItemId = shoppingListItemId;
        ShoppingListId = shoppingListId;
    }

    public override string ToString()
    {
        return $"{base.ToString()}, {nameof(ShoppingListItemId)}: {ShoppingListItemId}, {nameof(ShoppingListId)}: {ShoppingListId}";
    }
}