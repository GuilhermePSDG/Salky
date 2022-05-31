using Salky.Domain.Salky.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Salky.Domain.Models.GroupModels
{
    public class MessageGroup : Message
    {
        public MessageGroup()
        {

        }
        public MessageGroup(Guid GroupId,Guid SenderId,string Content):base(SenderId,Content)
        {
            this.GroupId = GroupId;
            this.Id = Guid.NewGuid();
        }
        public Guid GroupId { get; set; }
        public Group? Group { get; set; }
    }
}
