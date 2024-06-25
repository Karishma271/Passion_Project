using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Passion_Project.Models;

namespace Passion_Project.Controllers
{
    public class BookingDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/BookingData/ListBookings
        [HttpGet]
        [Route("api/BookingData/ListBookings")]
        public IEnumerable<BookingDto> ListBookings()
        {
            var bookings = db.Bookings.Include(b => b.User).Include(b => b.Class).ToList();
            var bookingDtos = bookings.Select(b => new BookingDto()
            {
                BookingID = b.BookingID,
                UserID = b.User.UserID,
                Username = b.User.Username,
                ClassID = b.Class.ClassID,
                ClassName = b.Class.ClassName,
                BookingDate = b.BookingDate,
                ClassDate = b.ClassDate,
                Status = b.Status
            }).ToList();

            return bookingDtos;
        }

        // POST: api/BookingData/AddBooking
        [HttpPost]
        [Route("api/BookingData/AddBooking")]
        [ResponseType(typeof(Booking))]
        public IHttpActionResult AddBooking(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.Bookings.Add(booking);
                db.SaveChanges();

                return Ok(booking);
            }
            catch (Exception ex)
            {
                // Log the exception
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        [Route("api/BookingData/FindBooking/{id}")]
        [ResponseType(typeof(BookingDto))]
        public IHttpActionResult FindBooking(int id)
        {
            Booking booking = db.Bookings.Include(b => b.User).Include(b => b.Class).FirstOrDefault(b => b.BookingID == id);
            if (booking == null)
            {
                return NotFound();
            }

            BookingDto bookingDto = new BookingDto
            {
                BookingID = booking.BookingID,
                UserID = booking.User.UserID,
                Username = booking.User.Username,
                ClassID = booking.Class.ClassID,
                ClassName = booking.Class.ClassName,
                BookingDate = booking.BookingDate,
                ClassDate = booking.ClassDate,
                Status = booking.Status
            };

            return Ok(bookingDto);
        }
        [HttpPut]
        [Route("api/BookingData/EditBooking/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult EditBooking(int id, BookingDto bookingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bookingDto.BookingID)
            {
                return BadRequest();
            }

            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }

            booking.BookingDate = bookingDto.BookingDate;
            booking.ClassDate = bookingDto.ClassDate;
            booking.Status = bookingDto.Status;

            db.Entry(booking).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        private bool BookingExists(int id)
        {
            return db.Bookings.Any(e => e.BookingID == id);
        }

        // DELETE: api/BookingData/DeleteBooking/5
        [HttpDelete]
        [Route("api/BookingData/DeleteBooking/{id}")]
        [ResponseType(typeof(Booking))]
        public IHttpActionResult DeleteBooking(int id)
        {
            var booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }

            db.Bookings.Remove(booking);
            db.SaveChanges();

            return Ok(booking);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
