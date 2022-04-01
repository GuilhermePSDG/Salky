using Microsoft.AspNetCore.Builder;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace WebSocket.Handler.Extensions;

public static class WebServerSocketMiddlewareExtensions
{

    public static IApplicationBuilder UseSalkyWebSocket(this IApplicationBuilder app)
    {
        app.UseWebSockets();
        return app.UseMiddleware<WebServerSocketMiddleware>();
    }
    public static IApplicationBuilder UseWebSocketServerMiddleware(this IApplicationBuilder app, params object[] parameter)
    {
        return app.UseMiddleware<WebServerSocketMiddleware>(parameter);
    }

    public static async Task SendAsync<T>(this System.Net.WebSockets.WebSocket socket, T data)
    {
        await socket.SendAsync(JsonSerializer.Serialize(data));
    }
    public static async Task SendAsync(this System.Net.WebSockets.WebSocket socket, string data)
    {
        var buffer = Encoding.UTF8.GetBytes(data);
        await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
    }


    public static async Task<T?> ReceiveAsJsonAsync<T>(this System.Net.WebSockets.WebSocket socket, byte[] buffer)
    {
        var result = await socket.ReceiveAsStringAsync(buffer);
        return JsonSerializer.Deserialize<T>(result);
    }

    public static async Task<string> ReceiveAsStringAsync(this System.Net.WebSockets.WebSocket webSocket, byte[] buffer)
    {
        var result = await webSocket.ReceiveAsync(buffer);
        return Encoding.UTF8.GetString(buffer, 0, result.Count);
    }
    public static async Task<WebSocketReceiveResult> ReceiveAsync(this System.Net.WebSockets.WebSocket webSocket, byte[] buffer)
    {
        return await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
    }




}