using Microsoft.AspNetCore.Builder;
using Salky.WebSocket.Handler;
using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.Socket;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Salky.WebSocket.Extensions;

internal static class Extensions
{
    public static bool IsInternalMethod(this Method method) => (int)method < 0;
}
