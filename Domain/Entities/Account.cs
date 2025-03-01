using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Domain.Entities
{
    public class Account: BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public List<Review> Reviews { get; set; } = new List<Review>();
    }
}