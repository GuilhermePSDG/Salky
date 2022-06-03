using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Salky.WebSocket.Handler;
using Salky.WebSocket.Infra.Interfaces;




namespace Salky.WebSocket.Extensions;

public static class RoutingExtensions
{

    public static IServiceCollection UseSalkyWebSocketRouter(this IServiceCollection serviceCollection)
    {
        foreach (var @class in RouteResolver.RouteClassTypes)
            serviceCollection.AddScoped(@class);
        return serviceCollection
            .AddTransient<IRouterResolver, RouteResolver>()
            .AddSingleton<IConnectionManager, ConnectionMannager>();
    }


}