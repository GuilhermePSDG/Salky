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
        /// <summary>
        /// Is who start the relationship
        /// </summary>
        [Column(Order = 0)]
        public Guid RequestedById { get; private set; }
        /// <summary>
        /// Is who will receive the request of the relationship
        /// </summary>
        [Column(Order = 1)]
        public Guid RequestedToId { get; private set; }
        /// <summary>
        /// Is who start the relationship
        /// </summary>
        public virtual User RequestedBy { get; private set; }
        /// <summary>
        /// Is who will receive the request of the relationship
        /// </summary>
        public virtual User RequestedTo { get; private set; }

        public List<FriendMessage> Messages { get; private set; }
        
        public DateTime? RequestTime { get; private set; }

        public DateTime? BecameFriendsTime { get; private set; }

        public RelationShipStatus FriendRequestFlag { get; private set; }

        [NotMapped]
        public bool Approved => FriendRequestFlag == RelationShipStatus.Approved;

        public static Friend CreateFriend(Guid userId, Guid friendUserId)
        {
            return new Friend()
            {
                RequestedById = userId,
                RequestedToId = friendUserId,
                RequestTime = DateTime.UtcNow,
                FriendRequestFlag = RelationShipStatus.Pending
            };
        }

        /// <summary>
        /// </summary>s
        /// <returns> <see langword="true"/> when can accept, otherwise <see langword="false"/></returns>
        public bool TryAcceptFriendRequest(Guid whoWantAccept)
        {
            if(
                FriendRequestFlag == RelationShipStatus.Pending &&
                RequestedToId == whoWantAccept
                )
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

        public bool CanInteractBetween() => FriendRequestFlag == RelationShipStatus.Approved;

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns> <see langword="true"/> when can reject, otherwise <see langword="false"/></returns>
        private bool CanRejectFriendRequest(Guid whoWantReject)
        {
            return
                FriendRequestFlag == RelationShipStatus.Pending &&
                RequestedToId == whoWantReject;
        }
        public bool RejectFriendRequest(Guid whoWantReject)
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

        public bool TryReturnToPending(Guid userId)
        {
            if (FriendRequestFlag != RelationShipStatus.Removed && FriendRequestFlag != RelationShipStatus.Canceled && FriendRequestFlag != RelationShipStatus.Rejected) return false;
            if(this.RequestedById != userId)
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


        public bool TryChangeToRemoved(Guid userId)
        {
            if(this.FriendRequestFlag != RelationShipStatus.Approved) return false;
            if (RequestedById != userId && RequestedToId != userId) return false;
            this.FriendRequestFlag = RelationShipStatus.Removed;
            RequestTime = null;
            BecameFriendsTime = null;
            return true;
        }

        public bool CancelFriendRequest(Guid whoWantCancel)
        {
            if (CanCancelFriendRequest(whoWantCancel))
            {
                this.FriendRequestFlag = RelationShipStatus.Canceled;
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool CanCancelFriendRequest(Guid whoWantCancel)
        {
            return
                FriendRequestFlag == RelationShipStatus.Pending &&
                RequestedById == whoWantCancel;
        }
        public bool TryBlockFriend(Guid whoWantBlock)
        {
            throw new NotImplementedException();
        }

       
    }
}
