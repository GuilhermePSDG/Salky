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
        public void MakeOrThrow(SalkyWebSocket httpContext);
    }
}
