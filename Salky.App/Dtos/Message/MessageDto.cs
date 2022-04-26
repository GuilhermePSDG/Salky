using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.App.Dtos.Message
{
    public class MessageDto
    {
        public Guid? Id { get; set; }
        public string Content { get; set; }
        public Guid UserSenderId { get; set; }
        public Guid UserReceiverId { get; set; }
        public DateTime SendedAt { get; set; }
    }
}
