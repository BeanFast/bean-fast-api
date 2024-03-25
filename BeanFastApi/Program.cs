using BeanFastApi.Middlewares;
using System.Text.Json;
using BeanFastApi.Extensions;
using Utilities.Constants;
using Utilities.Utils;
using Microsoft.AspNetCore.Http.Features;
using static System.Net.Mime.MediaTypeNames;
using Repositories.Interfaces;
using BusinessObjects.Models;
using BusinessObjects;
using Google.Apis.Auth.OAuth2;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.
Console.WriteLine(DateTime.Now);
services.AddHttpContextAccessor();
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
//Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALSJSON", GoogleCredential.FromFile("bean-fast-firebase-adminsdk.json"));
//credential = GoogleCredential.FromFile(credentialPath);
app.UseMiddleware<ExceptionHandlingMiddleWare>();
//using (var scope = app.Services.CreateScope())
//{
//    var userRepo = scope.ServiceProvider.GetRequiredService<IUnitOfWork<BeanFastContext>>().GetRepository<User>();
//    Console.WriteLine(await userRepo.CountAsync());
//}
app.UseHttpsRedirection();
app.UseAuthentication();
//app.UseRouting()
app.UseAuthorization();
app.UseCors(CorsConstrant.AllowAllPolicyName);
app.MapControllers();
app.UseRateLimiter();
app.Run();
