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

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            });

            //builder.WebHost.ConfigureKestrel(options =>
            //{
            //    options.ListenAnyIP(5043); // HTTP
            //    options.ListenAnyIP(7160);
            //});


            // CORS sozlamalari - Hamma kelayotgan so'rovlarni ruxsat berish
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
           
            builder.Services.AddInfrastructureRegisterServices(builder.Configuration);
            


            builder.Services.AddScoped<IFileService, FileService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
        
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles();
            app.UseCors();
            app.MapControllers();

            app.Run();
        }
    }
}
