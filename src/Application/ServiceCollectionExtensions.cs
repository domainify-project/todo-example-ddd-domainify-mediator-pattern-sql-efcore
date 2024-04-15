using Contract;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(
            this IServiceCollection services,
            DatabaseSettings databaseSettings,
            SqlServerSettings? sqlServerSettings = null,
            InMemoryDatabaseSettings? inMemoryDatabaseSettings = null)
        {
            services.ConfigureDataStore(
                databaseSettings,
                sqlServerSettings,
                inMemoryDatabaseSettings);

            // MediatR Registrations
            services.AddMediatR(typeof(ProjectSettingService));
            services.AddMediatR(typeof(Persistence.ProjectSettingStore.DefineProjectHandler));
            services.AddMediatR(typeof(Domain.ProjectSettingAggregation.DefineProject));

            // Application Services
            services.AddScoped<IProjectSettingService, ProjectSettingService>();
            services.AddScoped<ITaskService, TaskService>();

            // Infrastructure Services
        }

        private static void ConfigureDataStore(
            this IServiceCollection services,
            DatabaseSettings databaseSettings,
            SqlServerSettings? sqlServerSettings = null,
            InMemoryDatabaseSettings? inMemoryDatabaseSettings = null)
        {
            services.AddScoped<IDbTransaction, TodoDbTransaction>();

            if (databaseSettings.IsInMemory)
            {
                services.AddDbContext<TodoDbContext>(options =>
                   options.UseInMemoryDatabase(databaseName: inMemoryDatabaseSettings!.DatabaseName)
                   .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning)),
                   ServiceLifetime.Scoped);
            }
            else
            {
                var assembly = typeof(TodoDbContext).Assembly.GetName().Name;
                services.AddDbContext<TodoDbContext>(options =>
                   options.UseSqlServer(
                       sqlServerSettings!.ConnectionString!,
                       b => b.MigrationsAssembly(assembly)),
                   ServiceLifetime.Scoped);
            }

            new DatabaseInitialization(services).Initialize();
        }
    }
 }
