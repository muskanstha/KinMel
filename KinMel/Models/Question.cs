using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinMel.Models
{
    public class Question
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public int ClassifiedAdId { get; set; }
        public virtual ClassifiedAd ClassifiedAd { get; set; }

        public string CreatedById { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }

        public DateTimeOffset DateCreated { get; set; }
    }
}
