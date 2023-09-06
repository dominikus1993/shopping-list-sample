using ShoppingList.Core.Model;

namespace ShoppingList.Core.Repositories;

public interface IShoppingListsRepository
{
    Task<CustomerShoppingList?> Load(ShoppingListId id, CancellationToken cancellationToken = default);
    Task Save(CustomerShoppingList customerShoppingList, CancellationToken cancellationToken = default);
}