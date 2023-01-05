using Application.Intrefaces;
using Infrastructure.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddWebApiServices(this IServiceCollection services)
        {
            services.AddScoped<IMessageService, MessageService>();

            return services;
        }
    }
}
