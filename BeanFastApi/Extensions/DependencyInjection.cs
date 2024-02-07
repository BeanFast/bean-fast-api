using BusinessObjects;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repositories.Interfaces;
using System.Text;
using Repositories.Implements;
using Services.Implements;
using Services.Interfaces;
using Utilities.Constants;
using Services.Mappers;
using BeanFastApi.Middlewares;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Utilities.Settings;
using Microsoft.Extensions.Configuration;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.DependencyInjection;


namespace BeanFastApi.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork<BeanFastContext>, UnitOfWork<BeanFastContext>>();
            return services;
        }
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionStringKey = "";
            if(Environment.GetEnvironmentVariable("database") == "local")
            {
                connectionStringKey = "LocalConnection";
            }
            else
            {
                connectionStringKey = "DefaultConnection";
            }
            services.AddDbContext<BeanFastContext>(options => options.UseSqlServer(configuration.GetConnectionString(connectionStringKey)));
            return services;
        }
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(ApiEndpointConstants.ApiVersion, new OpenApiInfo { Title = "Beanfast API", Version = ApiEndpointConstants.ApiVersion });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //ValidIssuer = configuration.GetValue<string>(JwtConstant.Issuer),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(JWTConstants.JwtSecret)),
                    ClockSkew = TimeSpan.Zero
                };
            });
            return services;
        }
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            
            services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFoodService, FoodService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IKitchenService, KitchenService>();
            services.AddScoped<ICloudStorageService, FirebaseCloudStorageService>();
            services.AddScoped<StorageClient>(f => StorageClient.Create());
            return services;
        }
        public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(FoodMapper),
                typeof(CategoryMapper),
                typeof(MenuMapper),
                typeof(KitchenMapper)
                
                //typeof(Program)
                ); // Add multiple mappers by passing the assembly containing the mapper profiles
            return services;
        }
        public static IServiceCollection AddAppSettingsBinding(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration);
            return services;
        }
        
    }
}
