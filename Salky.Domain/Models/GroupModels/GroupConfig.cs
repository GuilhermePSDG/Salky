using Salky.Domain.Models.GenericsModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace Salky.Domain.Models.GroupModels
{
    public class GroupConfig : BaseEntity
    {
        public GroupConfig(int maxUser)
        {
            MaxUser = maxUser;
        }
        public GroupConfig()
        {

        }

        public static GroupConfig Create(int UsersLimit = int.MaxValue)
        {
            return new GroupConfig(UsersLimit);
        }

        public void ChangeUserLimit(int newQuantity)
        {
            MaxUser = newQuantity;
        }
        public int MaxUser { get; private set; }


    }
}
