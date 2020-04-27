﻿using System;
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
        public DateTime? OriginalDatetime { get;private set; }
        public string OriginalVenue { get;private set; }

        [Required]
        public Gig Gig { get; private set; }

        private Notification(NotificationType type,Gig gig)
        {
            if (gig == null)
                throw new ArgumentNullException("gig");
            Type = type;
            DateTime = DateTime.Now;
            Gig = gig;
        }

        public static Notification GigCreated(Gig gig)
        {
            return new Notification(NotificationType.GigCanceled, gig);
        }
        public static Notification GigUpdated(Gig newGig,DateTime originalDateTime,string originalVenue)
        {
            var notification = new Notification(NotificationType.GigUpdated,newGig);
            notification.OriginalDatetime = originalDateTime;
            notification.OriginalVenue = originalVenue;
            return notification;
        }
        public static Notification GigCanceled(Gig gig)
        {
            return new Notification(NotificationType.GigCanceled, gig);
        }
    }
}