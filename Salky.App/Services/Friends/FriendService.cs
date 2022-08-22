using Salky.App.Dtos;
using Salky.Domain.Contracts;
using Salky.Domain.Models.FriendModels;

namespace Salky.App.Services.Friends
{
    public class FriendService
    {
        private readonly IFriendRepository friendRepo;
        private readonly IUserRepository useRepo;

        public FriendService(
            IFriendRepository friendRepository,
            IUserRepository useRepo
            )
        {
            friendRepo = friendRepository;
            this.useRepo = useRepo;
        }

        public async Task<List<UserFriendDto>> GetAll(Guid UserId)
            => map(await friendRepo.GetAllAprovedOrPending(UserId, true), UserId);

        public async Task<UserFriendDto?> SendFriendRequest(Guid UserId, Guid OtherUserId)
        {
            var frieds = await friendRepo.GetByUsersId(UserId, OtherUserId);
            if (frieds != null) return await TryUpdateFriendRequest(UserId, frieds.Id, RelationShipStatus.Pending);
            //
            if (!useRepo.Exist(OtherUserId)) return null;
            var friend = Friend.CreateFriend(UserId, OtherUserId);
            friendRepo.Add(friend);
            await friendRepo.EnsureSaveChangesAsync();
            friend = await friendRepo.GetById(UserId, friend.Id, true) ?? throw new NullReferenceException();
            return map(friend, UserId);
        }

        public async Task<UserFriendDto?> GetById(Guid UserId, Guid FriendId)
        {
            var friend = await friendRepo.GetById(UserId, FriendId, true);
            if (friend == null) return null;
            if (friend.RequestedById != UserId && friend.RequestedToId != UserId) return null;
            return map(friend, UserId);
        }

        public async Task<UserFriendDto?> TryUpdateFriendRequest(Guid UserId, Guid FriendId, RelationShipStatus TargetStatus)
        {
            var friend = await friendRepo.GetById(UserId, FriendId, true);
            if (friend == null) return null;
            if (!friend.TryUpdateFriendRequestTo(UserId, TargetStatus)) return null;
            friendRepo.Update(friend);
            await friendRepo.EnsureSaveChangesAsync();
            if (friend.RequestedBy == null || friend.RequestedTo == null)
                return await GetById(UserId, FriendId);
            else
                return map(friend, UserId);
        }

        private UserFriendDto map(Friend friend, Guid UserId)
        {
            var other = friend.GetUserOfFriendDiferentOf(UserId);

            return new UserFriendDto()
            {
                FriendFlag = friend.FriendRequestFlag,
                PictureSource = ImageServiceConfiguration.CreateExternalLink(other.PictureSource),
                RequestByCurrentUser = friend.RequestedById == UserId,
                Id = friend.Id,
                UserName = other.UserName,
                UserId = other.Id,
            };
        }
        private List<UserFriendDto> map(List<Friend> friends, Guid UserId)
            => friends.Select(x => map(x, UserId)).ToList();


    }
}
