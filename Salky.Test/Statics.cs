global using static Salky.Test.Statics;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Salky.App.Dtos.Users;
using Salky.App.Services.User;
using Salky.App.Interfaces;

namespace Salky.Test
{

    public static class Statics
    {
        public static string GetRandomString => Guid.NewGuid().ToString().Replace("-", "")+"Ab1@s";
        public static string GetRandomSimpleString => GetString(8);
        private static string GetString(int n)
        {
            var l = "qwertyuiopasdfghjklzxcvbnm";
            var rng = new Random();
            return new String(Enumerable.Range(0, n).Select(x => l[rng.Next(l.Length)]).ToArray());
        }


        public static T GetRequiredService<T>()
        {
            return Provider.serviceProvider.GetRequiredService<T>();
        }

        public static void CreateRandomUsers(this IAccountService userService,int count, out UserLoggedDto[] users)
        {
            users = Enumerable.Range(0, count)
                .Select(f =>
                {
                    return userService.CreateAccountAsync(new()
                    {
                        Password = GetRandomString + $"{f}",
                        UserName = GetRandomString + $"{f}"
                    }).Result.Data ?? throw new Exception("Falha ao criar conta");
                })
                .ToArray();
        }

    }
}
