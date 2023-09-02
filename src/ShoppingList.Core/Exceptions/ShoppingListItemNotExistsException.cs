using ShoppingList.Core.Model;

namespace ShoppingList.Core.Exceptions;

public sealed class ShoppingListItemNotExistsException : Exception
{
    public ShoppingListItemId ShoppingListItemId { get; set; }


    public ShoppingListItemNotExistsException(ShoppingListItemId shoppingListItemId) : base("ShoppingList item not exists")
    {
        ShoppingListItemId = shoppingListItemId;
    }

    public override string ToString()
    {
        return $"{base.ToString()}, {nameof(ShoppingListItemId)}: {ShoppingListItemId}";
    }
}