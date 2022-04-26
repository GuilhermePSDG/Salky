using System;
using System.Collections.Generic;
using System.Text;
using Salky.App.Services;
using Salky.App.Interfaces;
using Salky.Persistence.Persist;
using Microsoft.Extensions.DependencyInjection;
using Salky.Persistence.Contexts;
using Salky.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace Salky.Test
{
    public static class Provider
    {
        private static ServiceProvider? _serviceProvider = null;

        public static ServiceProvider serviceProvider
        {
            get
            {
                if (_serviceProvider == null)
                    _serviceProvider = BuildNewProvider();
                return _serviceProvider;
            }
        }

        private static ServiceProvider BuildNewProvider()
        {
            if (File.Exists("teste.db"))
                File.Delete("teste.db");

            var tokenKey = Guid.NewGuid().ToString() + Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
            var services = new ServiceCollection();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IConfiguration>(f =>
            {
                var dc = new Dictionary<string, string>();
                dc.Add("TokenKey", tokenKey);
                return new FakeConfiguration(dc);
            });
            services.AddScoped<UserRepository>();
            services.AddScoped<MessageRepository>();
            services.AddScoped<ContactRepository>();
            services.AddScoped<IAccountService, UserService>();
            services.AddScoped<UserService>();
            services.AddScoped<ContactService>();
            services.AddScoped<MessageService>();
            services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 2;
                options.User.RequireUniqueEmail = false;
            })
                .AddRoles<Role>()
                .AddRoleManager<RoleManager<Role>>()
                .AddSignInManager<SignInManager<User>>()
                .AddRoleValidator<RoleValidator<Role>>()
                .AddEntityFrameworkStores<SalkyDbContext>()
                .AddDefaultTokenProviders();

            services.AddDbContext<SalkyDbContext>(
                           context =>
                           {
                               context.UseSqlite("Data Source=teste.db");
                           });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            return services.BuildServiceProvider();
        }
    }
}
