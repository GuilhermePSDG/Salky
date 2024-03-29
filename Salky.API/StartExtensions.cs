﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Npgsql;
using Salky.Domain;
using Salky.Domain.Contracts;

namespace Salky.API
{
    public static class StartExtensions
    {
        public static WebApplication MigrateDatabase<T>(this WebApplication webHost) where T : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var db = services.GetRequiredService<T>();
                    db.Database.EnsureCreated();
                    db.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
            return webHost;
        }


        public static IServiceCollection RegisterDomainEventsHandlers(this IServiceCollection collection, params string[] IncludeAssembly)
        {
            AppDomain.CurrentDomain.GetAssemblies()
                .Where(f => f.GetName().Name == AppDomain.CurrentDomain.FriendlyName || IncludeAssembly.Contains(f.GetName().Name))
                .SelectMany(f => f.GetTypes())
                .Where(item => item.GetInterfaces().Where(i => i.IsGenericType).Any(i => i.GetGenericTypeDefinition() == typeof(IHandler<>)) && !item.IsAbstract && !item.IsInterface)
                .ToList()
                .ForEach(assignedTypes =>
                {
                    var serviceType = assignedTypes.GetInterfaces().First(i => i.GetGenericTypeDefinition() == typeof(IHandler<>));
                    collection.AddScoped(serviceType, assignedTypes);
                });
            collection.AddScoped<IDispatcher, Dispatcher>();
            return collection;
        }


        public static WebApplicationBuilder UseLocalSqlite<T>(this WebApplicationBuilder builder) where T : DbContext
        {
            var ConStr = builder.Configuration.GetConnectionString("LocalSqlite");
            Console.WriteLine("CONNECITON STRING : "+ConStr);
            builder.Services.AddDbContext<T>(x => x
                .ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning))
                .UseSqlite(ConStr));
            return builder;
        }
        public static WebApplicationBuilder UseAwsPostgress<T>(this WebApplicationBuilder builder) where T : DbContext
        {
            var conStr = builder.Configuration.GetConnectionString("AwsPostGress");
            return builder.UsePostGress<T>(conStr);
        }

        public static WebApplicationBuilder UseHerokuPostGress<T>(this WebApplicationBuilder builder) where T : DbContext
        {
            return builder.UsePostGress<T>(Environment.GetEnvironmentVariable("DATABASE_URL") ?? throw new InvalidOperationException("Cannot Found ConnectionString In Environment Variables"));
        }

        private static WebApplicationBuilder UsePostGress<T>(this WebApplicationBuilder builder, string ConnectionString) where T : DbContext
        {
            var databaseUri = new Uri(ConnectionString);
            var userInfo = databaseUri.UserInfo.Split(':');
            var connectionStringBuider = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                IncludeErrorDetail = true,
            };

            builder.Services
                .AddNpgsql<T>(connectionStringBuider.ToString(), opt =>
                 {
                     opt.MigrationsAssembly(nameof(Salky.Domain));
                 });
            return builder;
        }


    }
}
