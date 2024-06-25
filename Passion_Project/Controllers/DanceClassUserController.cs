using Newtonsoft.Json;
using Passion_Project.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;

namespace Passion_Project.Controllers
{
    public class DanceClassUserController : Controller
    {
        private static readonly HttpClient client;

        static DanceClassUserController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44301/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: DanceClassUser/UserList
        public ActionResult UserList()
        {
            string url = "danceclassuserdata/listusers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<UserDto> users = response.Content.ReadAsAsync<IEnumerable<UserDto>>().Result;
                return View(users);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: DanceClassUser/ViewUser/2
        public ActionResult ViewUser(int id)
        {
            string url = $"danceclassuserdata/finduser/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                UserDto selectedUser = response.Content.ReadAsAsync<UserDto>().Result;
                return View(selectedUser);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return HttpNotFound();
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: DanceClassUser/AddUser
        public ActionResult AddUser()
        {
            return View();
        }

        // POST: DanceClassUser/CreateUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(User user)
        {
            string url = "danceclassuserdata/adduser";
            string jsonPayload = JsonConvert.SerializeObject(user);

            HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("UserList");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: DanceClassUser/EditUser/2
        public ActionResult EditUser(int id)
        {
            string url = $"DanceClassUserData/FindUser/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                UserDto user = response.Content.ReadAsAsync<UserDto>().Result;
                return View(user);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return HttpNotFound();
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: DanceClassUser/EditUser/2
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(int id, UserDto user)
        {
            if (!ModelState.IsValid)
            {
                return View(user); // Return to edit view with errors
            }

            string url = $"DanceClassUserData/UpdateUser/{id}";
            string jsonPayload = JsonConvert.SerializeObject(user);
            HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("UserList");
            }
            else
            {
                // Handle HTTP errors appropriately
                return RedirectToAction("Error");
            }
        }

        public ActionResult Error()
        {
            return View();
        }


        // GET: DanceClassUser/DeleteUser/5
        public ActionResult DeleteUser(int id)
        {
            HttpResponseMessage response = client.GetAsync($"DanceClassUserData/FindUser/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                UserDto user = response.Content.ReadAsAsync<UserDto>().Result;
                return View(user);
            }
            return HttpNotFound();
        }

        // POST: DanceClassUser/DeleteUserConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserConfirmed(int id)
        {
            HttpResponseMessage response = client.DeleteAsync($"DanceClassUserData/DeleteUser/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("UserList");
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }



    }
}
