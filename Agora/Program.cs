using Agora.Application.Interfaces;
using Agora.Infrastructure;
using Agora.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Agora API",
        Version = "v1"
    });
});

// Controllers va JSON sozlamalar
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

// Kestrel HTTP port sozlovi
builder.WebHost.UseUrls("http://0.0.0.0:5043");


// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Custom service'lar
builder.Services.AddInfrastructureRegisterServices(builder.Configuration);
builder.Services.AddScoped<IFileService, FileService>();

var app = builder.Build();

// Middlewarelar
app.UseCors();
// HTTPS yo‘q, shuning uchun quyidagini ishlatmaysiz:
// app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agora API v1");
    c.RoutePrefix = "swagger"; // bu URLni aniq qiladi
});

app.UseStaticFiles();
app.MapControllers();

app.Run();
