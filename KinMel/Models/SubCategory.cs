using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinMel.Models
{
    public class SubCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
