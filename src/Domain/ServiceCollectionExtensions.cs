using Domain.ProjectSetting;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDomainServices(
            this IServiceCollection services)
        {
            services.AddMediatR(typeof(DefineProjectHandler));
            services.AddMediatR(typeof(DefineProject));
        }
    }
 }
