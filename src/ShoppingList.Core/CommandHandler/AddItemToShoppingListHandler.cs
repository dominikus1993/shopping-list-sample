using Mediator;

using ShoppingList.Core.Commands;
using ShoppingList.Core.Exceptions;
using ShoppingList.Core.Repositories;

namespace ShoppingList.Core.CommandHandler;

public sealed class AddItemToShoppingListHandler : IRequestHandler<AddItemToShoppingList>
{
    private readonly IShoppingListsRepository _shoppingListsRepository;

    public AddItemToShoppingListHandler(IShoppingListsRepository shoppingListsRepository)
    {
        _shoppingListsRepository = shoppingListsRepository;
    }

    public async ValueTask<Unit> Handle(AddItemToShoppingList request, CancellationToken cancellationToken)
    {
        var shoppingList = await _shoppingListsRepository.Load(request.Id, cancellationToken);
        if (shoppingList is null)
        {
            throw new ShoppingListNotExistsException(request.Id);
        }
        
        shoppingList.AddItem(request.Item);

        await _shoppingListsRepository.Update(shoppingList, cancellationToken);

        return Unit.Value;
    }
}