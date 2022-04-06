using System;

namespace WebSocket.Shared.DataAcess.Models
{
    public class Message : BaseEntity
    {
        public Message()
        {
            Date = DateTime.Now;
        }
        public bool IsOwner { get; set; }
        public bool IsSended { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }

    }



}
