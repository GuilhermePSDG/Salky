using System.Text;
using WebSocket.Handler.Interfaces;

namespace WebSocket.Handler
{
    public class SalkyHtppController
    {
        private IApplicationBuilder app;

        private SalkyHtppController(IApplicationBuilder app)
        {
            this.app = app;
            app.Map("/ping", (appBuilder) =>
            {
                appBuilder.Run(async (ctx) => { await ctx.Response.WriteAsync("pong"); });
            });
            app.Map("/users", (appBuilder) =>
            {
                var connectionManger = appBuilder.ApplicationServices.GetService<IConnectionManager>();
                appBuilder.Run(async (ctx) => { await ctx.Response.WriteAsJsonAsync(connectionManger?.GetAllVisible()); });
            });
            app.Map($"/user" , (appBuilder) =>
            {
                var connectionManger = appBuilder.ApplicationServices.GetService<IConnectionManager>();
                appBuilder.Run(async (ctx) =>
                {
                    Console.WriteLine(ctx.Request.Path);
                    var splited = ctx.Request.Path.Value.Split("/");
                    if (splited.Length > 1 && ctx.Response.Headers["method"] == "GET")
                    {
                        await ctx.Response.WriteAsJsonAsync(connectionManger?.FindByUniqueName(splited[0])?.user);
                    }
                    else
                    {
                        ctx.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                        var buffer = Encoding.UTF8.GetBytes("Invalid request");
                        ctx.Response.Body.Write(buffer);
                        return;
                    }
                });
            });
        }
        
        internal static IApplicationBuilder StartNew(IApplicationBuilder app)
        {
            var builder = new SalkyHtppController(app);
            return builder.app;
        }
    }
}
