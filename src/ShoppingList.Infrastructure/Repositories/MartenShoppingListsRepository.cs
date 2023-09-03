using Marten;
using Marten;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using Marten.Pagination;
using Marten.Schema.Identity;
using Marten.Services.Json;
using ShoppingList.Core.Model;
using ShoppingList.Core.Repositories;

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
        _documentSession.Get
    }

    public Task Save(Core.Model.ShoppingList shoppingList)
    {
        throw new NotImplementedException();
    }
}