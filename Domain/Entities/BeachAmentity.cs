using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BeachAmentity : BaseEntity
    {
        public Guid BeachId { get; set; }
        public Guid AmentityId { get; set; }
        public Beach Beach { get; set; }
        public Amentity Amentity { get; set; }
    }
}