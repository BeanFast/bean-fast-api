﻿using BeanFastApi.Middlewares;
using System.Text.Json;
using BeanFastApi.Extensions;
using Utilities.Constants;
using Utilities.Utils;
using Microsoft.AspNetCore.Http.Features;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.
services.AddHttpContextAccessor();
//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.Limits.MaxRequestBodySize = null;
//});
//services.Configure<FormOptions>(x =>
//{
//    x.ValueLengthLimit = int.MaxValue;
//    x.MultipartBodyLengthLimit = int.MaxValue;
//    // In case of multipart
//});

services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
//Console.WriteLine(EntityCodeUtil.GenerateNamedEntityCode(EntityCodeConstrant.FoodCodeConstrant.FoodPrefix, "bánh mì bơ sữa", Guid.Parse("8513b8b8-106d-4fc5-b8b2-66d6f9b5861a")));
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
var qrCodeByteArray = QrCodeUtil.GenerateQRCode("Hello_world");

app.UseHttpsRedirection();
app.UseAuthentication();
//app.UseRouting()
app.UseAuthorization();
app.UseCors(CorsConstrant.AllowAllPolicyName);
app.MapControllers();
app.UseRateLimiter();
app.Run();
