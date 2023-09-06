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

    public Task<CustomerShoppingList?> Load(ShoppingListId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task Save(CustomerShoppingList customerShoppingList, CancellationToken cancellationToken = default)
    {
        var id = customerShoppingList.Id.Value;
        await _documentSession.Add<CustomerShoppingList>(id, customerShoppingList.GetUncommittedChanges(), cancellationToken);
    }
}