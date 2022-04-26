using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.Domain
{
    namespace Salky.Domain
    {
        public class Message : BaseEntity
        {
            public Contato Contato { get; set; }
            public Guid ContatoId { get; set; }
            //
            public Guid? UserSenderId { get; set; }
            public User UserSender { get; set; }
            //
            public Guid? UserReceiverId { get; set; }
            public User UserReceiver { get; set; }
            //
            public MessageStatus Status { get; set; }
            public string Content { get; set; }
        }
    }
}
