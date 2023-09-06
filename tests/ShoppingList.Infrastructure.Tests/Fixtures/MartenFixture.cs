using Marten;
using Marten.Events.Projections;
using Marten.Services.Json;

using ShoppingList.Core.Repositories;

using Testcontainers.PostgreSql;

using Weasel.Core;

namespace ShoppingList.Infrastructure.Tests.Fixtures;

public sealed class MartenFixture : IAsyncLifetime
{
    public PostgreSqlContainer PostgreSqlContainer { get; }  = new PostgreSqlBuilder().Build();
    public IShoppingListsRepository ShoppingListsRepository { get; private set; }
    public IDocumentSession Session { get; private set; }
    public IDocumentStore Store { get; private set; }
    public async Task InitializeAsync()
    {
        await PostgreSqlContainer.StartAsync();
        Store = DocumentStore.For(options =>
        {
            var schemaName = Environment.GetEnvironmentVariable("SchemaName") ?? "Shopping";
            options.Events.DatabaseSchemaName = schemaName;
            options.DatabaseSchemaName = schemaName;
            options.Connection(PostgreSqlContainer.GetConnectionString());

            options.UseDefaultSerialization(
                EnumStorage.AsString,
                nonPublicMembersStorage: NonPublicMembersStorage.All,
                serializerType: SerializerType.SystemTextJson
            );

            options.Projections.LiveStreamAggregation<Core.Model.CustomerShoppingList>();
        });
        Session = Store.LightweightSession();
    }

    public async Task DisposeAsync()
    {
        await Session.DisposeAsync();
        Store.Dispose();
        
        await PostgreSqlContainer.DisposeAsync();
    }
}