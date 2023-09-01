using Mediator;

using ShoppingList.Core.Model;

namespace ShoppingList.Core.Commands;

public sealed record CreateNewShoppingList(ShoppingListId Id, UserId UserId, ShoppingListName ShoppingListName) : IRequest;