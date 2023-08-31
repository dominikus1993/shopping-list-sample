using Mediator;

using ShoppingList.Core.Commands;

namespace ShoppingList.Core.CommandHandler;

public sealed class CreateNewShoppingListHandler : IRequestHandler<CreateNewShoppingList>
{
    public ValueTask<Unit> Handle(CreateNewShoppingList request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}