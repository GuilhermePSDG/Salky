using System;
using Wpf.Core.Models;

namespace Wpf.MVVM.Models
{
    public class MessageVM : BaseEntityVM
    {
        public MessageVM()
        {
            this.Date = DateTime.Now;
        }
        public string ImageSource { get; set; }
        public string SenderName { get; set; }
        public string UsernameColor = "#4440000";
        public string SenderPublicKey { get; set; }
        public bool IsOwnerMessage { get; set; }
        public bool IsSequencialMessage { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }

}
