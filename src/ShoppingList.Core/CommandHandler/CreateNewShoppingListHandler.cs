using Mediator;

using ShoppingList.Core.Commands;
using ShoppingList.Core.Repositories;

namespace ShoppingList.Core.CommandHandler;

public sealed class CreateNewShoppingListHandler(IShoppingListsRepository shoppingListsRepository) : IRequestHandler<CreateNewShoppingList, Guid>
{
    public async ValueTask<Guid> Handle(CreateNewShoppingList request, CancellationToken cancellationToken)
    {
        var shoppingList = new Model.CustomerShoppingList(request.Id, request.UserId, request.ShoppingListName);

        var saveResult = await shoppingListsRepository.Save(shoppingList, cancellationToken);
        if (saveResult.IsSuccess)
        {
            return saveResult.Success;
        }

        throw new InvalidOperationException("can't save new shopping list", saveResult.Error);
    }
}