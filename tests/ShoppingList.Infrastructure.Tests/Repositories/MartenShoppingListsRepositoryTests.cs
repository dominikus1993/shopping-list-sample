using AutoFixture.Xunit2;

using Marten.Exceptions;

using ShoppingList.Core.Model;
using ShoppingList.Core.Repositories;
using ShoppingList.Infrastructure.Exceptions;
using ShoppingList.Infrastructure.Tests.Fixtures;

namespace ShoppingList.Infrastructure.Tests.Repositories;

public sealed class MartenShoppingListsRepositoryTests : IClassFixture<MartenFixture>
{
    private readonly MartenFixture _martenFixture;
    private readonly IShoppingListsRepository _shoppingListsRepository;
    
    public MartenShoppingListsRepositoryTests(MartenFixture martenFixture)
    {
        _martenFixture = martenFixture;
        _shoppingListsRepository = _martenFixture.ShoppingListsRepository;
    }

    [Theory]
    [AutoData]
    public async Task SaveNewShoppingList(Guid shoppingListId, UserId userId, ShoppingListName shoppingListName)
    {
        var sl = new CustomerShoppingList(shoppingListId, userId, shoppingListName);

        var result = await _shoppingListsRepository.Save(sl);
        Assert.True(result.IsSuccess);

        Assert.Equal(1, sl.Version);
        var subject = await _shoppingListsRepository.Load(sl.Id);

        Assert.NotNull(subject);
    }
    
    [Theory]
    [AutoData]
    public async Task SaveShoppingListWithItem(Guid shoppingListId, UserId userId, ShoppingListName shoppingListName, ShoppingListItem item)
    {
        var sl = new CustomerShoppingList(shoppingListId, userId, shoppingListName);
        sl.AddItem(item);
        var result = await _shoppingListsRepository.Save(sl);
        Assert.True(result.IsSuccess);
        
        Assert.Equal(2, sl.Version);
        var subject = await _shoppingListsRepository.Load(sl.Id);

        Assert.NotNull(subject);
        Assert.Single(subject.Items);
    }
    
    [Theory]
    [AutoData]
    public async Task SaveShoppingListTwice(Guid shoppingListId, UserId userId, ShoppingListName shoppingListName, ShoppingListItem item, ShoppingListItem item2)
    {
        var sl = new CustomerShoppingList(shoppingListId, userId, shoppingListName);
        sl.AddItem(item);
        var result = await _shoppingListsRepository.Save(sl);
        Assert.True(result.IsSuccess);
        sl.MarkChangesAsCommitted();

        sl.AddItem(item2);
        var subject = await _shoppingListsRepository.Save(sl);
        
        Assert.False(subject.IsSuccess);
        Assert.IsType<DuplicateShoppingListException>(subject.Error);
        Assert.IsType<ExistingStreamIdCollisionException>(subject.Error.InnerException);
    }
    
    [Theory]
    [AutoData]
    public async Task SaveAndUpdateShoppingList(Guid shoppingListId, UserId userId, ShoppingListName shoppingListName, ShoppingListItem item, ShoppingListItem item2)
    {
        var sl = new CustomerShoppingList(shoppingListId, userId, shoppingListName);
        sl.AddItem(item);
        var result = await _shoppingListsRepository.Save(sl);
        Assert.True(result.IsSuccess);
        sl.MarkChangesAsCommitted();

        sl.AddItem(item2);
        await _shoppingListsRepository.Update(sl);

        var subject = await _shoppingListsRepository.Load(sl.Id);
        
        Assert.NotNull(subject);
        Assert.NotEmpty(subject.Items);
        Assert.Equal(2, subject.Items.Count);
    }
}