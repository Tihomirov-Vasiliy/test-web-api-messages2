using Application.Intrefaces;
using Infrastructure.Stores;
using Infrastructure.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IMessageService, MessageServiceInMemory>();
            services.AddSingleton<InMemoryRecipientsStore>();

            return services;
        }
    }
}
