using Mediator;

using ShoppingList.Core.Commands;
using ShoppingList.Core.Exceptions;
using ShoppingList.Core.Repositories;

using Unit = Mediator.Unit;

namespace ShoppingList.Core.CommandHandler;

public sealed class AddItemToShoppingListHandler(IShoppingListsRepository shoppingListsRepository) : IRequestHandler<AddItemToShoppingList>
{
    public async ValueTask<Unit> Handle(AddItemToShoppingList request, CancellationToken cancellationToken)
    {
        var shoppingList = await shoppingListsRepository.Load(request.Id, cancellationToken);
        if (shoppingList is null)
        {
            throw new ShoppingListNotExistsException(request.Id);
        }
        
        shoppingList.AddItem(request.Item);

        await shoppingListsRepository.Update(shoppingList, cancellationToken);

        return Unit.Value;
    }
}