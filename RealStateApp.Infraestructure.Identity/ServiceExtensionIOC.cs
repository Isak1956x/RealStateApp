using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Settings;
using RealStateApp.Infraestructure.Identity.Context;
using RealStateApp.Infraestructure.Identity.Entities;
using RealStateApp.Infraestructure.Identity.Seeds;
using RealStateApp.Infraestructure.Identity.Service;
using RealStateApp.Infraestructure.Identity.Services;
using System.Text;

namespace RealStateApp.Infraestructure.Identity
{
    public static class ServiceExtensionIoC
    {
         
        public static IServiceCollection AddIdentityServiceForWebApp(this IServiceCollection services, IConfiguration configuration)
        {
            GeneralConfig(services, configuration);

            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = true;

                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.Lockout.MaxFailedAccessAttempts = 5;

                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = true;
            });

            services.AddIdentityCore<AppUser>()
                .AddRoles<IdentityRole>()
                .AddSignInManager()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromHours(3);
            });
            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = IdentityConstants.ApplicationScheme;
                opt.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                opt.DefaultSignInScheme = IdentityConstants.ApplicationScheme;

            }).AddCookie(IdentityConstants.ApplicationScheme, opt =>
            {
                opt.ExpireTimeSpan = TimeSpan.FromHours(4);
                opt.SlidingExpiration = true;
                opt.LoginPath = "/Login";
                opt.AccessDeniedPath = "/Login";
            });
            services.AddScoped<IAccountServiceForWebApp, AccountServiceForWebApp>();

            return services;
        }

        public static IServiceCollection AddIdentityServiceForWebApi(this IServiceCollection services, IConfiguration configuration)
        {
            GeneralConfig(services, configuration);
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = true;

                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.Lockout.MaxFailedAccessAttempts = 5;

                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = true;
            });

            services.AddIdentityCore<AppUser>()
                .AddRoles<IdentityRole>()
                .AddSignInManager()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromHours(3);
            });
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(opt =>
            {
                opt.RequireHttpsMetadata = false;
                opt.SaveToken = false;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"] ?? ""))
                };
                opt.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        context.NoResult();
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "text/plain";
                        return context.Response.WriteAsync(context.Exception.Message.ToString());
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync("Unauthorized access. Please login.");
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync("You do not have permission to access this resource.");
                    }
                };

            });
            services.AddScoped<IAccountServiceForApi, AccountServiceForWebApi>();

            return services;
        }


        private static void GeneralConfig(IServiceCollection services, IConfiguration configuration)
        {
            //if(configuration.GetValue<bool>(U))
            var connectionString = configuration.GetConnectionString("IdentityConnection");
            services.AddDbContext<IdentityContext>(opt =>
            {
                opt.EnableSensitiveDataLogging();
                opt.UseSqlServer(connectionString, m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));

            }, contextLifetime: ServiceLifetime.Scoped,
                   optionsLifetime: ServiceLifetime.Scoped
            );
        }

        public static async Task RunIdentitySeedAsyn(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var userManager = provider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
                await DefaultRoles.SeedAsync(roleManager);
                await DefaultIdentUser.SeedAsync(userManager);
                //await DefaultIdentUser.SeedAsync(userManager);
                await DefaultAdminUser.SeedAsync(userManager);
                await DefaultAdminUser.SeedAsync(userManager);
            }
        }
    }
}
