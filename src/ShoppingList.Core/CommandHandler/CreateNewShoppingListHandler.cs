using Mediator;

using ShoppingList.Core.Commands;
using ShoppingList.Core.Repositories;

namespace ShoppingList.Core.CommandHandler;

public sealed class CreateNewShoppingListHandler : IRequestHandler<CreateNewShoppingList>
{
    private readonly IShoppingListsRepository _shoppingListsRepository;

    public CreateNewShoppingListHandler(IShoppingListsRepository shoppingListsRepository)
    {
        _shoppingListsRepository = shoppingListsRepository;
    }

    public async ValueTask<Unit> Handle(CreateNewShoppingList request, CancellationToken cancellationToken)
    {
        var shoppingList = new Model.CustomerShoppingList(request.Id, request.UserId, request.ShoppingListName);

        await _shoppingListsRepository.Save(shoppingList, cancellationToken);
        
        return Unit.Value;
    }
}