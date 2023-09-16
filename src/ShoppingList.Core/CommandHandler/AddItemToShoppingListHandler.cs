using Mediator;

using ShoppingList.Core.Commands;
using ShoppingList.Core.Repositories;

namespace ShoppingList.Core.CommandHandler;

public sealed class AddItemToShoppingListHandler : IRequestHandler<AddItemToShoppingList>
{
    private readonly IShoppingListsRepository _shoppingListsRepository;

    public AddItemToShoppingListHandler(IShoppingListsRepository shoppingListsRepository)
    {
        _shoppingListsRepository = shoppingListsRepository;
    }

    public ValueTask<Unit> Handle(AddItemToShoppingList request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}