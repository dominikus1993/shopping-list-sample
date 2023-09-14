using AutoFixture.Xunit2;

using ShoppingList.Core.CommandHandler;
using ShoppingList.Core.Commands;
using ShoppingList.Core.Tests.Fixtures;
using ShoppingList.Infrastructure.Exceptions;

namespace ShoppingList.Core.Tests.CommandHandler;

public class CreateNewShoppingListHandlerTests : IClassFixture<MartenFixture>
{
    private CreateNewShoppingListHandler _createNewShoppingListHandler;

    public CreateNewShoppingListHandlerTests(MartenFixture martenFixture)
    {
        _createNewShoppingListHandler = new CreateNewShoppingListHandler(martenFixture.ShoppingListsRepository);
    }

    [Theory]
    [AutoData]
    public async Task TestWhenTryAddNewRecord(CreateNewShoppingList cmd)
    {
        var id = await _createNewShoppingListHandler.Handle(cmd, default);
        Assert.Equal(cmd.Id, id);
    }
    
    [Theory]
    [AutoData]
    public async Task TestWhenTryAddNewRecordAndAddItAgain(CreateNewShoppingList cmd)
    {
        var id = await _createNewShoppingListHandler.Handle(cmd, default);
        Assert.Equal(cmd.Id, id);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _createNewShoppingListHandler.Handle(cmd, default));
        
        Assert.NotNull(ex);
        Assert.NotNull(ex.InnerException);
        Assert.IsType<DuplicateShoppingListException>(ex.InnerException);
    }
}