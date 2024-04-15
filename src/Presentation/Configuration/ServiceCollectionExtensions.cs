using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Application;

namespace Presentation.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureApplicationServices(
            this IServiceCollection services,
            IConfigurationRoot configuration)
        {
            var databaseSettings = new DatabaseSettings();
            configuration.GetSection("DatabaseSettings").Bind(databaseSettings);

            var inMemoryDatabaseSettings = new InMemoryDatabaseSettings();
            configuration.GetSection("InMemoryDatabaseSettings").Bind(inMemoryDatabaseSettings);

            var sqlServerSettings = new SqlServerSettings();
            configuration.GetSection("SqlServerSettings").Bind(sqlServerSettings);

            services.ConfigureApplicationServices(
                databaseSettings: databaseSettings,
                inMemoryDatabaseSettings: inMemoryDatabaseSettings,
                sqlServerSettings: sqlServerSettings);
        }

        public static void ConfigureApplicationServices(
            this IServiceCollection services,
            DatabaseSettings databaseSettings,
            SqlServerSettings? sqlServerSettings = null,
            InMemoryDatabaseSettings? inMemoryDatabaseSettings = null)
        {
            //-- Application
            services.AddApplicationServices(
                databaseSettings,
                sqlServerSettings,
                inMemoryDatabaseSettings);
        }

        public static void ConfigureLanguage(
            this IServiceCollection services,
            IConfigurationRoot configuration)
        {
            services.ConfigureLanguage(
                appLanguage: configuration.GetSection("AppLanguage").Value!);
        }

        public static void ConfigureLanguage(
            this IServiceCollection services,
            string appLanguage)
        {
            Thread.CurrentThread.CurrentUICulture =
                CultureInfo.GetCultureInfo(appLanguage);
        }
    }
}
