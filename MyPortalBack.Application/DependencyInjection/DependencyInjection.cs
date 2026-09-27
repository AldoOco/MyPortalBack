using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MyPortalBack.Application.Common.Interfaces;
using MyPortalBack.Application.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace MyPortalBack.Application.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //Services
            services.AddScoped<UserOperations>();
            services.AddScoped<AuthOperations>();
            services.AddScoped<RoleOperations>();
            services.AddScoped<UserRoleOperations>();

        return services;
        }
    }

