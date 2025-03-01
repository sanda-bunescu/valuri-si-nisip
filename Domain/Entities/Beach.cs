using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Domain.Entities
{
    public class Beach : BaseEntity
    {
        public Guid LocationId { get; set; }
        [Required]
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Location Location { get; set; }
        public List<Review> Reviews { get; set; } = new List<Review>();
        public List<BeachAmentity> BeachAmentity { get; set; } = new List<BeachAmentity>();
    }
}