using Salky.Domain.Models.GenericsModels;
using Salky.Domain.Models.GroupModels;
using Salky.Domain.Models.UserModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace Salky.Domain.Models.FriendModels
{
    public class Friend : BaseEntity
    {
        private Friend()
        {

        }
        [Column(Order = 0)]
        public Guid RequestedById { get; private set; }
        [Column(Order = 1)]
        public Guid RequestedToId { get; private set; }
        public virtual User RequestedBy { get; private set; }
        public virtual User RequestedTo { get; private set; }
        public List<FriendMessage> Messages { get; private set; }
        public DateTime? RequestTime { get; private set; }
        public DateTime? BecameFriendsTime { get; private set; }
        public RelationShipStatus FriendRequestFlag { get; private set; }


        public static Friend CreateFriend(Guid userId, Guid OtherUserId)
        {
            if (userId == OtherUserId) throw new InvalidOperationException("User cannot be your own friend");
            return new Friend()
            {
                //Id=Guid.NewGuid(),
                RequestedById = userId,
                RequestedToId = OtherUserId,
                RequestTime = DateTime.UtcNow,
                FriendRequestFlag = RelationShipStatus.Pending
            };
        }

        public User GetUserOfFriendDiferentOf(Guid UserId)
        {
            if(this.RequestedById == UserId)
            {
                return this.RequestedTo;
            }
            if(this.RequestedToId == UserId)
            {
                return RequestedBy;
            }
            throw new InvalidOperationException("User is not one of the friends");
        }
        public bool CanInteractBetween() => FriendRequestFlag == RelationShipStatus.Approved;


        #region FriendFlag Update
        public bool TryUpdateFriendRequestTo(Guid CurrentUserId,RelationShipStatus TargetRelationStatus)
        {
            switch (TargetRelationStatus)
            {
                case RelationShipStatus.Approved:
                    return TryAcceptFriendRequest(CurrentUserId);
                case RelationShipStatus.Removed:
                    return TryChangeToRemoved(CurrentUserId);
                case RelationShipStatus.Rejected:
                    return TryRejectFriendRequest(CurrentUserId);
                case RelationShipStatus.Canceled:
                    return TryCancelFriendRequest(CurrentUserId);
                case RelationShipStatus.Pending:
                    return TryReturnToPending(CurrentUserId);
                default:
                    throw new InvalidOperationException();
            }
        }
        private bool TryAcceptFriendRequest(Guid whoWantAccept)
        {
            if (CanAcceptFriendRequest(whoWantAccept))
            {
                BecameFriendsTime = DateTime.UtcNow;
                FriendRequestFlag = RelationShipStatus.Approved;
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool TryRejectFriendRequest(Guid whoWantReject)
        {
            if (CanRejectFriendRequest(whoWantReject))
            {
                this.FriendRequestFlag = RelationShipStatus.Rejected;
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool TryReturnToPending(Guid userId)
        {
            if (CanReturnToPending(userId))
            {
                if (this.RequestedById != userId)
                {
                    this.RequestedToId = this.RequestedById;
                    this.RequestedById = userId;
                    this.RequestedTo = null;
                    this.RequestedBy = null;
                }
                this.FriendRequestFlag = RelationShipStatus.Pending;
                this.RequestTime = DateTime.UtcNow;
                return true;
            }
            return false;
        }
        private bool TryChangeToRemoved(Guid userId)
        {
            if (CanChangeToRemoved(userId))
            {
                this.FriendRequestFlag = RelationShipStatus.Removed;
                RequestTime = null;
                BecameFriendsTime = null;
                return true;
            }
            return false;
        }
        private bool TryCancelFriendRequest(Guid whoWantCancel)
        {
            if (CanCancelFriendRequest(whoWantCancel))
            {
                this.FriendRequestFlag = RelationShipStatus.Canceled;
                return true;
            }
            return false;
        }

        private bool CanChangeToRemoved(Guid userId) 
            => IsOneOfTheFriends(userId) && this.FriendRequestFlag == RelationShipStatus.Approved;
        private bool CanReturnToPending(Guid WhoWantReturn)
            => IsOneOfTheFriends(WhoWantReturn) && 
            FriendRequestFlag == RelationShipStatus.Removed ||
            FriendRequestFlag == RelationShipStatus.Canceled ||
            FriendRequestFlag == RelationShipStatus.Rejected;
        private bool CanRejectFriendRequest(Guid whoWantReject) 
            => FriendRequestFlag == RelationShipStatus.Pending && RequestedToId == whoWantReject;
        private bool CanAcceptFriendRequest(Guid whoWantAccept)
            => FriendRequestFlag == RelationShipStatus.Pending && RequestedToId == whoWantAccept;
        private bool CanCancelFriendRequest(Guid whoWantCancel) 
            => FriendRequestFlag == RelationShipStatus.Pending && RequestedById == whoWantCancel;
        private bool IsOneOfTheFriends(Guid UserId)
            => UserId == this.RequestedById || UserId == this.RequestedToId;

        #endregion

    }
}
