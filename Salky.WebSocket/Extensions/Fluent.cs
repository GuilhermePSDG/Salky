using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Salky.WebSocket.Contracts;
using Salky.WebSocket.Handler;
using Salky.WebSocket.Infra.Interfaces;
using Salky.WebSocket.Interfaces;
using Salky.WebSocket.Routing;
using System.Collections;

namespace Salky.WebSocket.Extensions;

public interface IBuildHandShake
{
    public IBuildHandShake UseHttpHandShake<THandShaker>() where THandShaker : class, HttpWebSocketGuardIdentityProvider;
}
public class Builder : IBuildHandShake
{
    public Builder(IServiceCollection serviceCollection)
    {
        this.serviceCollection = serviceCollection;
    }

    public IServiceCollection serviceCollection { get; }
    
    public IBuildHandShake UseHttpHandShake<THandShaker>() where THandShaker : class, HttpWebSocketGuardIdentityProvider
    {
        serviceCollection.AddScoped<HttpWebSocketGuardIdentityProvider, THandShaker>();
        return this;
    }
    public IBuildHandShake UseSalkyWebSocktHandShakerMiddleWare<THandShaker>() where THandShaker : class, IDoWebSocketHandShake
    {
        serviceCollection.AddScoped<THandShaker, THandShaker>();
        return this;
    }
}

public static class Fluent
{
    public static IBuildHandShake UseSalkyWebSocketRouter(this IServiceCollection serviceCollection)
    {
        foreach (var @class in RouteMapper.AllWebSocketRoutesClass)
            serviceCollection.AddScoped(@class);
        serviceCollection
            .AddSingleton<IPoolMannager, ConnectionMannager>()
            .AddScoped<IRouteList, SingletonRouteList>()
            .AddScoped<IRouterResolver, RouteResolver>()
            .AddScoped<IRouteMapper, RouteMapper>()
            .AddScoped<IRouteParametersParser, SingleParameterRouteParser>();
        return new Builder(serviceCollection);
    }

}
