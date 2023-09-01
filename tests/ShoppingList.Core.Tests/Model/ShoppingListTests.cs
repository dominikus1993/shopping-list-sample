using AutoFixture.Xunit2;

using ShoppingList.Core.Model;

namespace ShoppingList.Core.Tests.Model;

public class ShoppingListTests
{
    [Theory]
    [AutoData]
    public void TestCreateNewShoppingList(ShoppingListId id, UserId userId, ShoppingListName name)
    {
        var subject = new Core.Model.ShoppingList(id, userId, name);
        
        Assert.Equal(id, subject.Id);
        Assert.Equal(userId, subject.UserId);
        Assert.Equal(name, subject.ShoppingListName);
    }
}