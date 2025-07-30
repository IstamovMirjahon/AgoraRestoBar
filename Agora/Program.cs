using Agora.Application.Interfaces;
using Agora.Infrastructure;
using Agora.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();




// Add services
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

builder.WebHost.UseUrls("http://0.0.0.0:5043");

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
builder.Services.AddEndpointsApiExplorer();
// Project-specific services
builder.Services.AddInfrastructureRegisterServices(builder.Configuration);
builder.Services.AddScoped<IFileService, FileService>();

var app = builder.Build();

// Middleware

// app.UseHttpsRedirection(); // Agar HTTPS ishlatilmayotgan bo‘lsa, o‘chirib qo‘ying

app.UseCors();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.MapOpenApi();
app.UseStaticFiles();
app.MapControllers();

app.Run();
