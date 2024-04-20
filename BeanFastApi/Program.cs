using BeanFastApi.Middlewares;
using System.Text.Json;
using BeanFastApi.Extensions;
using Utilities.Constants;
using Utilities.Utils;
using System.Globalization;
using Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Services.Implements;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.
//Console.WriteLine(TimeUtil.GetCurrentVietNamTime().Date);
//QrCodeUtil.GenerateQRCode("3e24d372-1776-4dc2-81f3-a6322317aad3");
Console.WriteLine(TimeUtil.GetCurrentVietNamTime().ToString());
Console.WriteLine(QrCodeUtil.GenerateQRCodeString("C4D5E6F7-A8B9-4C3D-9E8F-0A1B2C3D4E5F" + "AD000002" + TimeUtil.GetCurrentVietNamTime().ToString(), "this_is_a_very_secret_key"));
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
services.AddBackgroundServices();
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


app.UseAuthorization();
app.UseCors(CorsConstrant.AllowAllPolicyName);
app.MapControllers();
app.UseRateLimiter();

app.Run();
