using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GigHub.Models
{
    public class Notification
    {
        public int Id { get; private set; }
        public DateTime DateTime { get; private set; }
        public NotificationType Type { get; private set; }
        public DateTime? OriginalDatetime { get; set; }
        public string OriginalVenue { get;  set; }

        [Required]
        public Gig Gig { get; private set; }

        public Notification(NotificationType type,Gig gig)
        {
            if (gig == null)
                throw new ArgumentNullException("gig");
            Type = type;
            DateTime = DateTime.Now;
            Gig = gig;
        }
    }
}