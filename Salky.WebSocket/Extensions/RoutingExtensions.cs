using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Salky.WebSocket.Infra.Interfaces;
using Salky.WebSocket.Infra.RoutingMinimal;

namespace Salky.WebSocket.Extensions;

public static class RoutingExtensions
{

    public static IServiceCollection UseSalkyRouter(this IServiceCollection serviceCollection)
    {
        foreach (var @class in RouteResolver.RouteClassTypes)
            serviceCollection.AddTransient(@class);
        return serviceCollection.AddSingleton<ISalkyRouter, RouteResolver>();
    }

    public static IServiceCollection UseMinimalRouter(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<ISalkyRouter, MinimalRouting>();
    }

    public static void BuildMinimalRouting(this IApplicationBuilder app, Action<RouteBuilderOn> action)
    {
        var context = (MinimalRouting)app.ApplicationServices.GetRequiredService<ISalkyRouter>();
        var builder = RoutingBuilder.Create(context);
        action.Invoke(builder);
    }


}