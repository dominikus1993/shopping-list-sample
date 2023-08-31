using ShoppingList.Core.Model;

namespace ShoppingList.Core.Repositories;

public interface IShoppingListsRepository
{
    Task<Model.ShoppingList> Load(ShoppingListId id, CancellationToken cancellationToken = default);
    Task Save(Model.ShoppingList shoppingList);
}