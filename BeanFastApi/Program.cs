using BeanFastApi.Middlewares;
using System.Text.Json;
using BeanFastApi.Extensions;

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
services.AddJwtAuthentication();
services.AddDatabase(builder.Configuration);
services.AddUnitOfWork();
services.AddServices();
services.AddSwagger();
services.AddAppSettingsBinding(builder.Configuration);
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "bean-fast-firebase-adminsdk.json");
app.UseMiddleware<ExceptionHandlingMiddleWare>();

//app.UseMiddleware<ResponseSuccessMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
//app.UseRouting()
app.UseAuthorization();

app.MapControllers();

app.Run();
