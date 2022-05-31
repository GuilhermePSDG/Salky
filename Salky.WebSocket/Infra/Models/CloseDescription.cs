using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.WebSocket.Infra.Models
{
    public enum CloseDescription
    {
        DuplicatedConnection,
        Normal,
        HandShakeProblems,
        Unknow
    }
}
