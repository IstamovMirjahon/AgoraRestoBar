using Agora.Application.Interfaces;
using Agora.Infrastructure;
using Agora.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Project-specific services
builder.Services.AddInfrastructureRegisterServices(builder.Configuration);
builder.Services.AddScoped<IFileService, FileService>();

var app = builder.Build();

// Middleware
app.UseStaticFiles();
// app.UseHttpsRedirection(); // Agar HTTPS ishlatilmayotgan bo‘lsa, o‘chirib qo‘ying

app.UseCors();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
