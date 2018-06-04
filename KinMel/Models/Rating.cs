using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KinMel.Models
{
    public class Rating
    {
        public int Id { get; set; }

        [ForeignKey("RatedFor")]
        public string RatedForId { get; set; }
        public virtual ApplicationUser RatedFor { get; set; }

        [ForeignKey("RatedBy")]
        public string RatedById { get; set; }
        public virtual ApplicationUser RatedBy { get; set; }

        [Range(1, 5)]
        public int Stars { get; set; }

        [DataType(DataType.MultilineText)]
        public string Review { get; set; }

    }
}
