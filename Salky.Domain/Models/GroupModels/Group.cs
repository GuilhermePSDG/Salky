using Salky.Domain.Contracts;
using Salky.Domain.Events.GroupEvents;
using Salky.Domain.Models.FriendModels;
using Salky.Domain.Models.GenericsModels;
using Salky.Domain.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Salky.Domain.Models.GroupModels
{
    public class Group : BaseEntity
    {
        private Group()
        {
        }
        public List<GroupMember> Members { get; set; } = new();
        public List<MessageGroup> Messages { get; set; } = new();
        public GroupConfig Config { get; set; }
        public virtual List<GroupRole> GroupRoles { get; set; } = new();
        public List<Transference> TransfersRecords => Transferences;
        private List<Transference> Transferences { get; set; } = new();

        [RegularExpression("^([a-zA-Z0-9 ]{3,24})$",ErrorMessage ="Caracteres Invalidos")]
        public string Name { get;private set; }
        public string PictureSource { get; private set; }
        public User? Owner { get; private set; }
        /// <summary>
        /// <see langword="null"/> when is a group of <see cref="Friend.ChatGroup"/>
        /// </summary>
        public Guid? OwnerId { get; private set; }

        /// <summary>
        /// Make the tranfer of group to another user
        /// </summary>
        /// <param name="WhoWantsTranferTheGroup"></param>
        /// <param name="WhoWillReceiveTheGroup"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public bool TransferGroupToAnotherMember(Guid WhoWantsTranferTheGroup, Guid WhoWillReceiveTheGroup)
        {
            if (!this.Members.Any(x => x.UserId == WhoWillReceiveTheGroup))
                return false;
            if (OwnerId == WhoWantsTranferTheGroup)
            {
                Transferences.Add(new Transference(WhoWantsTranferTheGroup, WhoWillReceiveTheGroup, "Normal transfer"));
                OwnerId = WhoWillReceiveTheGroup;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupMember"></param>
        /// <param name="message"></param>
        /// <returns><see langword="true"/> if can add, otherwise <see langword="false"/> </returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool MemberCanDeleteMessage(GroupMember groupMember, MessageGroup message, [NotNullWhen(true)]out MessageGroupRemoved? @event)
        {
            @event = null;
            //Se não é o mesmo grupo  -> false
            if (groupMember.GroupId != Id || message.GroupId != Id) return false;
            //Se quem fez o envio é quem quer deletar -> true
            //Se o dono do grupo quer remover -> true
            //Se as roles permitem -> true
            if (
                message.SenderId == groupMember.UserId || 
                groupMember.UserId == OwnerId || 
                groupMember.Role.ChatPermissions.CanDeleteOtherUserMessages
                )
            {
                @event = new MessageGroupRemoved(message);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <param name="picturePath"></param>
        /// <returns></returns>
        public bool ChangePicture(GroupMember member, string picturePath, [NotNullWhen(true)] out GroupPictureChanged? @event)
        {
            @event = null;
            if (MemberCanChangeImage(member))
            {
                this.PictureSource = picturePath;
                @event = new GroupPictureChanged(this.Id,this.PictureSource);
                return true;
            }
            return false;
        }

        public bool MemberCanChangeImage(GroupMember member)
        {
            if (member.GroupId != this.Id) return false;
            if (member.UserId == this.OwnerId) return true;
            if (member.Role.GroupPermissions.CanEditGroupPicture) return true;
            return false;
        }

        public bool MemberCanRemoveTheOther(GroupMember MemberWhoWantRemove, GroupMember MemberToRemove, [NotNullWhen(true)] out GroupMemberRemoved? @event)
        {
            @event = null;
            //Se não é o mesmo grupo  -> false
            if (MemberWhoWantRemove.GroupId != this.Id || MemberToRemove.GroupId != this.Id) return false;
            //Se for para remover for o dono -> false
            if(MemberToRemove.UserId == OwnerId) return false;
            //Se quer remover a si mesmo -> true
            //Se quem quer remover for o dono -> true
            //Se a role permite remoção -> true
            if (
                MemberWhoWantRemove.UserId == MemberToRemove.UserId || 
                MemberWhoWantRemove.UserId == this.OwnerId || 
                MemberWhoWantRemove.Role.GroupPermissions.CanRemoveOtherUsers
                )
            {
                @event = new GroupMemberRemoved(MemberToRemove);
                return true;
            }
            //Se não false
            return false;
        }


        /// <summary>
        /// This method do not add <see cref="GroupMember"/> in the <see cref="Members"/> list
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="UserToAddId"></param>
        /// <returns><see langword="true"/> if can add, otherwise <see langword="false"/> </returns>
        /// <exception cref="InvalidOperationException"></exception>
        private bool MemberCanAddOtherMember(Guid UserId,Guid UserToAddId)
        {
            if (Members.Count == 0) throw new InvalidOperationException($"{nameof(Group.Members)} is required for this method");
            if (Members.Any(x => x.UserId == UserToAddId)) return false;
            var groupMember = this.Members.Single(x => x.UserId == UserId);
            if (UserId == OwnerId) return true;
            if (groupMember.Role.GroupPermissions.CanInviteOtherUsers) return true;
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="whoWillAddTheOtherUser"></param>
        /// <param name="userToAddId"></param>
        /// <returns><see cref="Group"/> if can create , otherwise <see langword="null"/> </returns>
        /// <exception cref="InvalidOperationException"></exception>
        public GroupMember? TryCreateNewMember(Guid whoWillAddTheOtherUser,Guid userToAddId,out GroupMemberAdded? @event)
        {
            @event = null;
            if (MemberCanAddOtherMember(whoWillAddTheOtherUser,userToAddId))
            {
                var role = this.GroupRoles.OrderBy(x => x.Hierarchy).Last();
                if (role.Hierarchy <= 1) throw new InvalidOperationException("No such roles");
                var member = GroupMember.Create(this.Id, userToAddId, role);
                this.Members.Add(member);
                role.MemberWithRoles.Add(member);
                @event = new GroupMemberAdded(member);
                return member;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// <paramref name="member"/> who wants remove the group
        /// </summary>
        /// <param name="member"> </param> 
        /// <returns></returns>
        public bool MemberCanDeleteTheGroup(GroupMember member, [NotNullWhen(true)]out GroupDeleted? @event)
        {
            @event = null;
            if (OwnerId == member.UserId)
            {
                @event = new GroupDeleted(this);
                return true;
            }

            return false;
        }

        public static Group Create(Guid creatorId, string GroupName, out GroupCreated @event)
        {

            var DefaultRole = GroupRole.Default();
            var AdminRole = GroupRole.Admin();
            var config = GroupConfig.Create();
            var members = new List<GroupMember>()
            {
                new GroupMember()
                {
                    UserId = creatorId,
                    Role = AdminRole,
                },
            };

            var group =  new Group()
            {
                Config = config,
                GroupRoles = {DefaultRole,AdminRole},
                Members = members,
                OwnerId = creatorId,
                PictureSource = "http://cdn.onlinewebfonts.com/svg/img_329195.png",
                Name = GroupName,
            };
            @event = new GroupCreated(group);
            return group;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="WhoWantChangeTheName"></param>
        /// <param name="newGroupName"></param>
        /// <returns><see langword="true"/> if changed, otherwise <see langword="false"/></returns>
        public bool ChangeName(Guid WhoWantChangeTheName, string newGroupName,[NotNullWhen(true)] out GroupNameChanged? @event)
        {
            @event = null;
            if (WhoWantChangeTheName == this.OwnerId)
            {
                this.Name = newGroupName;
                @event = new GroupNameChanged(this.Id, this.Name);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns><see langword="true"/> if is valid, otherwise <see langword="false"/></returns>
      
    }
}
