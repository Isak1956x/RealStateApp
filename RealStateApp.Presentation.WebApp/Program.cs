using RealState.Infraestructure.Persistence;
using RealStateApp.Core.Application;
using RealStateApp.Core.Application.Interfaces.Infraestructure.Shared;
using RealStateApp.Infraestructure.Identity;
using RealStateApp.Infraestructure.Shared;
using RealStateApp.Presentation.WebApp.Handlers;

namespace RealStateApp.Presentation.WebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSession(opt =>
            {
                opt.IdleTimeout = TimeSpan.FromMinutes(60);
                opt.Cookie.HttpOnly = true;
            });

            builder.Services.AddPersistenceLayerIoc(builder.Configuration);
            builder.Services.AddIdentityServiceForWebApp(builder.Configuration);
            builder.Services.AddApplicationLayerIoc();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddSharedLayerService(builder.Configuration);

            var app = builder.Build();
            await app.Services.RunIdentitySeedAsyn();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
       
}
