using Microsoft.AspNetCore.Http;
using Salky.App.Dtos.Users;
using Salky.App.Interfaces;
using Salky.WebSocket.Infra.Interfaces;
using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.Socket;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Salky.WebSocket.Infra.Security
{
    public class SalkySecurity : ISecurityProvider
    {
        private IConnectionManager _connectionManager;
        private readonly ITokenService tokenService;

        public SalkySecurity(IConnectionManager connectionManager,ITokenService tokenService)
        {
            _connectionManager = connectionManager;
            this.tokenService = tokenService;
        }

        public List<Claim> ValidateJwtToken(HttpContext httpContext)
        {
            var token = System.Web.HttpUtility.ParseQueryString(httpContext.Request.QueryString.Value).Get("Authorization");
            if (string.IsNullOrEmpty(token)) throw new NullReferenceException("Authorization token not found");
            var claims = this.tokenService.ValidateToken(token);
            if(claims == null) throw new InvalidTokenException("Token is not valid.");
            return claims;
        }

        public void DisconnectIfOnline(UserDto user)
        {
            _connectionManager.TryRemove(user.Id,out var removeSocket);
            if(removeSocket != null)
                removeSocket.Dispose();
        }




        //public async Task<SalkyWsServer> DoHandShakeOrThrow(System.Net.WebSockets.WebSocket socket)
        //{
        //    var salkySocket = await ForceReicevePublicKey(socket);
        //    salkySocket = await ValidatePublicKey(salkySocket);
        //    return salkySocket;
        //}


        //public async Task<SalkyWebSocket> ForceReicevePublicKey(System.Net.WebSockets.WebSocket socket)
        //{
        //    var MaxTime = TimeSpan.FromSeconds(5);
        //    var msg = await ReceiveSingleAsyncOrThrow(socket, MaxTime);
        //    if (msg == null)
        //    {
        //        await socket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Invalid data", CancellationToken.None);
        //        throw new Exception("Invalid data");
        //    }
        //    if (msg.PathArray[0] != "setuser")
        //    {
        //        await socket.CloseAsync(WebSocketCloseStatus.EndpointUnavailable, "Invalid endpoint", CancellationToken.None);
        //        throw new Exception("Invalid endpoint");
        //    }
        //    var pubkey = msg.Data.ToString();
        //    if (pubkey == null || pubkey.Length < 10)
        //    {
        //        await socket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "The public key cannot be null or invalid", CancellationToken.None);
        //        throw new Exception("The public key cannot be null or invalid");
        //    }
        //    if (_connectionManager.Any(f => f.Storage.TryGetValue("userpublickey", out var publicKey) ? pubkey == publicKey.ToString() : false))
        //    {
        //        await socket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "The User is already online", CancellationToken.None);
        //        throw new Exception("The User is already online");
        //    }
        //    else
        //    {
        //        return new SalkyWebSocket(socket, new()
        //        {

        //        });
        //    }
        //}

        //public async Task<SalkyWsServer> ValidatePublicKey(SalkyWsServer salkyWebSocketServer)
        //{
        //    var MaxTime = TimeSpan.FromSeconds(2);
        //    //CRIA O DADO ALEATORIO
        //    var DataToEncrypt = $"{Guid.NewGuid()}";
        //    //ENCRIPTA O DADO
        //    var DataEncryptedBase64 = RsaService.FromPublicKey(salkyWebSocketServer.user.PublicKey).Encrypt(DataToEncrypt);
        //    //CRIA A MSG COM O DADO
        //    var msg = new MessageServer(salkyWebSocketServer.user.PublicKey, "server", "proveowner", DataEncryptedBase64);
        //    //ENVA A MSG
        //    await salkyWebSocketServer.SendMessageServer(msg);
        //    //Espera pela resposta
        //    var response = await ReceiveSingleAsyncOrThrow(salkyWebSocketServer, MaxTime);
        //    //SE NULL FECHA
        //    if (response == null)
        //    {
        //        await salkyWebSocketServer.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Invalid data", CancellationToken.None);
        //        throw new Exception("Invalid data");
        //    }
        //    else
        //    {
        //        //SE MANDOU NA ROTA ERRADA FECHA
        //        if (response.PathArray[0] != "proveowner")
        //        {
        //            await salkyWebSocketServer.CloseAsync(WebSocketCloseStatus.EndpointUnavailable, "Invalid endpoint", CancellationToken.None);
        //            throw new Exception("Invalid endpoint");
        //        }
        //        //VERIFICA SE É VALIDO, SE NÃO FECHA.
        //        if (response.Data.ToString().Equals(DataToEncrypt))
        //        {
        //            return salkyWebSocketServer;
        //        }
        //        else
        //        {
        //            await salkyWebSocketServer.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Invalid data", CancellationToken.None);
        //            throw new Exception("Invalid data");
        //        }
        //    }

        //}

        private async Task<MessageServer?> ReceiveSingleAsyncOrThrow(System.Net.WebSockets.WebSocket socket, TimeSpan MaxTime)
        {
            var timeCounter = new Stopwatch();
            timeCounter.Start();
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            var buffer = new byte[1024 * 10];
            var receiveTask = socket.ReceiveAsync(buffer, token);
            while (!receiveTask.IsCompleted && timeCounter.Elapsed < MaxTime)
            {
                await Task.Delay(500);
            }
            if (!receiveTask.IsCompleted)
            {
                tokenSource.Cancel();
                await socket.CloseAsync(WebSocketCloseStatus.ProtocolError, "TIME OUT", CancellationToken.None);
                throw new TimeoutException("TIME OUT");
            }
            else
            {
                var result = await receiveTask;
                var fullMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                return JsonSerializer.Deserialize<MessageServer>(fullMessage);
            }
            throw new Exception("Cannot complete the request");
        }

    }
}
