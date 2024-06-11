using Passion_Project.Models;
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
    public class DanceClassController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DanceClassController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44384/api/");
        }


        // GET:
        // class/List
        public ActionResult List()
        {
            // objective: communicate with our class data api to retrieve a list of classs
            // curl https://localhost:44384/api/classdata/listclasses

            string url = "classdata/listclasses";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ClassDto> Classes = response.Content.ReadAsAsync<IEnumerable<ClassDto>>().Result;

            return View(Classes);
        }


        // Get: Studioclass/Details/2
        public ActionResult Details(int id)
        {
            // objective: communicate with our class data api to retrieve one class
            // curl https://localhost:44384/api/danceclassdata/findclass/{id}

            string url = "classdata/findclass/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            ClassDto selectClass = response.Content.ReadAsAsync<ClassDto>().Result;
            Debug.WriteLine("class data retrived : ");
            Debug.WriteLine(selectClass.ClassName);
            Debug.WriteLine(selectClass.Instructor);
            Debug.WriteLine(selectClass.Schedule);
            Debug.WriteLine(selectClass.Duration);
            Debug.WriteLine(selectClass.Price);
            Debug.WriteLine(selectClass.Status);
            return View(selectClass);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Studioclass/Add
        public ActionResult Add()
        {
            // objective: communicate with our Studio data api to retrieve a list of studios
            // curl https://localhost:44384/api/studiodata/liststudios

            string url = "studiodata/liststudios";

            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<StudioDto> studios = response.Content.ReadAsAsync<IEnumerable<StudioDto>>().Result;

            return View(studios);
        }

        // POST: Studioclass/Create
        [HttpPost]
        public ActionResult Create(Class Class)
        {
            Debug.WriteLine("the class json load is : ");
            // Debug.WriteLine(class.className);

            // objective: add a new class into our system using the API
            // curl -H "Content-Type:application/json" -d @class.json https://localhost:44384/api/danceclassData/add

            string url = "classdata/addclass";
            string jsonpayload = jss.Serialize(Class);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Studioclass/Edit/2
        public ActionResult Edit(int id)
        {
            // Get Particular class information

            // objective: communicate with our class data api to retrieve one class
            // curl https://localhost:44384/api/danceclassdata/findclass/{id}

            string url = "classdata/findclass/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ClassDto selectClass = response.Content.ReadAsAsync<ClassDto>().Result;

            return View(selectClass);
            ;
        }

        // POST: Studioclass/Update/2
        [HttpPost]
        public ActionResult Update(int id, Class classes)
        {
            try
            {
                Debug.WriteLine("The new class information is: ");
                Debug.WriteLine(classes.ClassName);
                Debug.WriteLine(classes.Instructor);
                Debug.WriteLine(classes.Schedule);
                Debug.WriteLine(classes.Duration);
                Debug.WriteLine(classes.Price);
                Debug.WriteLine(classes.Status);
                Debug.WriteLine(classes.StudioID);

                // serialize update classdata into JSON
                // Send the request to the API
                // POST: api/danceclassData/Updateclass/{id}
                // Header : Content-Type: application/json

                string url = "danceclassdata/updateclass/" + id;
                string jsonpayload = jss.Serialize(classes);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return View();
            }
        }

        // GET: Studioclass/Deleteclass/2
        public ActionResult DeleteClass(int id)
        {
            // Get Particular class information

            // objective: communicate with our class data api to retrieve one class
            // curl https://localhost:44384/api/danceclassdata/findclass/{id}

            string url = "classdata/findclass/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ClassDto selectClass = response.Content.ReadAsAsync<ClassDto>().Result;

            return View(selectClass);
        }

        // POST: Danceclass/Delete/2
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                string url = "danceclassdata/deleteclass/" + id;
                HttpContent content = new StringContent("");
                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return View();
            }
        }

    }
}
