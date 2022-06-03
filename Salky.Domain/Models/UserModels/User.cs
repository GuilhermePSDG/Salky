using Salky.Domain.Contracts;
using Salky.Domain.Events.UserEvents;
using Salky.Domain.Models.FriendModels;
using Salky.Domain.Models.GenericsModels;
using Salky.Domain.Models.GroupModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Salky.Domain.Models.UserModels
{

    public class User /*: IdentityUser<Guid>*/
    {
        public static string UserNameRegexPattern = @"^[A-Za-z0-9]{3,32}$";
        public static System.Text.RegularExpressions.Regex UserNameRegex = new (UserNameRegexPattern);
        public User(string UserName,string PassWordHash)
        {
            this.UserName = UserName;
            this.NormalizedUserName = this.UserName.ToUpper();
            this.PassWordHash = PassWordHash;
        }
        public bool IsValid([NotNullWhen(false)] out string? InvalidMessage)
        {
            InvalidMessage = null;
            if (!UserNameIsValid())
            {
                InvalidMessage = "Usuário inválido.";
                return false;
            }
            return true;
        }
        public bool UserNameIsValid()
        {
            return UserNameRegex.IsMatch(UserName);
        }


        [Required(),MinLength(3),MaxLength(32),RegularExpression(@"^[A-Za-z0-9]{3,32}$")]
        public string UserName { get; private set; }
        public string NormalizedUserName { get; private set; }
        public Guid Id { get; private set; }
        public string PictureSource { get; private set; } = "https://e7.pngegg.com/pngimages/262/672/png-clipart-user-profile-aurangabad-computer-icons-great-value-blue-service-thumbnail.png";
        public Visibility Visibility { get; private set; }
        public List<GroupMember> Groups { get; private set; }
        public List<Group> OwnerGroups { get; private set; }
        public List<Notification> Notifications { get; private set; }
        public virtual List<Friend> SentFriendRequests { get; private set; }
        public virtual List<Friend> ReceievedFriendRequests { get; private set; }

        public void ChangePicture(string PictureSource,out UserPictureChanged @event)
        {
            this.PictureSource = PictureSource;
            @event = new UserPictureChanged(this.Id,PictureSource);
        }
        
        public string PassWordHash { get; set; }
    }
}
