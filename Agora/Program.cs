using Agora.Application.DTOs.Errors;
using Agora.Application.Interfaces;
using Agora.Infrastructure;
using Agora.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Agora
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddOpenApi(); // Agar siz OpenAPI uchun maxsus extension ishlatayotgan bo‘lsangiz

            // CORS sozlamalari
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Kestrel port sozlamasi
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(5043); // HTTP
            });

            // Service-lar
            builder.Services.AddInfrastructureRegisterServices(builder.Configuration);
            builder.Services.AddScoped<IFileService, FileService>();

            var app = builder.Build();

            // Middleware tartibi muhim
            app.UseStaticFiles();

            // ⚠️ HTTPS yo‘naltirishni o‘chirib qo‘yamiz, agar SSL ishlatilmasa
            // app.UseHttpsRedirection();

            app.UseCors(); // CORS doim avval

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapOpenApi(); // Optional
            app.MapControllers();

            app.Run();
        }
    }
}
