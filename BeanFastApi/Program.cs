using BeanFastApi.Middlewares;
using System.Text.Json;
using BeanFastApi.Extensions;
using Utilities.Constants;
using Utilities.Utils;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.
//Console.WriteLine(TimeUtil.GetCurrentVietNamTime().Date);
//QrCodeUtil.GenerateQRCode("3e24d372-1776-4dc2-81f3-a6322317aad3");
services.AddHttpContextAccessor();
services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddAutoMapperProfiles();
services.AddJwtAuthentication();
services.AddDatabase(builder.Configuration);
services.AddUnitOfWork();
services.AddServices();
services.AddSwagger();
services.AddAppSettingsBinding(builder.Configuration);
services.AddRateLimiting();
services.AddCors(options =>
{
    options.AddPolicy(CorsConstrant.AllowAllPolicyName,
               builder =>
               {
                   builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
               });
});
var app = builder.Build();
// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "bean-fast-firebase-adminsdk.json");
app.UseMiddleware<ExceptionHandlingMiddleWare>();
app.UseHttpsRedirection();
app.UseAuthentication();
//app.UseRouting()
app.UseAuthorization();
app.UseCors(CorsConstrant.AllowAllPolicyName);
app.MapControllers();
app.UseRateLimiter();
app.Run();
