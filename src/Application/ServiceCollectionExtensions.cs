using Contract;
using Contract.InfrastructureServices;
using Domain;
using Domain.ProjectSetting;
using Domain.Task;
using Infrastructure.Adapters;
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

            // Application Services
            services.AddScoped<IProjectSettingService, ProjectSettingService>();
            services.AddScoped<ITaskService, TaskService>();

            // Persistence Services
            services.AddScoped<IProjectSettingRepository, ProjectSettingRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();

            // Domain Services
            services.AddDomainServices();
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
