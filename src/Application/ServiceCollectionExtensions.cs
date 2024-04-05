using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(
            this IServiceCollection services,
            DatabaseSettings databaseSettings,
            MongoDBSettings mongoDBSettings,
            InMemoryDatabaseSettings? inMemoryDatabaseSettings = null)
        {
            services.ConfigureDataStore(
                databaseSettings,
                mongoDBSettings,
                inMemoryDatabaseSettings);

            // MediatR Registrations
   

            // Application Services
      

            // Infrastructure Services
        }

        private static void ConfigureDataStore(
            this IServiceCollection services,
            DatabaseSettings databaseSettings,
            MongoDBSettings mongoDBSettings,
            InMemoryDatabaseSettings? inMemoryDatabaseSettings = null)
        {
            if (databaseSettings.IsInMemory)
            {
            }
            else
            {
                var client = new MongoClient(mongoDBSettings.ConnectionString);
                IMongoDatabase database = client.GetDatabase(mongoDBSettings.DatabaseName);
                services.AddSingleton(database);
            }

            new DatabaseInitialization(services).Initialize();
        }
    }
 }
