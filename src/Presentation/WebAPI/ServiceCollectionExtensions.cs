using Contract.InfrastructureServices;
using Infrastructure.Adapters;
using Microsoft.OpenApi.Models;

namespace Presentation.WebAPI
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSwaggerService(
            this IServiceCollection services,
            IConfigurationRoot configuration)
        {

            // Infrastructure Services
            services.AddScoped<IManagementService, ManagementService>();

            if (configuration
                .GetSection("OAuthSetting")
                .GetSection("RunningWithIdentity").Value == "false")
            {
                services.AddSwaggerGen();
            }
            else
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Task Management API", Version = "v1" });

                    // Add the Bearer token support
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer"
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] { }
                        }
                    });
                });
            }
        }

        public static void UseSwaggerService(
            this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DefaultModelsExpandDepth(-1); // Disable swagger schemas at bottom
            });
        }
    }
}
