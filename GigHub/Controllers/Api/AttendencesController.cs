﻿using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class AttendencesController : ApiController
    {
        private ApplicationDbContext _context;
        public AttendencesController()
        {
            _context = new ApplicationDbContext();
        }
        [HttpPost]
        public IHttpActionResult Attend(AttendenceDto dto)
        {
            var userId = User.Identity.GetUserId();
            var exists = _context.Attendences.Any(a => a.AttendeeId == userId && a.GigId == dto.GigId);
            if (exists)
                return BadRequest("The attendence already exixsts.");
            var attendence = new Attendence 
            { 
                GigId = dto.GigId,
                AttendeeId = userId 
            };
            _context.Attendences.Add(attendence);
            _context.SaveChanges();
            return Ok();
        }
        [HttpDelete]
        public IHttpActionResult DeleteAttendence(int id)
        {
            var userId = User.Identity.GetUserId();
            var attendence = _context.Attendences.SingleOrDefault(a => a.AttendeeId == userId && a.GigId == id);
            if (attendence == null)
                return NotFound();
            _context.Attendences.Remove(attendence);
            _context.SaveChanges();
            return Ok(id);
        }
    }
}
