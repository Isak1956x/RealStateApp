using Microsoft.Extensions.DependencyInjection;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.Services;
using System.Reflection;

namespace RealStateApp.Core.Application
{
    public static class ServicesRegistration
    {
        //Extension method - Decorator pattern
        public static void AddApplicationLayerIoc(this IServiceCollection services)
        {
            #region Configurations
            services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
            #endregion
            #region Services IOC
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IFavoritePropertyService, FavoritePropertyService>();
            services.AddScoped<IImprovementService, ImprovementService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IOfferService, OfferService>();
            services.AddScoped<IPropertyImageService, PropertyImageService>();
            services.AddScoped<IPropertyImprovementService, PropertyImprovementService>();
            services.AddScoped<IPropertyService, PropertyService>();
            services.AddScoped<IPropertyTypeService, PropertyTypeService>();
            services.AddScoped<ISaleTypeService, SaleTypeService>();
            #endregion
        }
    }
}
