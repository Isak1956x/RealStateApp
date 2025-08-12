
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealState.Infraestructure.Persistence.Context;
using RealState.Infraestructure.Persistence.Repositories;
using RealStateApp.Core.Domain.Interfaces;

namespace RealState.Infraestructure.Persistence
{
    public static class ServicesRegistration
    {
        public static void AddPersistenceLayerIoc(this IServiceCollection services, IConfiguration configuration)
        {
            #region Contexts
            var conecctionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<RealStateContext>(options =>
                options.UseSqlServer(conecctionString,
                m => m.MigrationsAssembly(typeof(RealStateContext).Assembly.FullName)),
                ServiceLifetime.Scoped);
            #endregion

            #region Repositories IOC
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IFavoritePropertyRepository, FavoritePropertyRepository>();
            services.AddScoped<IImprovementRepository, ImprovementRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IOfferRepository, OfferRepository>();
            services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();
            services.AddScoped<IPropertyImprovementRepository, PropertyImprovementRepository>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IPropertyTypeRepository, PropertyTypeRepository>();
            services.AddScoped<ISaleTypeRepository, SaleTypeRepository>();
            #endregion

        }
    }
}
