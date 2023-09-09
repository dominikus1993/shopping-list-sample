using ShoppingList.Core.Model;

namespace ShoppingList.Core.Repositories;

public interface IShoppingListsRepository
{
    Task<CustomerShoppingList?> Load(Guid id, int version, CancellationToken cancellationToken = default);
    Task Update(CustomerShoppingList customerShoppingList, CancellationToken cancellationToken = default);
    Task Save(CustomerShoppingList customerShoppingList, CancellationToken cancellationToken = default);
}