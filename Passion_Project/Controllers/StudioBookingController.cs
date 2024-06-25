using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Passion_Project.Models;
using Passion_Project.Models.ViewModels;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Passion_Project.Controllers
{
    public class StudioBookingController : Controller
    {
        private static readonly HttpClient client;

        static StudioBookingController()
        {
            // Initialize HttpClient with base URL for API requests
            client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44301/api/") // Adjust base URL to match your API address
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: StudioBooking/BookingList
        public ActionResult BookingList()
        {
            try
            {
                HttpResponseMessage response = client.GetAsync("BookingData/ListBookings").Result;

                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<BookingDto> bookings = response.Content.ReadAsAsync<IEnumerable<BookingDto>>().Result;
                    return View(bookings);
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to retrieve bookings from the API.";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return View("Error");
            }
        }

        // GET: StudioBooking/ViewBooking/5
        public ActionResult ViewBooking(int id)
        {
            try
            {
                HttpResponseMessage response = client.GetAsync($"BookingData/FindBooking/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    BookingDto selectedBooking = response.Content.ReadAsAsync<BookingDto>().Result;
                    return View(selectedBooking);
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to retrieve booking details. Please try again.";
                    ViewBag.ErrorStackTrace = response.Content.ReadAsStringAsync().Result; // Example: Pass detailed error info
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                ViewBag.ErrorStackTrace = ex.StackTrace; // Example: Pass detailed error info
                return View("Error");
            }
        }

        // GET: StudioBooking/AddBooking
        public ActionResult AddBooking()
        {
            try
            {
                var viewModel = new AddBookingViewModel
                {
                    Classes = GetClassesFromApi(),
                    Users = GetUsersFromApi()
                };

                if (viewModel.Classes == null || viewModel.Users == null)
                {
                    return View("Error");
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBooking(AddBookingViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Reload dropdown lists if needed
                    viewModel.Classes = GetClassesFromApi();
                    viewModel.Users = GetUsersFromApi();
                    return View(viewModel);
                }

                // Prepare the Booking object to be sent to the API
                Booking booking = new Booking
                {
                    ClassID = viewModel.ClassID,
                    UserID = viewModel.UserID,
                    BookingDate = viewModel.BookingDate,
                    ClassDate = viewModel.ClassDate,
                    Status = viewModel.Status
                };

                // Call the API to add the booking
                HttpResponseMessage response = client.PostAsJsonAsync("BookingData/AddBooking", booking).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Redirect to the booking list upon success
                    return RedirectToAction("BookingList");
                }
                else
                {
                    // Handle API error response
                    ViewBag.ErrorMessage = "Failed to create booking. Please try again later.";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return View("Error");
            }
        }
        // GET: StudioBooking/EditBooking/5
        public ActionResult EditBooking(int id)
        {
            HttpResponseMessage response = client.GetAsync($"BookingData/FindBooking/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                BookingDto booking = response.Content.ReadAsAsync<BookingDto>().Result;
                return View(booking);
            }
            return HttpNotFound();
        }

        // POST: StudioBooking/EditBooking/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBooking(BookingDto bookingDto)
        {
            if (!ModelState.IsValid)
            {
                return View(bookingDto);
            }

            HttpResponseMessage response = client.PutAsJsonAsync($"BookingData/EditBooking/{bookingDto.BookingID}", bookingDto).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("BookingList");
            }

            ModelState.AddModelError("", "Failed to update booking.");
            return View(bookingDto);
        }
        // GET: StudioBooking/DeleteBooking/5
        public ActionResult DeleteBooking(int id)
        {
            HttpResponseMessage response = client.GetAsync($"BookingData/FindBooking/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                BookingDto booking = response.Content.ReadAsAsync<BookingDto>().Result;
                return View(booking);
            }
            return HttpNotFound();
        }

        // POST: StudioBooking/DeleteBookingConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBookingConfirmed(int id)
        {
            HttpResponseMessage response = client.DeleteAsync($"BookingData/DeleteBooking/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("BookingList");
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        // Helper methods to fetch data from API
        private IEnumerable<ClassDto> GetClassesFromApi()
        {
            try
            {
                HttpResponseMessage response = client.GetAsync("DanceClassData/ListClasses").Result;

                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<IEnumerable<ClassDto>>().Result;
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to retrieve classes from the API.";
                    return null;
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return null;
            }
        }

        private IEnumerable<UserDto> GetUsersFromApi()
        {
            try
            {
                HttpResponseMessage response = client.GetAsync("DanceClassUserData/ListUsers").Result;

                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<IEnumerable<UserDto>>().Result;
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to retrieve users from the API.";
                    return null;
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return null;
            }
        }

        // Error handling view
        public ActionResult Error()
        {
            return View();
        }
    }
}

