using Application.UnitTests.MockedServices;
using Contract.InfrastructureServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Configuration;
using System;

namespace Application.UnitTests
{
    public abstract class ServiceContext
    {
        public ServiceProvider ServiceProvider { get; set; }
        public ServiceContext()
        {
            var configuration = new ConfigurationBuilder().
                AddJsonFile("appsettings.Test.json").Build();

            var services = new ServiceCollection();

            // Infrastructure Services
            services.AddScoped<IManagementService, MockedManagementService>();

            var databaseSettings = new DatabaseSettings
            {
                IsInMemory = true
            };

            var inMemoryDatabaseSettings = new InMemoryDatabaseSettings
            {
                DatabaseName = Guid.NewGuid().ToString()
            };

            services.ConfigureApplicationServices(
                databaseSettings: databaseSettings,
                inMemoryDatabaseSettings: inMemoryDatabaseSettings);

            services.ConfigureLanguage(configuration.GetSection("AppLanguage").Value!);

            ServiceProvider = services.BuildServiceProvider(validateScopes: true);

            EnsureRecreatedDatabase();
        }

        public void EnsureRecreatedDatabase()
        {
            var serviceScope = ServiceProvider.CreateAsyncScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<TodoDbContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
        public void Dispose()
        {
            if (ServiceProvider != null)
                ServiceProvider.Dispose();
        }
    }
}