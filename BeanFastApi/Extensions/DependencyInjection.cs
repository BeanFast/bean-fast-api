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
using Microsoft.AspNetCore.Authorization;
using Utilities.Settings;
using Google.Cloud.Storage.V1;
using System.Threading.RateLimiting;
using Utilities.Exceptions;
using Utilities.Utils;
using BeanFastApi.BackgroundJobs;


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
            if (Environment.GetEnvironmentVariable("database") == "local")
            {
                connectionStringKey = "LocalConnection";
            }
            else
            {
                connectionStringKey = "DefaultConnection";
            }
            services.AddDbContext<BeanFastContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(connectionStringKey));
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            });
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
            services.AddScoped(f => StorageClient.Create());
            services.AddScoped<IAreaService, AreaService>();
            services.AddScoped<ISchoolService, SchoolService>();
            services.AddScoped<IComboService, ComboService>();
            services.AddScoped<ICardTypeService, CardTypeService>();
            services.AddScoped<IGiftService, GiftService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderActivityService, OrderActivityService>();
            services.AddScoped<ISessionDetailService, SessionDetailService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IExchangeGIftService, ExchangeGiftService>();
            services.AddScoped<IMenuDetailService, MenuDetailService>();
            services.AddScoped<ISmsOtpService, SmsOtpService>();
            services.AddScoped<IOrderActivityService, OrderActivityService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ILoyaltyCardService, LoyaltyCardService>();
            services.AddScoped<IVnPayService, VnPayService>();
            services.AddScoped<IGameService, GameService>();
            return services;
        }
        public static IServiceCollection AddRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.AddPolicy("otpRateLimit", httpContext => RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress!.ToString(),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 1,
                        Window = TimeSpan.FromMinutes(1)
                    }
                ));
                options.OnRejected = (context, cancellationToken) =>
                {
                    Console.WriteLine(context.Lease.GetAllMetadata().FirstOrDefault(m => m.Key.Equals("RETRY_AFTER")).Value);
                    string retryAfterString = context.Lease.GetAllMetadata().FirstOrDefault(m => m.Key.Equals("RETRY_AFTER")).Value!.ToString()!;
                    var seconds = TimeUtil.ConvertTimeToSeconds(retryAfterString);
                    throw new TooManyRequestException(seconds);
                    //context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    //context.HttpContext.Response.ContentType = "application/json";
                    //var responseData = new { message = "Too many requests. Please try again later." };
                    //await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(responseData));
                };
            });
            return services;
        }
        public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(FoodMapper),
                typeof(CategoryMapper),
                typeof(MenuMapper),
                typeof(KitchenMapper),
                typeof(AreaMapper),
                typeof(CardTypeMapper),
                typeof(GiftMapper),
                typeof(WalletMapper),
                typeof(ExchangeGiftMapper),
                typeof(NotificationMapper),
                typeof(TransactionMapper),
                typeof(GameMapper)
                );
            return services;
        }
        public static IServiceCollection AddAppSettingsBinding(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration);
            return services;
        }
        public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
        {
            services.AddHostedService<SessionBackgroundService>();
            return services;
        }
        //public async static Task<WebApplication> UseBackgroundJobs(this WebApplication app)
        //{
        //    //app.UseRouting()
        //    using (var scope = app.Services.CreateScope())
        //    {
        //        var injectedService = scope.ServiceProvider;
        //        var timer = new PeriodicTimer(TimeSpan.FromMinutes(BackgroundJobConstrant.DelayedInMinutes));
        //        var sessionService = injectedService.GetRequiredService<ISessionService>();
        //        while (await timer.WaitForNextTickAsync())
        //        {
        //            await sessionService.UpdateOrdersStatusAutoAsync();
        //            Console.WriteLine(BackgroundJobConstrant.DelayedInMinutes);
        //            Console.Out.WriteLine(TimeUtil.GetCurrentVietNamTime());
        //        }

        //    }
        //    return app;
        //}


    }
}
