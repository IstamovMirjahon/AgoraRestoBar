using Agora.Application.Interfaces;
using Agora.Infrastructure;
using Agora.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Agora API",
        Version = "v1"
    });
});
builder.WebHost.UseUrls("http://0.0.0.0:5043");
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


builder.Services.AddInfrastructureRegisterServices(builder.Configuration);
builder.Services.AddScoped<IFileService, FileService>();

var app = builder.Build();

app.UseCors();
// app.UseHttpsRedirection(); // HTTPS kerak bo‘lmasa o‘chirilsin

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agora v1");
});

app.UseStaticFiles();
app.MapControllers();

app.Run();
