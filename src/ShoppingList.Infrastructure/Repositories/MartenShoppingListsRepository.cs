using Marten;
using Marten.Exceptions;

using ShoppingList.Core.Model;
using ShoppingList.Core.Repositories;
using ShoppingList.Infrastructure.Exceptions;
using ShoppingList.Infrastructure.Extensions;

namespace ShoppingList.Infrastructure.Repositories;

public sealed class MartenShoppingListsRepository(IDocumentStore store) : IShoppingListsRepository
{
    public async Task<CustomerShoppingList?> Load(Guid id, CancellationToken cancellationToken = default)
    {
        await using var session = store.LightweightSession();
        return await session.Get<CustomerShoppingList>(id, cancellationToken);
    }

    public async Task<UpdateResult> Update(CustomerShoppingList customerShoppingList, CancellationToken cancellationToken = default)
    {
        await using var session = store.LightweightSession();
        var id = customerShoppingList.Id;
        await session.Update(id, customerShoppingList.GetUncommittedChanges(), cancellationToken);
        return UpdateResult.Ok;
    }

    public async Task<SaveResult> Save(CustomerShoppingList customerShoppingList, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var session = store.LightweightSession();
            var id = customerShoppingList.Id;
            await session.Add<CustomerShoppingList>(id, customerShoppingList.GetUncommittedChanges(), cancellationToken);
            return id;
        }
        catch (ExistingStreamIdCollisionException e)
        {
            return new DuplicateShoppingListException(customerShoppingList.Id, e);
        }
    }
}