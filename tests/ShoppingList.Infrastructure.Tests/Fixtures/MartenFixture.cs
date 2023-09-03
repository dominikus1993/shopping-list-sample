using Testcontainers.PostgreSql;

namespace ShoppingList.Infrastructure.Tests.Fixtures;

public sealed class MartenFixture : IAsyncLifetime
{
    public PostgreSqlContainer PostgreSqlContainer { get; }  = new PostgreSqlBuilder().Build();

    public async Task InitializeAsync()
    {
        await PostgreSqlContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await PostgreSqlContainer.DisposeAsync();
    }
}