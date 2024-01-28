using BeanFastApi.Middlewares;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using BeanFastApi.Extensions;
using Services.Mappers;
using Utilities.Utils;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.


services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    
    //options.
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();

services.AddAutoMapperProfiles();
services.AddSwaggerGen();
services.AddJWTAuthentication();
services.AddDatabase(builder.Configuration);
services.AddUnitOfWork();
services.AddServices();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

 app.UseMiddleware<ExceptionHandlingMiddleWare>();
//app.UseMiddleware<ResponseSuccessMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
//app.UseRouting()
app.UseAuthorization();

app.MapControllers();

app.Run();
