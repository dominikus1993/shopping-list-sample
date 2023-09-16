using Mediator;

using ShoppingList.Core.Commands;
using ShoppingList.Core.Exceptions;
using ShoppingList.Core.Repositories;

namespace ShoppingList.Core.CommandHandler;

public class RemoveItemFromShoppingListHandler(IShoppingListsRepository shoppingListsRepository) : IRequestHandler<RemoveItemFromShoppingList>
{
    public async ValueTask<Unit> Handle(RemoveItemFromShoppingList request, CancellationToken cancellationToken)
    {
        var shoppingList = await shoppingListsRepository.Load(request.Id, cancellationToken);
        if (shoppingList is null)
        {
            throw new ShoppingListNotExistsException(request.Id);
        }
        
        shoppingList.RemoveItem(request.ItemId);

        await shoppingListsRepository.Update(shoppingList, cancellationToken);

        return Unit.Value;
    }
}