using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.WebSocket.Infra.RoutingExceptions
{
    public class InvalidRouteParammeterException : Exception
    {
        public InvalidRouteParammeterException(string message) : base(message)
        {

        }
    }
}
