using LiteDB;
using System.ComponentModel.DataAnnotations.Schema;

namespace Salky.Domain.Models.GenericsModels
{
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [BsonId]
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; } = DateTime.UtcNow;
    }
}
