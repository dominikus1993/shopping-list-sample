using AutoFixture.Xunit2;

using ShoppingList.Core.CommandHandler;
using ShoppingList.Core.Commands;
using ShoppingList.Core.Exceptions;
using ShoppingList.Core.Model;
using ShoppingList.Core.Tests.Fixtures;

namespace ShoppingList.Core.Tests.CommandHandler;

public class RemoveItemFromShoppingListHandlerTests: IClassFixture<MartenFixture>
{
    private readonly AddItemToShoppingListHandler _addItemToShoppingListHandler;
    private readonly CreateNewShoppingListHandler _createNewShoppingListHandler;
    private readonly RemoveItemFromShoppingListHandler _removeItemFromShoppingListHandler;

    public RemoveItemFromShoppingListHandlerTests(MartenFixture martenFixture)
    {
        _addItemToShoppingListHandler = new AddItemToShoppingListHandler(martenFixture.ShoppingListsRepository);
        _createNewShoppingListHandler = new CreateNewShoppingListHandler(martenFixture.ShoppingListsRepository);
        _removeItemFromShoppingListHandler =
            new RemoveItemFromShoppingListHandler(martenFixture.ShoppingListsRepository);
    }

    [Theory]
    [AutoData]
    public async Task TestWhenShoppingListNotExists(RemoveItemFromShoppingList removeItemFromShoppingList)
    {
        var ex  = await Assert.ThrowsAsync<ShoppingListNotExistsException>(async () => await _removeItemFromShoppingListHandler.Handle(removeItemFromShoppingList, default));
        Assert.NotNull(ex);
    }
    
    [Theory]
    [AutoData]
    public async Task TestWhenShoppingListExistsAndItemExistsInShoppingList(CreateNewShoppingList createNewShoppingListCmd, AddItemToShoppingList addItemToShoppingListCmd)
    {
        addItemToShoppingListCmd = addItemToShoppingListCmd with { Id = createNewShoppingListCmd.Id };
        var removeItemFromShoppingList =
            new RemoveItemFromShoppingList(createNewShoppingListCmd.Id, addItemToShoppingListCmd.Item.Id);
        
        await _createNewShoppingListHandler.Handle(createNewShoppingListCmd, default);

        await _addItemToShoppingListHandler.Handle(addItemToShoppingListCmd, default);
        
        var ex  = await Record.ExceptionAsync(async () => await _removeItemFromShoppingListHandler.Handle(removeItemFromShoppingList, default));
        Assert.Null(ex);
    }
    
    [Theory]
    [AutoData]
    public async Task TestWhenShoppingListExistsAndItemNotExistsInShoppingList(CreateNewShoppingList createNewShoppingListCmd, AddItemToShoppingList addItemToShoppingListCmd, ShoppingListItemId notExistsentListItemId)
    {
        addItemToShoppingListCmd = addItemToShoppingListCmd with { Id = createNewShoppingListCmd.Id };
        await _createNewShoppingListHandler.Handle(createNewShoppingListCmd, default);
        
        var removeItemFromShoppingList =
            new RemoveItemFromShoppingList(createNewShoppingListCmd.Id, notExistsentListItemId);
        
        await _addItemToShoppingListHandler.Handle(addItemToShoppingListCmd, default);
        
        var shoppingListItemNotExistsException  = await Assert.ThrowsAsync<ShoppingListItemNotExistsException>(async () => await _removeItemFromShoppingListHandler.Handle(removeItemFromShoppingList, default));

        Assert.NotNull(shoppingListItemNotExistsException);
        Assert.Equal(createNewShoppingListCmd.Id, shoppingListItemNotExistsException.ShoppingListId);
        Assert.Equal(notExistsentListItemId, shoppingListItemNotExistsException.ShoppingListItemId);
    }
}