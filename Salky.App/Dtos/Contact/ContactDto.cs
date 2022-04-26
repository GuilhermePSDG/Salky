namespace Salky.App.Dtos.Contact
{
    public class ContactDto
    {
        public Guid ContactId { get; set; }
        public Guid UserContactId { get; set; }
        public string UserName { get; set; }
        public string PictureSource { get; set; }
    }
}
