using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KinMel.Models
{
    public class Rating
    {
        public int Id { get; set; }


        public string RatedForId { get; set; }
        public virtual ApplicationUser RatedFor { get; set; }

        public string RatedById { get; set; }

        [Display(Name = "Rated by")]
        public string RateByFirstName { get; set; }

        [Range(1, 5)]
        public int Stars { get; set; }

        [DataType(DataType.MultilineText)]
        public string Review { get; set; }

    }
}
