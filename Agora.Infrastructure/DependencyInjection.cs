using Agora.Application.Interfaces;
using Agora.Application.Services;
using Agora.Domain.Abstractions;
using Agora.Infrastructure.Data;
using Agora.Infrastructure.Repositories;
using Agora.Infrastructure.Repositories.Admin;
using Agora.Infrastructure.Repositories.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Agora.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureRegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                throw new ArgumentNullException(nameof(configuration));

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                // Corrected: Removed UseSnakeCaseNamingConvention as it is not a built-in method.
                // If snake_case naming is required, configure it in the ApplicationDbContext model configuration.
                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IBannerRepository, BannerRepository>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IBannerService, BannerService>();

            services.AddAutoMapper(typeof(IBannerService).Assembly);


            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
            return services;
        }
    }
}
