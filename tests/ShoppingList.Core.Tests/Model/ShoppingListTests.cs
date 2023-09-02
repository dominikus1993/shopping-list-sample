using AutoFixture.Xunit2;

using ShoppingList.Core.Exceptions;
using ShoppingList.Core.Model;

using Shouldly;

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
        Assert.Empty(subject.Items);
    }
    
    [Theory]
    [AutoData]
    public void TestAddItemWhenShoppingListIsEmpty(ShoppingListId id, UserId userId, ShoppingListName name, ShoppingListItem item)
    {
        var subject = new Core.Model.ShoppingList(id, userId, name);
        
        subject.AddItem(item);
        
        Assert.Equal(id, subject.Id);
        Assert.Equal(userId, subject.UserId);
        Assert.Equal(name, subject.ShoppingListName);
        Assert.NotEmpty(subject.Items);
        Assert.Single(subject.Items);
        subject.Items.ShouldContain(item);
    }
    
    [Theory]
    [AutoData]
    public void TestAddItemWhenShoppingListContainsItem(ShoppingListId id, UserId userId, ShoppingListName name, ShoppingListItem item, ShoppingListItemName newName)
    {
        var subject = new Core.Model.ShoppingList(id, userId, name);
        
        subject.AddItem(item);

        var newItem = item with { Name = newName };

        var ex = Assert.Throws<ShoppingListItemExistsException>(() => subject.AddItem(newItem));
        
        Assert.Equal(item.Id, ex.ShoppingListItemId);
    }
}