namespace Salky.Domain.Models.GenericsModels
{
    public class Notification : BaseEntity
    {
        public NotificationType Type { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}