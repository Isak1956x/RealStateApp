using Microsoft.AspNetCore.Mvc;
using RealState.Infraestructure.Persistence;
using RealStateApp.Core.Application;
using RealStateApp.Infraestructure.Identity;
using RealStateApp.Infraestructure.Shared;
using RealStateApp.Presentation.API.Extensions;
using System.Threading.Tasks;

using RealStateApp.Presentation.API.Handlers;

namespace RealStateApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(opt =>
            {
                opt.Filters.Add(new ProducesAttribute("application/json"));
            }).ConfigureApiBehaviorOptions(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true; 
                opt.SuppressMapClientErrors = true;
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddControllersWithViews();
            builder.Services.AddPersistenceLayerIoc(builder.Configuration);
            builder.Services.AddIdentityServiceForWebApi(builder.Configuration);
            builder.Services.AddApplicationLayerIoc();
            builder.Services.AddCQRS();
            builder.Services.AddSharedLayerService(builder.Configuration);
         //  builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHealthChecks();
            
            builder.Services.AddApiVersioningExtension();
            builder.Services.AddSwaggerExtension();

            var app = builder.Build();
            await app.Services.RunIdentitySeedAsyn();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerExtension(app);
                app.MapOpenApi();
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            app.UseHealthChecks("/health");



            app.MapControllers();

            await app.RunAsync();
        }
    }
}
