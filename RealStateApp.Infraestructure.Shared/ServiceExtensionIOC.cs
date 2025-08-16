using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealStateApp.Core.Application.Interfaces.Infraestructure.Shared;
using RealStateApp.Core.Domain.Settings;
using RealStateApp.Infraestructure.Shared.Email;

namespace RealStateApp.Infraestructure.Shared
{
    public static class ServiceExtensionIOC
    {
        public static IServiceCollection AddSharedLayerService(this IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddScoped<IEmailService, EmailService>();
            return services;
        }
    }
}
