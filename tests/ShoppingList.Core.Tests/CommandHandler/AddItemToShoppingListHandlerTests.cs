using AutoFixture.Xunit2;

using ShoppingList.Core.CommandHandler;
using ShoppingList.Core.Commands;
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
    public async Task TestWhenShoppingListNotExists(CreateNewShoppingList createNewShoppingListCmd, AddItemToShoppingList addItemToShoppingListCmd)
    {
        var ex  = await Assert.ThrowsAsync<SLisNot>(async () => await _createNewShoppingListHandler.Handle(createNewShoppingListCmd, default));
        Assert.Null(ex);
    }
    
    [Theory]
    [AutoData]
    public async Task TestWhenShoppingListExistsAndItemIsNew(CreateNewShoppingList createNewShoppingListCmd, AddItemToShoppingList addItemToShoppingListCmd)
    {
        var ex  = await Record.ExceptionAsync(async () => await _createNewShoppingListHandler.Handle(createNewShoppingListCmd, default));
        Assert.Null(ex);
        
        ex  = await Record.ExceptionAsync(async () => await _addItemToShoppingListHandler.Handle(addItemToShoppingListCmd, default));
        Assert.Null(ex);
    }
    
}