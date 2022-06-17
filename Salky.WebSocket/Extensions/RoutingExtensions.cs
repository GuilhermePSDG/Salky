using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Salky.WebSocket.Handler;
using Salky.WebSocket.Infra.Interfaces;




namespace Salky.WebSocket.Extensions;

public interface IBuildHandShake
{
    public IBuildHandShake UseSalkyHttpHandShakerMiddleWare<THandShaker>() where THandShaker : class, IDoHttpHandshake;
    public IBuildHandShake UseSalkyWebSocktHandShakerMiddleWare<THandShaker>() where THandShaker : class, IDoWebSocketHandShake;
}
public class Builder : IBuildHandShake
{
    public Builder(IServiceCollection serviceCollection)
    {
        this.serviceCollection = serviceCollection;
    }

    public IServiceCollection serviceCollection { get; }

    /// <summary>
    /// You Must Provide a <see langword="class"/> that inherits from <see cref="IDoHttpHandshake"/>
    /// <para>if you <see langword="throw"/> any <see cref="Exception"/> inside <see cref="IDoHttpHandshake.MakeOrThrow(Microsoft.AspNetCore.Http.HttpContext, out List{System.Security.Claims.Claim}, out string)"/> the connection will be <see langword="rejected"/> </para>
    /// </summary>
    /// <typeparam name="THandShaker"></typeparam>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    public IBuildHandShake UseSalkyHttpHandShakerMiddleWare<THandShaker>() where THandShaker : class, IDoHttpHandshake
    {
        serviceCollection.AddScoped<IDoHttpHandshake, THandShaker>();
        return this;
    }
    public IBuildHandShake UseSalkyWebSocktHandShakerMiddleWare<THandShaker>() where THandShaker : class, IDoWebSocketHandShake
    {
        serviceCollection.AddScoped<THandShaker, THandShaker>();
        return this;
    }
}

public static class RoutingExtensions
{
    public static IBuildHandShake UseSalkyWebSocketRouter(this IServiceCollection serviceCollection)
    {
        foreach (var @class in RouteResolver.RouteClassTypes)
            serviceCollection.AddScoped(@class);
        serviceCollection
            .AddTransient<IRouterResolver, RouteResolver>()
            .AddSingleton<IConnectionManager, ConnectionMannager>();
        return new Builder(serviceCollection);
    }

}
