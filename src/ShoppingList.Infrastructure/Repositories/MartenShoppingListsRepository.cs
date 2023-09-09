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

    public async Task<CustomerShoppingList?> Load(Guid id, int version, CancellationToken cancellationToken = default)
    {
        return await _documentSession.Get<CustomerShoppingList>(id, version, cancellationToken);
    }

    public async Task Update(CustomerShoppingList customerShoppingList, CancellationToken cancellationToken = default)
    {
        var id = customerShoppingList.Id;
        await _documentSession.Update(id, customerShoppingList.GetUncommittedChanges(), cancellationToken);
    }

    public async Task Save(CustomerShoppingList customerShoppingList, CancellationToken cancellationToken = default)
    {
        var id = customerShoppingList.Id;
        await _documentSession.Add<CustomerShoppingList>(id, customerShoppingList.GetUncommittedChanges(), cancellationToken);
    }
}