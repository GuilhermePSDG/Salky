using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WebSocket.Handler;
using WebSocket.Handler.Extensions;
using WebSocket.Handler.Interfaces;
using Microsoft.AspNetCore.Builder;
namespace WebSocket;


public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IConnectionManager,SalkyWebSocketConnectionManager>();
    }
    public void Configure(IApplicationBuilder app)
    {
        app.UseSalkyWebSocket();
        app.Run(async context => await context.Response.WriteAsync("<h1>hello</h1>"));
    }
}