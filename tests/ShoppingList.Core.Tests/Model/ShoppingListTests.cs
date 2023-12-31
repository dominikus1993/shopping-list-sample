using AutoFixture.Xunit2;

using ShoppingList.Core.Exceptions;
using ShoppingList.Core.Model;

using Shouldly;

namespace ShoppingList.Core.Tests.Model;

public class ShoppingListTests
{
    [Theory]
    [AutoData]
    public void TestCreateNewShoppingList(Guid id, UserId userId, ShoppingListName name)
    {
        var subject = new Core.Model.CustomerShoppingList(id, userId, name);
        
        Assert.Equal(id, subject.Id);
        Assert.Equal(userId, subject.UserId);
        Assert.Equal(name, subject.ShoppingListName);
        Assert.Empty(subject.Items);
    }
    
    [Theory]
    [AutoData]
    public void TestAddItemWhenShoppingListIsEmpty(Guid id, UserId userId, ShoppingListName name, ShoppingListItem item)
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
    public void TestAddItemWhenShoppingListContainsItem(Guid id, UserId userId, ShoppingListName name, ShoppingListItem item, ShoppingListItemName newName)
    {
        var subject = new Core.Model.CustomerShoppingList(id, userId, name);
        
        subject.AddItem(item);

        var newItem = item with { Name = newName };

        var ex = Assert.Throws<ShoppingListItemExistsException>(() => subject.AddItem(newItem));
        
        Assert.Equal(item.Id, ex.ShoppingListItemId);
    }
    
    [Theory]
    [AutoData]
    public void TestAddItemWhenCmdIsNull(Guid id, UserId userId, ShoppingListName name)
    {
        var subject = new Core.Model.CustomerShoppingList(id, userId, name);
        
        Assert.Throws<ArgumentNullException>(() => subject.AddItem(null!));
    }
    
    [Theory]
    [AutoData]
    public void TestAddItemWhenWhenShoppingListIsUnActive(Guid id, UserId userId, ShoppingListName name, ShoppingListItem item)
    {
        var subject = new Core.Model.CustomerShoppingList(id, userId, name);
        subject.MarkAsUnActive();
        Assert.Throws<ShoppingListIsUnActiveException>(() => subject.AddItem(item));
    }
    
    [Theory]
    [AutoData]
    public void TestRemoveItemWhenWhenShoppingListIsUnActive(Guid id, UserId userId, ShoppingListName name, ShoppingListItem item)
    {
        var subject = new Core.Model.CustomerShoppingList(id, userId, name);
        subject.AddItem(item);
        subject.MarkAsUnActive();
        Assert.Throws<ShoppingListIsUnActiveException>(() => subject.RemoveItem(item.Id));
    }
    
    [Theory]
    [AutoData]
    public void TestRemoveItemWhenShoppingListContainsItem(Guid id, UserId userId, ShoppingListName name, ShoppingListItem item)
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
    public void TestRemoveItemWhenShoppingListContainsItemAndHAsMoreThanOneItem(Guid id, UserId userId, ShoppingListName name, ShoppingListItem item,  ShoppingListItem item2)
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
    public void TestRemoveItemWhenShoppingListNotContainsItem(Guid id, UserId userId, ShoppingListName name, ShoppingListItem item, ShoppingListItemId itemId2)
    {
        var subject = new Core.Model.CustomerShoppingList(id, userId, name);
        
        subject.AddItem(item);
        

        var ex = Assert.Throws<ShoppingListItemNotExistsException>(() => subject.RemoveItem(itemId2));
        
        Assert.Equal(itemId2, ex.ShoppingListItemId);
    }
    
    
    [Theory]
    [AutoData]
    public void TestReconstructShoppingListByEvents(Guid id, UserId userId, ShoppingListName name, ShoppingListItem item)
    {
        var sl = new Core.Model.CustomerShoppingList(id, userId, name);
        
        sl.AddItem(item);

        var events = sl.GetUncommittedChanges();
        var subject = new Core.Model.CustomerShoppingList();
        subject.LoadsFromHistory(events);
        
        Assert.Equivalent(sl, subject);
    }
    
    [Theory]
    [AutoData]
    public void TestMarkShoppingListAsNoActiveWhenIsActive(Guid id, UserId userId, ShoppingListName name)
    {
        var sl = new CustomerShoppingList(id, userId, name);
        sl.MarkAsUnActive();
        
        Assert.Equal(CustomerShoppingListState.NoActive, sl.State);
    }
    
    [Theory]
    [AutoData]
    public void TestMarkShoppingListAsNoActiveWhenIsNoActive(Guid id, UserId userId, ShoppingListName name)
    {
        var sl = new Core.Model.CustomerShoppingList(id, userId, name);
        sl.MarkAsUnActive();
        
        Assert.Equal(CustomerShoppingListState.NoActive, sl.State);

        var ex = Assert.Throws<ShoppingListIsAlereadyUnactiveException>(() => sl.MarkAsUnActive());
        Assert.Equal(sl.Id, ex.ShoppingListId);
    }
    
    [Theory]
    [AutoData]
    public void TestMarkShoppingListAsActive(Guid id, UserId userId, ShoppingListName name)
    {
        var sl = new CustomerShoppingList(id, userId, name);
        sl.MarkAsActive();
        
        Assert.Equal(CustomerShoppingListState.Active, sl.State);
    }
    
    [Theory]
    [AutoData]
    public void TestMarkShoppingListAsActiveWhenIsActive(Guid id, UserId userId, ShoppingListName name)
    {
        var sl = new Core.Model.CustomerShoppingList(id, userId, name);
        Assert.Equal(CustomerShoppingListState.Active, sl.State);

        var ex = Assert.Throws<ShoppingListIsAlereadyActiveException>(() => sl.MarkAsActive());
        Assert.Equal(sl.Id, ex.ShoppingListId);
    }
    
    [Theory]
    [AutoData]
    public void TestMarkShoppingListAsNoActiveWhenIsActiveAndMarkAsActive(Guid id, UserId userId, ShoppingListName name)
    {
        var sl = new CustomerShoppingList(id, userId, name);
        sl.MarkAsUnActive();
        
        Assert.Equal(CustomerShoppingListState.NoActive, sl.State);
        
        sl.MarkAsActive();
        
        Assert.Equal(CustomerShoppingListState.Active, sl.State);
    }
}