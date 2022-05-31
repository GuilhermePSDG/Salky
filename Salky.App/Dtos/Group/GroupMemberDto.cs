using Salky.App.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.App.Dtos.Group
{
    public class GroupMemberDto : UserMinimalDto
    {
        public string RoleName { get; set; }
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }
    }
}
