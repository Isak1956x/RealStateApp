using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Features.Common.GenericCommands;
using RealStateApp.Core.Application.Features.Common.GenericValidations;
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
            //services.AddMediatR(opt => opt.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
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

            #region Generic Validation
            services.AddGenericValidation<PropertyTypeDto>();
            services.AddGenericValidation<SaleTypeDto>();
            services.AddGenericValidation<ImprovementDto>();
            #endregion
        }

        public static void AddCQRS(this IServiceCollection services)
        {
            services.AddMediatR(opt => opt.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        }

        private static void AddGenericValidation<Tdto>(this IServiceCollection services)
        {
            services.AddTransient<IValidator<CreateResourceCommand<Tdto>>, CreateCommandValidation<Tdto>>();
            services.AddTransient<IValidator<UpdateResourceCommand<Tdto>>, UpdateCommandValidator<Tdto>>();
        }
    }
}
