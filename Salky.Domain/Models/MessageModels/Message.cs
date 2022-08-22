using Salky.Domain.Models.GenericsModels;
using Salky.Domain.Models.MessageModels;
using Salky.Domain.Models.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.Domain
{
    namespace Salky.Domain
    {
        public class Message : BaseEntity
        {
            public Message()
            {

            }
            public Message(Guid SenderId, string content)
            {
                this.SenderId = SenderId;
                this.Content = content;
            }
            public Guid SenderId { get; set; }
            public User? Sender { get; set; }
            //
            public string Content { get; set; }
            public MessageStatus Status { get; set; }
            public bool HeIsTheSender(Guid userId)
            {
                return this.SenderId == userId;
            }
        }
    }
}
