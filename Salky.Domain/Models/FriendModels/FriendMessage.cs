using Salky.Domain.Salky.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.Domain.Models.FriendModels
{
    public class FriendMessage : Message
    {
        public Friend Friend { get; set; }
        public Guid FriendId { get; set; }

      
    }
}
