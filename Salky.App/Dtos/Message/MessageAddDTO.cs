using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.App.Dtos.Message
{
    public class MessageAddDTO
    {
        public Guid ContactId { get; set; }
        public Guid UserContactId { get; set; }
        public string Content { get; set; }
    }
}
