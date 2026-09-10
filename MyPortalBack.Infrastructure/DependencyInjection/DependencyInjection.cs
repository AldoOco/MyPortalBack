using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyPortalBack.Application.Common.Interfaces;
using MyPortalBack.Infrastructure.Persistence.Context;
using MyPortalBack.Infrastructure.Repositories;
using MyPortalBack.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
       this IServiceCollection services,
       IConfiguration configuration)
    {
        // Aquí iremos registrando:
        // DbContext
        // Repositories
        // Servicios
        // Identity
        // etc.
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

        return services;
    }
}
