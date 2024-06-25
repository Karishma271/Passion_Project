using Passion_Project.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Passion_Project.Controllers
{
    public class DanceClassController : Controller
    {
        private static readonly HttpClient client;
        private readonly JavaScriptSerializer jss = new JavaScriptSerializer();

        // Initialize the HttpClient with base address and headers
        static DanceClassController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44301/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: DanceClass/List
        public async Task<ActionResult> List()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("DanceClassData/ListClasses");
                response.EnsureSuccessStatusCode(); // Throw on error code

                IEnumerable<ClassDto> classes = await response.Content.ReadAsAsync<IEnumerable<ClassDto>>();
                return View(classes);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching classes: {ex.Message}");
                return RedirectToAction("Error");
            }
        }

        // GET: DanceClass/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"DanceClassData/FindClass/{id}");
                response.EnsureSuccessStatusCode(); // Throw on error code

                ClassDto selectClass = await response.Content.ReadAsAsync<ClassDto>();
                return View(selectClass);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching class details: {ex.Message}");
                return RedirectToAction("Error");
            }
        }

        // GET: DanceClass/Add
        public async Task<ActionResult> Add()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("studiodata/liststudios");
                response.EnsureSuccessStatusCode(); // Throw on error code

                IEnumerable<StudioDto> studios = await response.Content.ReadAsAsync<IEnumerable<StudioDto>>();
                return View(studios);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching studios: {ex.Message}");
                return RedirectToAction("Error");
            }
        }

        // POST: DanceClass/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ClassDto newClass)
        {
            try
            {
                string url = "DanceClassData/AddClass";
                string jsonpayload = jss.Serialize(newClass);

                HttpContent content = new StringContent(jsonpayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode(); // Throw on error code

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating class: {ex.Message}");
                return RedirectToAction("Error");
            }
        }

        // GET: DanceClass/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"DanceClassData/FindClass/{id}");
                response.EnsureSuccessStatusCode(); // Throw on error code

                ClassDto selectClass = await response.Content.ReadAsAsync<ClassDto>();

                HttpResponseMessage studioResponse = await client.GetAsync("StudioData/ListStudios");
                studioResponse.EnsureSuccessStatusCode(); // Throw on error code

                IEnumerable<StudioDto> studios = await studioResponse.Content.ReadAsAsync<IEnumerable<StudioDto>>();

                ViewBag.Studios = studios; // Pass studios to the view
                return View(selectClass);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching class for edit: {ex.Message}");
                return RedirectToAction("Error");
            }
        }

        // POST: DanceClass/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(int id, ClassDto updatedClass)
        {
            try
            {
                string url = $"DanceClassData/UpdateClass/{id}";
                string jsonpayload = jss.Serialize(updatedClass);

                HttpContent content = new StringContent(jsonpayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode(); // Throw on error code

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating class: {ex.Message}");
                return RedirectToAction("Error");
            }
        }

        // GET: DanceClass/Delete/5
        public async Task<ActionResult> DeleteClass(int id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"DanceClassData/FindClass/{id}");
                response.EnsureSuccessStatusCode(); // Throw on error code

                ClassDto selectClass = await response.Content.ReadAsAsync<ClassDto>();
                return View(selectClass);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching class for deletion: {ex.Message}");
                return RedirectToAction("Error");
            }
        }

        // POST: DanceClass/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                HttpResponseMessage response = await client.PostAsync($"DanceClassData/DeleteClass/{id}", null);
                response.EnsureSuccessStatusCode(); // Throw on error code

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting class: {ex.Message}");
                return RedirectToAction("Error");
            }
        }

        // GET: DanceClass/Error
        public ActionResult Error()
        {
            return View();
        }
    }
}
