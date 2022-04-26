using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.WebSocket.Infra.RoutingExcepetions
{
    public class InvalidRouteException : Exception
    {
        public InvalidRouteException(string message) : base(message)
        {

        }
       
    }
}
