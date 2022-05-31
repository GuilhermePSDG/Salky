using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.WebSocket.Infra.Interfaces
{
    public interface IDoHttpHandshake
    {
        public void MakeOrThrow(HttpContext httpContext);
    }
}
