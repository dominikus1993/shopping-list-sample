using Marten;

using ShoppingList.Core.Model;
using ShoppingList.Core.Repositories;
using ShoppingList.Infrastructure.Extensions;

namespace ShoppingList.Infrastructure.Repositories;

public sealed class MartenShoppingListsRepository : IShoppingListsRepository
{
    private readonly IDocumentStore _store;

    public MartenShoppingListsRepository(IDocumentStore store)
    {
        _store = store;
    }

    public async Task<CustomerShoppingList?> Load(Guid id, int version, CancellationToken cancellationToken = default)
    {
        await using var session = _store.LightweightSession();
        return await session.Get<CustomerShoppingList>(id, version, cancellationToken);
    }

    public async Task Update(CustomerShoppingList customerShoppingList, CancellationToken cancellationToken = default)
    {
        await using var session = _store.LightweightSession();
        var id = customerShoppingList.Id;
        await session.Update(id, customerShoppingList.GetUncommittedChanges(), cancellationToken);
    }

    public async Task Save(CustomerShoppingList customerShoppingList, CancellationToken cancellationToken = default)
    {
        await using var session = _store.LightweightSession();
        var id = customerShoppingList.Id;
        await session.Add<CustomerShoppingList>(id, customerShoppingList.GetUncommittedChanges(), cancellationToken);
    }
}