using Passion_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Passion_Project.Controllers
{
    public class BookingDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves a list of all bookings.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: All bookings in the database, including the associated user and 
        /// information.
        /// </returns>
        // GET: api/BookingData/Listbookings
        [HttpGet]
        [Route("api/BookingData/ListBookings")]
        public IEnumerable<BookingDto> ListBookings()
        {
            List<Bookings> Bookings = db.Bookings.ToList();
            List<BookingDto> BookingDtos = new List<BookingDto>();

            Bookings.ForEach(r => BookingDtos.Add(new BookingDto()
            {
                BookingID = r.BookingID,
                UserID = r.Users.UserID,
                Username = r.Users.Username,
                ClassID = r.Classes.ClassID,
                ClassName = r.Classes.ClassName,
                BookingDate = r.BookingDate,
                ClassDate = r.ClassDate,
                Status = r.Status
            }));

            return BookingDtos;
        }

        /// <summary>
        /// Retrieves information about a booking by its ID.
        /// </summary>
        /// <param name="bookingId">The ID of the booking to retrieve.</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: The booking information, including the associated user and class details.
        /// </returns>
        // GET: api/BookingData/FindBooking/3
        [ResponseType(typeof(Bookings))]
        [HttpGet]
        [Route("api/BookingData/FindBooking/{bookingId}")]
        public IHttpActionResult FindBooking(int bookingId)
        {
            Bookings res = db.Bookings.Find(bookingId);
            BookingDto bookingDto = new BookingDto()
            {
                BookingID = res.BookingID,
                UserID = res.Users.UserID,
                Username = res.Users.Username,
                ClassID = res.Classes.ClassID,
                ClassName = res.Classes.ClassName,
                BookingDate = res.BookingDate,
                ClassDate = res.ClassDate,
                Status = res.Status
            };
            if (bookingDto == null)
            {
                return NotFound(); // HTTP Status COde 404
            }
            return Ok(bookingDto); // return booking Object
        }

        /// <summary>
        /// Adds a new booking to the system.
        /// </summary>
        /// <param name="booking">The booking object containing the information to add.</param>
        /// <returns>
        /// HEADER: 200 (OK) if the booking is added successfully.
        /// </returns>
        // POST: api/BookingData/AddBooking
        [ResponseType(typeof(Bookings))]
        [HttpPost]
        [Route("api/BookingData/AddBooking")]
        public IHttpActionResult AddBooking(Bookings booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Bookings.Add(booking);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Updates the information of an existing booking in the system.
        /// </summary>
        /// <param name="id">The ID of the booking to update.</param>
        /// <param name="booking">The updated booking object with new information.</param>
        /// <returns>
        /// HEADER: 204 (No Content) if the booking is updated successfully.
        /// </returns>
        // POST: api/bookingData/Updatebooking/3
        [ResponseType(typeof(Bookings))]
        [HttpPost]
        [Route("api/BookingData/UpdateBooking/{id}")]
        public IHttpActionResult UpdateBooking(int id, Bookings booking)
        {
            Debug.WriteLine("Is it reached to Update booking Method!!!");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("State is not Valid For booking Model!!!");
                return BadRequest(ModelState);
            }
            if (id != booking.BookingID)
            {
                Debug.WriteLine("booking ID is not Match!!");
            }

            db.Entry(booking).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!BookingExists(id))
                {
                    Debug.WriteLine("booking details not found!!");
                    return NotFound();
                }
                else
                {
                    Debug.WriteLine(ex);
                    throw;
                }
            }
            Debug.WriteLine("Nothing is trigger in update booking function!!");
            return (StatusCode(HttpStatusCode.NoContent));
        }

        /// <summary>
        /// Deletes a booking from the system by its ID.
        /// </summary>
        /// <param name="id">The ID of the booking to delete.</param>
        /// <returns>
        /// HEADER: 200 (OK) if the booking is deleted successfully.
        /// HEADER: 404 (Not Found) if the booking with the given ID is not found.
        /// </returns>
        // POST: api/bookingData/Deletebooking/3
        [ResponseType(typeof(Bookings))]
        [HttpPost]
        [Route("api/BookingData/DeleteBooking/{id}")]
        public IHttpActionResult DeleteBooking(int id)
        {
            Bookings res = db.Bookings.Find(id);
            if (res == null)
            {
                return NotFound();
            }
            db.Bookings.Remove(res);
            db.SaveChanges();

            return Ok();
        }

        // Check Is Booking(booking) Already Exists and Return Boolean Value Either True Or False
        private bool BookingExists(int id)
        {
            return db.Bookings.Count(e => e.BookingID == id) > 0;
        }
    }
}