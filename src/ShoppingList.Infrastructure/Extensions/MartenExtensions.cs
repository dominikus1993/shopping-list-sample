using Marten;

namespace ShoppingList.Infrastructure.Extensions;

public static class DocumentSessionExtensions
{
    public static Task Add<T>(this IDocumentSession documentSession, Guid id, IEnumerable<object> events, CancellationToken ct)
        where T : class
    {
        documentSession.Events.StartStream<T>(id, events);
        return documentSession.SaveChangesAsync(token: ct);
    }
    
    public static Task Update(this IDocumentSession documentSession, Guid id, IEnumerable<object> events, CancellationToken ct)
    {
        documentSession.Events.Append(id, events);
        return documentSession.SaveChangesAsync(token: ct);
    }
    
    public static Task GetAndUpdate<T>(
        this IDocumentSession documentSession,
        Guid id,
        int version,
        Func<T, object> handle,
        CancellationToken ct
    ) where T : class =>
        documentSession.Events.WriteToAggregate<T>(id, version, stream =>
            stream.AppendOne(handle(stream.Aggregate)), ct);

    public static Task<T?> Get<T>(
        this IDocumentSession documentSession,
        Guid id,
        int version,
        CancellationToken ct
    ) where T : class =>
        documentSession.Events.AggregateStreamAsync<T>(id, token: ct);


}