global using static Salky.Test.Statics;
using System;
using Salky.App.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Salky.App.Dtos.Auth;

namespace Salky.Test
{

    public static class Statics
    {
        public static string GetRandomString => Guid.NewGuid().ToString().Replace("-", "")+"Ab1@s";
        public static T GetService<T>()
        {
            return Provider.serviceProvider.GetService<T>();
        }

        public static void CreateRandomUsers(this UserService userService,int count, out UserLoggedDto[] users)
        {
            users = Enumerable.Range(0, count)
                .Select(f =>
                {
                    return userService.CreateAccountAsync(new()
                    {
                        Password = GetRandomString + $"{f}",
                        UserName = GetRandomString + $"{f}"
                    }).Result ?? throw new Exception("Falha ao criar conta");
                })
                .ToArray();
        }

    }
}
