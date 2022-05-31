using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.WebSocket.Infra.Models
{
    public struct RoutePath
    {
        public string Path { get;set; }
        public Method Method { get;set; }
        public RoutePath(string Path, Method method)
        {
            (this.Path,this.Method) = (Path,method);
        }
        public override string ToString()
        {
            return $"{Path}{Method}";
        }
        public string ToLower()
        {
            return this.ToString().ToLower();
        }

    }
}
