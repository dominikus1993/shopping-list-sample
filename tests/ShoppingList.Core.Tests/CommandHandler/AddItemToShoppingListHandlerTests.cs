using AutoFixture.Xunit2;

using ShoppingList.Core.CommandHandler;
using ShoppingList.Core.Commands;
using ShoppingList.Core.Exceptions;
using ShoppingList.Core.Tests.Fixtures;

namespace ShoppingList.Core.Tests.CommandHandler;

public class AddItemToShoppingListHandlerTests : IClassFixture<MartenFixture>
{
    private readonly AddItemToShoppingListHandler _addItemToShoppingListHandler;
    private readonly CreateNewShoppingListHandler _createNewShoppingListHandler;

    public AddItemToShoppingListHandlerTests(MartenFixture martenFixture)
    {
        _addItemToShoppingListHandler = new AddItemToShoppingListHandler(martenFixture.ShoppingListsRepository);
        _createNewShoppingListHandler = new CreateNewShoppingListHandler(martenFixture.ShoppingListsRepository);
    }
    
    [Theory]
    [AutoData]
    public async Task TestWhenShoppingListNotExists(AddItemToShoppingList addItemToShoppingListCmd)
    {
        var ex  = await Assert.ThrowsAsync<ShoppingListNotExistsException>(async () => await _addItemToShoppingListHandler.Handle(addItemToShoppingListCmd, default));
        Assert.NotNull(ex);
    }
    
    [Theory]
    [AutoData]
    public async Task TestWhenShoppingListExistsAndItemIsNew(CreateNewShoppingList createNewShoppingListCmd, AddItemToShoppingList addItemToShoppingListCmd)
    {
        addItemToShoppingListCmd = addItemToShoppingListCmd with { Id = createNewShoppingListCmd.Id };
        var ex  = await Record.ExceptionAsync(async () => await _createNewShoppingListHandler.Handle(createNewShoppingListCmd, default));
        Assert.Null(ex);
        
        ex  = await Record.ExceptionAsync(async () => await _addItemToShoppingListHandler.Handle(addItemToShoppingListCmd, default));
        Assert.Null(ex);
    }
    
    [Theory]
    [AutoData]
    public async Task TestWhenShoppingListExistsAndItemIsDuplicate(CreateNewShoppingList createNewShoppingListCmd, AddItemToShoppingList addItemToShoppingListCmd)
    {
        addItemToShoppingListCmd = addItemToShoppingListCmd with { Id = createNewShoppingListCmd.Id };
        var ex  = await Record.ExceptionAsync(async () => await _createNewShoppingListHandler.Handle(createNewShoppingListCmd, default));
        Assert.Null(ex);
        
        ex  = await Record.ExceptionAsync(async () => await _addItemToShoppingListHandler.Handle(addItemToShoppingListCmd, default));
        Assert.Null(ex);
        
        var shoppingListItemExistsException  = await Assert.ThrowsAsync<ShoppingListItemExistsException>(async () => await _addItemToShoppingListHandler.Handle(addItemToShoppingListCmd, default));

        Assert.NotNull(shoppingListItemExistsException);
        Assert.Equal(createNewShoppingListCmd.Id, shoppingListItemExistsException.ShoppingListId);
        Assert.Equal(addItemToShoppingListCmd.Item.Id, shoppingListItemExistsException.ShoppingListItemId);
    }
}