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
        var subject = new Core.Model.CustomerShoppingList(id, userId, name);
        
        Assert.Equal(id, subject.Id);
        Assert.Equal(userId, subject.UserId);
        Assert.Equal(name, subject.ShoppingListName);
        Assert.Empty(subject.Items);
    }
    
    [Theory]
    [AutoData]
    public void TestAddItemWhenShoppingListIsEmpty(ShoppingListId id, UserId userId, ShoppingListName name, ShoppingListItem item)
    {
        var subject = new Core.Model.CustomerShoppingList(id, userId, name);
        
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
        var subject = new Core.Model.CustomerShoppingList(id, userId, name);
        
        subject.AddItem(item);

        var newItem = item with { Name = newName };

        var ex = Assert.Throws<ShoppingListItemExistsException>(() => subject.AddItem(newItem));
        
        Assert.Equal(item.Id, ex.ShoppingListItemId);
    }
    
    [Theory]
    [AutoData]
    public void TestRemoveItemWhenShoppingListContainsItem(ShoppingListId id, UserId userId, ShoppingListName name, ShoppingListItem item)
    {
        var subject = new Core.Model.CustomerShoppingList(id, userId, name);
        
        subject.AddItem(item);
        subject.RemoveItem(item.Id);

        Assert.Equal(id, subject.Id);
        Assert.Equal(userId, subject.UserId);
        Assert.Equal(name, subject.ShoppingListName);
        Assert.Empty(subject.Items);
    }
    
    [Theory]
    [AutoData]
    public void TestRemoveItemWhenShoppingListContainsItemAndHAsMoreThanOneItem(ShoppingListId id, UserId userId, ShoppingListName name, ShoppingListItem item,  ShoppingListItem item2)
    {
        var subject = new Core.Model.CustomerShoppingList(id, userId, name);
        
        subject.AddItem(item);
        subject.AddItem(item2);
        subject.RemoveItem(item.Id);

        Assert.Equal(id, subject.Id);
        Assert.Equal(userId, subject.UserId);
        Assert.Equal(name, subject.ShoppingListName);
        Assert.NotEmpty(subject.Items);
        subject.Items.ShouldContain(item2);
        subject.Items.ShouldNotContain(item);
    }
    
    [Theory]
    [AutoData]
    public void TestRemoveItemWhenShoppingListNotContainsItem(ShoppingListId id, UserId userId, ShoppingListName name, ShoppingListItem item, ShoppingListItemId itemId2)
    {
        var subject = new Core.Model.CustomerShoppingList(id, userId, name);
        
        subject.AddItem(item);
        

        var ex = Assert.Throws<ShoppingListItemNotExistsException>(() => subject.RemoveItem(itemId2));
        
        Assert.Equal(itemId2, ex.ShoppingListItemId);
    }
    
    
    [Theory]
    [AutoData]
    public void TestReconstructShoppingListByEvents(ShoppingListId id, UserId userId, ShoppingListName name, ShoppingListItem item)
    {
        var sl = new Core.Model.CustomerShoppingList(id, userId, name);
        
        sl.AddItem(item);

        var events = sl.GetUncommittedChanges();
        var subject = new Core.Model.CustomerShoppingList();
        subject.LoadsFromHistory(events);
        
        Assert.Equivalent(sl, subject);
    }
}