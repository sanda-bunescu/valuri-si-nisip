using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Location : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public List<Beach> Beaches { get; set; } = new List<Beach>();
    }
}