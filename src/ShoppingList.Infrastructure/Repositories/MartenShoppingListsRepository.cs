using Marten;

using ShoppingList.Core.Model;
using ShoppingList.Core.Repositories;
using ShoppingList.Infrastructure.Extensions;

namespace ShoppingList.Infrastructure.Repositories;

public sealed class MartenShoppingListsRepository : IShoppingListsRepository
{
    private readonly IDocumentSession _documentSession;

    public MartenShoppingListsRepository(IDocumentSession documentSession)
    {
        _documentSession = documentSession;
    }

    public Task<Core.Model.ShoppingList> Load(ShoppingListId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task Save(Core.Model.ShoppingList shoppingList, CancellationToken cancellationToken = default)
    {
        var id = shoppingList.Id.Value;
        await _documentSession.Add<Core.Model.ShoppingList>(id, shoppingList.GetUncommittedChanges(), cancellationToken);
    }
}