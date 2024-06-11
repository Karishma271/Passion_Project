using Passion_Project.Models;
using Passion_Project.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Passion_Project.Controllers
{
    public class StudioBookingController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static StudioBookingController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44384/api/");
        }

        // GET:
        //
        // booking/BookingList
        public ActionResult BookingList()
        {
            // objective: communicate with our booking data api to retrieve a list of bookings
            // curl https://localhost:44384/api/BookingData/ListBookings

            string url = "BookingData/ListBookings";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<BookingDto> bookings = response.Content.ReadAsAsync<IEnumerable<BookingDto>>().Result;

            return View(bookings);
        }

        // GET: Studiobooking/Viewbooking/2
        public ActionResult Viewbooking(int id)
        {
            // objective: communicate with our booking data api to retrieve one booking
            // curl https://localhost:44384/api/studiodata/findbooking/{id}

            string url = "bookingdata/findbooking/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Debug.WriteLine(response.StatusCode);

            BookingDto selectBooking = response.Content.ReadAsAsync<BookingDto>().Result;
            
            Debug.WriteLine(selectBooking);
            Debug.WriteLine("#####Hello World!!#########");
            return View(selectBooking);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Studiobooking/Addbooking
        public ActionResult AddBooking()
        {
            // objective: communicate with our class data api to retrieve a list of classs
            // curl https://localhost:44384/api/classdata/listclasses

            string url = "classdata/listclasses";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ClassDto> classes = response.Content.ReadAsAsync<IEnumerable<ClassDto>>().Result;

            // objective: communicate with our user data api to retrieve a list of users
            // curl https://localhost:44384/api/userdata/listusers

            string urlUser = "userdata/listusers";
            HttpResponseMessage responseUser = client.GetAsync(urlUser).Result;

            IEnumerable<UserDto> users = responseUser.Content.ReadAsAsync<IEnumerable<UserDto>>().Result;

            var viewModel = new AddBookingViewModel
            {
               Classes = classes,
                Users = users
            };

            return View(viewModel);
        }

        // POST: Studiobooking/CreateBooking
        [HttpPost]
        public ActionResult CreateBooking(Bookings booking)
        {
            string url = "bookingdata/addbooking";
            string jsonpayload = jss.Serialize(booking);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage responseMessage = client.PostAsync(url,content).Result; 
            if(responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("BookingList");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Studiobooking/Editbooking/3
        public ActionResult EditBooking(int id)
        {
            // objective: communicate with our booking data api to retrieve one booking
            // curl https://localhost:44384/api/studiodata/findbooking/{id}

            string url = "bookingdata/findbooking/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            BookingDto selectBooking = response.Content.ReadAsAsync<BookingDto>().Result;

            return View(selectBooking);
        }

        // POST: Studiobooking/Updatebooking/3
        [HttpPost]
        public ActionResult UpdateBooking(int id,Bookings booking)
        {
            try
            {
                Debug.WriteLine("Booking Details Check: ");
                
                Debug.WriteLine(booking.BookingDate);
                Debug.WriteLine(booking.ClassDate);
                Debug.WriteLine(booking.Status);

                // serialize update bookingData into JSON
                // Send the request to the API
                // POST: api/bookingdata/updatebooking/{id}
                // Header : Content-Type: application/json

                string url = "bookingdata/updatebooking/" + id;
                string jsonpayoad = jss.Serialize(booking);
                Debug.WriteLine(jsonpayoad);

                HttpContent content = new StringContent(jsonpayoad);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage responseMessage = client.PostAsync(url, content).Result;
                return RedirectToAction("BookingList");
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return View();
            }
        }

        // GET: Studiobooking/Deletebooking/3
        public ActionResult DeleteBooking(int id)
        {
            // Get Particular booking information

            // objective: communicate with our booking data api to retrieve one booking
            // curl https://localhost:44384/api/studiodata/findbooking/{id}

            string url = "bookingdata/findbooking/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            BookingDto selectBooking = response.Content.ReadAsAsync<BookingDto>().Result;

            return View(selectBooking);
        }

        // POST: Studiobooking/Delete/3
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                string url = "bookingdata/deletebooking/" + id;
                HttpContent content = new StringContent("");

                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage responseMessage = client.PostAsync(url, content).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("BookingList");
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return View();
            }
        }
    }
}