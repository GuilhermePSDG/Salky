using System;

namespace WebSocket.Shared.DataAcess.Models
{
    public class Message : BaseEntity
    {
        public Message()
        {
            Date = DateTime.Now;
        }
        public Contato Sender { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }

}
