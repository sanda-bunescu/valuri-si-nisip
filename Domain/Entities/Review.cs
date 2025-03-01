using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Domain.Entities
{
    public class Review : BaseEntity
    {
        public Guid BeachId { get; set; }
        public Guid AccountId { get; set; }
        [Required]
        public string Description { get; set; }
        public int Grade { get; set; }
        public Beach Beach { get; set; }
        public Account Account { get; set; }
    }
}