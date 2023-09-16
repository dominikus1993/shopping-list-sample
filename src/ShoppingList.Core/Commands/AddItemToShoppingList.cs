using Mediator;

using ShoppingList.Core.Model;

namespace ShoppingList.Core.Commands;

public sealed record AddItemToShoppingList(ShoppingListId Id, ShoppingListItem Item) : IRequest<Unit>;