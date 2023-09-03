using JasperFx.CodeGeneration;

using Marten;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using Marten.Services.Json;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Weasel.Core;

namespace ShoppingList.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMarten(sp =>
            {
                var options = new StoreOptions();

                var schemaName = Environment.GetEnvironmentVariable("SchemaName") ?? "Shopping";
                options.Events.DatabaseSchemaName = schemaName;
                options.DatabaseSchemaName = schemaName;
                options.Connection(configuration.GetConnectionString("ShoppingLists") ??
                                   throw new InvalidOperationException());

                options.UseDefaultSerialization(
                    EnumStorage.AsString,
                    nonPublicMembersStorage: NonPublicMembersStorage.All,
                    serializerType: SerializerType.SystemTextJson
                );

                options.Projections.LiveStreamAggregation<Core.Model.ShoppingList>();
                return options;
            })
            .OptimizeArtifactWorkflow(TypeLoadMode.Static)
            .UseLightweightSessions()
            .AddAsyncDaemon(DaemonMode.Solo);

        return services;
    }
}