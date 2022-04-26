using Salky.WebSocket.Infra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.WebSocket.Infra.Routing.Atributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class WsRedirect : RouteMethodAtribute
    {
        public WsRedirect() : this("") { }
        public WsRedirect(string routePath) : base(routePath, Method.REDIRECT) { }
    }
}
