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
        {
            var friends = await friendRepo.GetAllAprovedOrPending(UserId, true);
            return map(friends, UserId);
        }

        public async Task<List<UserFriendDto>> GetAllAccepted(Guid UserId)
        {
            var friends = await friendRepo.GetAllAproved(UserId, true);
            return map(friends, UserId);
        }

        public async Task<UserFriendDto?> SendFriendRequest(Guid UserId, Guid OtherUserId)
        {
            var frieds = await friendRepo.GetByUsersId(UserId, OtherUserId);
            if (frieds != null)
            {
                return await ReturnFriendToPedding(UserId, frieds);
            }
            if (!useRepo.Exist(OtherUserId)) return null;
            var friend = Friend.CreateFriend(UserId, OtherUserId);
            friendRepo.Add(friend);
            await friendRepo.EnsureSaveChangesAsync();
            friend = await friendRepo.GetById(UserId, friend.Id, true) ?? throw new NullReferenceException();
            return map(friend, UserId);
        }

        private async Task<UserFriendDto?> ReturnFriendToPedding(Guid UserId, Friend friend)
        {
            if (!friend.TryReturnToPending(UserId)) return null;
            friendRepo.Update(friend);
            await friendRepo.EnsureSaveChangesAsync();
            return map(await friendRepo.GetById(UserId, friend.Id, true) ?? throw new NullReferenceException(), UserId);
        }

        public async Task<UserFriendDto?> ChangeToRemoved(Guid UserId, Guid FriendId)
        {
            var friends = await friendRepo.GetById(UserId, FriendId, true);
            if (friends == null) return null;
            if (!friends.TryChangeToRemoved(UserId)) return null;
            friendRepo.Update(friends);
            await friendRepo.EnsureSaveChangesAsync();
            return map(friends, UserId);
        }

        public async Task<UserFriendDto?> GetById(Guid UserId, Guid FriendId)
        {
            var friend = await friendRepo.GetById(UserId, FriendId, true);
            if (friend == null) return null;
            if (friend.RequestedById != UserId && friend.RequestedToId != UserId) return null;
            return map(friend, UserId);
        }

        public async Task<UserFriendDto?> AcceptFriend(Guid UserId, Guid FriendId)
        {
            var friend = await friendRepo.GetById(UserId, FriendId, true);
            if (friend == null) return null;
            if (!friend.TryAcceptFriendRequest(UserId)) return null;
            friendRepo.Update(friend);
            await friendRepo.EnsureSaveChangesAsync();
            return map(friend, UserId);
        }

        public bool BlockFriend(Guid UserId, Guid FriendId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="FriendId"></param>
        /// <returns><see langword="true"/> se removido se não <see langword="false"/></returns>
        public async Task<bool> RejectFriend(Guid UserId, Guid FriendId)
        {
            var friend = await friendRepo.GetById(UserId, FriendId, true);
            if (friend == null || !friend.RejectFriendRequest(UserId))
                return false;
            friendRepo.Update(friend);
            return await friendRepo.EnsureSaveChangesAsync() > 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="FriendId"></param>
        /// <returns><see langword="true"/> se removido se não <see langword="false"/></returns>
        public async Task<bool> CancelFriendRequest(Guid UserId, Guid FriendId)
        {
            var friend = await friendRepo.GetById(UserId, FriendId, true);
            if (friend == null || !friend.CancelFriendRequest(UserId))
                return false;
            friendRepo.Update(friend);
            return await friendRepo.EnsureSaveChangesAsync() > 0;
        }

        public async Task<bool> IsOneOfTheFriends(Guid UserId, Guid FriendId)
        {
            return await friendRepo.IsOneOfTheFriends(UserId, FriendId);
        }

        private UserFriendDto map(Friend friend, Guid UserId)
        {
            var other = friend.GetUserOfFriendDiferentOf(UserId);
            return new UserFriendDto()
            {
                FriendFlag = friend.FriendRequestFlag,
                PictureSource = other.PictureSource,
                RequestByCurrentUser = friend.RequestedById == UserId,
                Id = friend.Id,
                UserName = other.UserName,
                UserId = other.Id,
            };
        }
        private List<UserFriendDto> map(List<Friend> friends, Guid UserId)
        {
            return friends.Select(x => map(x, UserId)).ToList();

        }

    }
}
