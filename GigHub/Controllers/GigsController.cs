using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private ApplicationDbContext _context;
        public GigsController()
        {
            _context = new ApplicationDbContext();
        }
        [HttpPost]
        public ActionResult Search(GigsListingViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new { query = viewModel.SearchTerm });
        }
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigsViewModel {
            Genres = _context.Genres.ToList(),
            Heading = "Add a gig"
            };

            return View("GigForm", viewModel);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }
               
            
            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue=viewModel.Venue
            };
            _context.Gigs.Add(gig);
            _context.SaveChanges();
            return RedirectToAction("MyUpcomingGigs", "Gigs");
        }
        public ActionResult Details(int id)
        {
            var gig = _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .SingleOrDefault(g => g.Id == id);
            if (gig == null)
                return HttpNotFound();
            var viewModel = new GigDetailsViewModel
            {
                Gig =gig
            };
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                viewModel.IsAttending = _context.Attendences.Any(a => a.GigId == gig.Id && a.AttendeeId == userId);
                viewModel.IsFollowong = _context.Followings.Any(f => f.FolloweeId == gig.ArtistId && f.FollowerId == userId);
            }
            
            
            return View("Details",viewModel);
        }
        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _context.Attendences
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre).ToList();
            var model = new GigsListingViewModel
            {
                UpComingGigs = gigs,
                ShowActions = User.Identity.IsAuthenticated
            };
            return View(model);
        }
        [Authorize]
        public ActionResult FollowingArtist()
        {
            var userId = User.Identity.GetUserId();
            var followings = _context.Followings.Where(f => f.FollowerId == userId).Select(f => f.Followee).Include(f=>f.Followers).ToList();

            GigsListingViewModel model = new GigsListingViewModel();
            model.Followers = followings;
            return View(model);
        }
        [Authorize]
        public ActionResult MyUpcomingGigs()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _context.Gigs
                .Where(g => g.ArtistId == userId && g.DateTime > DateTime.Now && !g.IsCanceled)
                .Include(g => g.Genre)
                .ToList();
            return View(gigs);
        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs.Single(g => g.Id == id && g.ArtistId == userId);
            var viewModel = new GigsViewModel
            {
                Heading = "Edit a gig",
                Id = gig.Id,
                Genres = _context.Genres.ToList(),
                Venue =gig.Venue,
                Date=gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Genre = gig.GenreId
                
            };

            return View("GigForm",viewModel);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }
            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs
                .Include(g=>g.Attendences.Select(a=>a.Attendee))
                .Single(g => g.Id == viewModel.Id && g.ArtistId == userId);

            gig.Modify(viewModel.GetDateTime(), viewModel.Venue, viewModel.Genre);
            
            _context.SaveChanges();
            return RedirectToAction("MyUpcomingGigs", "Gigs");
        }
    }
}