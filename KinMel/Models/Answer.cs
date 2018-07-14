using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinMel.Models
{
    public class Answer
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int QuestionId { get; set; }
        public virtual Question Category { get; set; }

        public string CreatedById { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }

        public DateTimeOffset DateCreated { get; set; }
    }
}
