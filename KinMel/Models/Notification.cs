using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KinMel.Models
{
    public class Notification
    {
        public int Id { get; set; }

        // notification sent to
        [ForeignKey("NotificationTo")]
        public string NotificationToId { get; set; }

        public virtual ApplicationUser NotificationTo { get; set; }


        [ForeignKey("NotificationFrom")]
        public string NotificationFromId { get; set; }

        // notification from

        public virtual ApplicationUser NotificationFrom { get; set; }

        public string NotificationText { get; set; }

        // notification action binding
        public string ActionController { get; set; }
        public string Action { get; set; }
        public string ActionId { get; set; }

        public bool IsRead { get; set; }

        // notification date
        public DateTimeOffset Date { get; set; }

    }
}
