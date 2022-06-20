using Salky.App.Dtos.Users;
using Salky.App.Services;

namespace Salky.App.Dtos.Message
{
    public class MessageDto
    {
        public Guid? Id { get; set; }
        public Guid GroupId { get; set; }
        public string Content { get; set; }
        public DateTime SendedAt { get; set; }
        public List<PartialContent> PartialContents { get; set; }
        public UserMinimalDto Author { get; set; }
    }
}
