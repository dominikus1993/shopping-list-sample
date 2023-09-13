using OneOf;

using ShoppingList.Core.Model;

namespace ShoppingList.Core.Repositories;

[GenerateOneOf]
public partial class SaveResult : OneOfBase<Guid, Exception>
{
    public bool IsSuccess => base.IsT0;

    public Guid Success => IsSuccess ? AsT0 : throw new InvalidOperationException("Value is not success");
    public Exception Error => IsSuccess ? throw new InvalidOperationException("Value is not error") : AsT1; 
    
}

public interface IShoppingListsRepository
{
    Task<CustomerShoppingList?> Load(Guid id, int version, CancellationToken cancellationToken = default);
    Task Update(CustomerShoppingList customerShoppingList, CancellationToken cancellationToken = default);
    Task<SaveResult> Save(CustomerShoppingList customerShoppingList, CancellationToken cancellationToken = default);
}