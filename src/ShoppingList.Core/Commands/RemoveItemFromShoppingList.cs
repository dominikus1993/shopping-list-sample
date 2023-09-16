using Mediator;

using ShoppingList.Core.Model;

namespace ShoppingList.Core.Commands;

public sealed record RemoveItemFromShoppingList(ShoppingListId Id, ShoppingListItemId ItemId) : IRequest<Unit>;