using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Infrastructure.Repositories;

namespace TaskManagement.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();

            return services;
        }
    }
}