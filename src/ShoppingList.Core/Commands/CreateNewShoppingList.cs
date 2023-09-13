using Mediator;

using ShoppingList.Core.Model;

namespace ShoppingList.Core.Commands;

public sealed record CreateNewShoppingList(Guid Id, UserId UserId, ShoppingListName ShoppingListName) : IRequest<Guid>;
