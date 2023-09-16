using OneOf;

using ShoppingList.Core.Model;

namespace ShoppingList.Core.Repositories;

public readonly record struct Unit
{
    public static readonly Unit Value = new();
}

[GenerateOneOf]
public partial class SaveResult : OneOfBase<Guid, Exception>
{
    public bool IsSuccess => base.IsT0;

    public Guid Success => IsSuccess ? AsT0 : throw new InvalidOperationException("Value is not success");
    public Exception Error => IsSuccess ? throw new InvalidOperationException("Value is not error") : AsT1; 
}

[GenerateOneOf]
public partial class UpdateResult : OneOfBase<Unit, Exception>
{
    public bool IsSuccess => base.IsT0;

    public Unit Success => IsSuccess ? AsT0 : throw new InvalidOperationException("Value is not success");
    public Exception Error => IsSuccess ? throw new InvalidOperationException("Value is not error") : AsT1;

    public static readonly UpdateResult Ok = new(Unit.Value);
}

public interface IShoppingListsRepository
{
    Task<CustomerShoppingList?> Load(Guid id, CancellationToken cancellationToken = default);
    Task<UpdateResult> Update(CustomerShoppingList customerShoppingList, CancellationToken cancellationToken = default);
    Task<SaveResult> Save(CustomerShoppingList customerShoppingList, CancellationToken cancellationToken = default);
}