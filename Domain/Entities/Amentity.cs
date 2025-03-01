using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Amentity : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public List<BeachAmentity> BeachAmentity { get; set; } = new List<BeachAmentity>();
    }
}