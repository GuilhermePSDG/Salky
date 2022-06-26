using Microsoft.AspNetCore.Http;
using Salky.WebSocket.Infra.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.WebSocket.Infra.Interfaces
{
    public interface IDoWebSocketHandShake
    {
        /// <summary>
        /// Any <see cref="Exception"/> when want reject the connection
        /// </summary>
        /// <param name="httpContext"></param>
        /// <exception cref="Exception"></exception>
        public void MakeOrThrow(SalkyWebSocket httpContext);
    }
}
