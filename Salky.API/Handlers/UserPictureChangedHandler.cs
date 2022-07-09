using Salky.App.Services.Friends;
using Salky.App.Services.Group;
using Salky.Domain.Contracts;
using Salky.Domain.Events.UserEvents;

namespace Salky.API.Handlers
{
    public class UserPictureChangedHandler : IHandler<UserPictureChanged>
    {
        private readonly IConnectionPoolMannager connectionMannager;
        private readonly GroupMemberService groupMemberService;
        private readonly FriendService friendService;
        public UserPictureChangedHandler(
            IConnectionPoolMannager connectionMannager,
            GroupMemberService groupMemberService,
            FriendService friendService
            )
        {
            this.connectionMannager = connectionMannager;
            this.groupMemberService = groupMemberService;
            this.friendService = friendService;
        }

        public async void Handle(UserPictureChanged args)
        {
            (await groupMemberService.GetAllMembersOfUser(args.Id))
                .Select(x => new 
                { 
                    GroupId = x.GroupId.ToString() , 
                    MemberId = x.Id,
                    PictureSource = args.Value,
                }
                )
                .ToList()
                .ForEach(async info =>
                {
                    await connectionMannager.SendToAll(info.GroupId, new("group/member/change/picture", Method.PUT, Status.Success,info));
                });
          
            (await this.friendService.GetAll(args.Id))
                .Select(x => new
                {
                    FriendId = x.Id.ToString(),
                    PictureSource = args.Value,
                })
                .ToList()
                .ForEach(async info =>
                {
                    await connectionMannager.SendToAll(info.FriendId, new("friend/change/picture", Method.PUT,Status.Success, info));
                }); 
            ;
        }
    }
}
