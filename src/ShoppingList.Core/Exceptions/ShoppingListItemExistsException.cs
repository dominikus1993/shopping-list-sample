using ShoppingList.Core.Model;

namespace ShoppingList.Core.Exceptions;

public sealed class ShoppingListItemExistsException : Exception
{
    public ShoppingListItemId ShoppingListItemId { get; set; }


    public ShoppingListItemExistsException(ShoppingListItemId shoppingListItemId) : base("ShoppingList item aleready exists")
    {
        ShoppingListItemId = shoppingListItemId;
    }

    public override string ToString()
    {
        return $"{base.ToString()}, {nameof(ShoppingListItemId)}: {ShoppingListItemId}";
    }
}