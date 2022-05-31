using System;
using System.Collections.Generic;
using System.Text;
using Salky.App.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Salky.App.Security;
using Salky.Domain.Repositories;
using Salky.Domain.Contexts;
using Salky.Domain.Models.UserModels;
using Salky.Domain.Contracts;
using Salky.App.Services.Group;
using Salky.App.Services.Friends;
using Salky.App.Services.User;

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
                {
                    var key = new byte[byte.MaxValue];
                    for(int i = 0; i < key.Length; i++)
                        key[i] = (byte)i;
                    Environment.SetEnvironmentVariable("JWTKEY", Convert.ToBase64String(key));
                    _serviceProvider = BuildNewProvider();
                    
                }
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
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<UserService>();
            services.AddScoped<GroupMessageService>();
            services.AddScoped<GroupRepository>();
            services.AddScoped<FriendRepository>();
            services.AddScoped<GroupService>();
            services.AddScoped<FriendService>();
            services.AddScoped<GroupMemberService>();
            services.AddScoped<FriendMessageService>();
            services.AddScoped<IFriendRepository, FriendRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            services.AddScoped<IMessageFriendRepository, MessageFriendRepository>();
            services.AddScoped<IMessageGroupRepository, MessageRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IGroupMemberRepository, GroupMemberRepository>();
            services.AddScoped<IMessageGroupRepository, MessageRepository>();

            //services.AddIdentityCore<User>(options =>
            //{
            //    options.Password.RequireDigit = false;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequireLowercase = false;
            //    options.Password.RequireUppercase = false;
            //    options.Password.RequiredLength = 2;
            //    options.User.RequireUniqueEmail = false;
            //})
            //    .AddRoles<Role>()
            //    .AddRoleManager<RoleManager<Role>>()
            //    .AddSignInManager<SignInManager<User>>()
            //    .AddRoleValidator<RoleValidator<Role>>()
            //    .AddEntityFrameworkStores<SalkyDbContext>()
            //    .AddDefaultTokenProviders();

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
            var provider = services.BuildServiceProvider();
            provider.GetService<SalkyDbContext>().Database.EnsureCreated();
            return provider;
        }
    }
}
