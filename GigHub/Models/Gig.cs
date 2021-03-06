﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GigHub.Models
{
    public class Gig
    {
        public int Id { get; set; }
        public ApplicationUser Artist { get; set; }

        [Required]
        public string ArtistId { get; set; }
        public DateTime DateTime { get; set; }

        [Required]
        [StringLength(255)]
        public string Venue { get; set; }

        
        public Genre Genre { get; set; }

        [Required]
        public byte GenreId { get; set; }

        public bool IsCanceled { get;private set; }

        public ICollection<Attendence> Attendences { get;private set; }
        public Gig()
        {
            Attendences = new Collection<Attendence>();
        }
        public void Cancel()
        {

            IsCanceled = true;

            var notification = Notification.GigCanceled(this);

            foreach (var attendee in Attendences.Select(a => a.Attendee))
            {
                attendee.Notify(notification);

            }
        }
        public void Modify(DateTime dateTime,string venue,byte genre)
        {
            var notification = Notification.GigUpdated(this,DateTime,Venue);
            DateTime = dateTime;
            Venue = venue;
            GenreId = genre;

            foreach (var attendee in Attendences.Select(a=>a.Attendee))
            {
                attendee.Notify(notification);
            }
        }
    }
}