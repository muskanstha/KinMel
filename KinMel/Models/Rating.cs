using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinMel.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public string RatedById { get; set; }
        public ApplicationUser RatedBy { get; set; }

        public string RatedForId { get; set; }
        public ApplicationUser RatedFor { get; set; }

        public int Stars { get; set; }
        public string Review { get; set; }

    }
}
