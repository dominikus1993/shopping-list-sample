using AutoFixture.Xunit2;

using Marten.Exceptions;

using ShoppingList.Core.Model;
using ShoppingList.Core.Repositories;
using ShoppingList.Infrastructure.Repositories;
using ShoppingList.Infrastructure.Tests.Fixtures;

namespace ShoppingList.Infrastructure.Tests.Repositories;

public class MartenShoppingListsRepositoryTests : IClassFixture<MartenFixture>
{
    private readonly MartenFixture _martenFixture;
    private readonly IShoppingListsRepository _shoppingListsRepository;
    
    public MartenShoppingListsRepositoryTests(MartenFixture martenFixture)
    {
        _martenFixture = martenFixture;
        _shoppingListsRepository = new MartenShoppingListsRepository(_martenFixture.Session);
    }

    [Theory]
    [AutoData]
    public async Task SaveNewShoppingList(Guid shoppingListId, UserId userId, ShoppingListName shoppingListName)
    {
        var sl = new CustomerShoppingList(shoppingListId, userId, shoppingListName);

        await _shoppingListsRepository.Save(sl);

        Assert.Equal(1, sl.Version);
        var subject = await _shoppingListsRepository.Load(sl.Id, sl.Version);

        Assert.NotNull(subject);
    }
    
    [Theory]
    [AutoData]
    public async Task SaveShoppingListWithItem(Guid shoppingListId, UserId userId, ShoppingListName shoppingListName, ShoppingListItem item)
    {
        var sl = new CustomerShoppingList(shoppingListId, userId, shoppingListName);
        sl.AddItem(item);
        await _shoppingListsRepository.Save(sl);
        
        Assert.Equal(2, sl.Version);
        var subject = await _shoppingListsRepository.Load(sl.Id, sl.Version);

        Assert.NotNull(subject);
        Assert.Single(subject.Items);
    }
    
    [Theory]
    [AutoData]
    public async Task SaveShoppingListTwice(Guid shoppingListId, UserId userId, ShoppingListName shoppingListName, ShoppingListItem item, ShoppingListItem item2)
    {
        var sl = new CustomerShoppingList(shoppingListId, userId, shoppingListName);
        sl.AddItem(item);
        await _shoppingListsRepository.Save(sl);
        sl.MarkChangesAsCommitted();

        sl.AddItem(item2);
        await Assert.ThrowsAsync<ExistingStreamIdCollisionException>(async () => await _shoppingListsRepository.Save(sl));
        
        // var subject = await _shoppingListsRepository.Load(sl.Id, 3);
        //
        // Assert.NotNull(subject);
        // Assert.NotEmpty(subject.Items);
        // Assert.Equal(2, subject.Items.Count);
    }
}