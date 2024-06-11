using Passion_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Passion_Project.Controllers
{
    public class DanceClassDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves a list of all 
        /// s.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: All classs in the database, including their details such as class name, instructor, schedule, duration, price, status and associated studio name.
        /// </returns>
        // GET: api/classData/Listclasss
        [HttpGet]
        [Route("api/DanceClassData/ListClasses")]
        public IEnumerable<ClassDto> ListClasses()
        {
            List<Class> Classes = db.Class.ToList();
            List<ClassDto> ClassDtos = new List<ClassDto>();

            Classes.ForEach(classes => ClassDtos.Add(new ClassDto()
            {
                ClassID = classes.ClassID,
                ClassName = classes.ClassName,
                Instructor = classes.Instructor,
                Schedule = classes.Schedule,
                Duration = classes.Duration,
                Price = classes.Price,
                Status = classes.Status,
                
            }));

            return ClassDtos;
        }

        /// <summary>
        /// Retrieves information about a class by its ID.
        /// </summary>
        /// <param name="classId">The ID of the class to retrieve.</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: The class information, including its details such as class name, instructor, schedule, duration, price, status and associated studio name.
        // GET: api/classData/Findclass/2
        [ResponseType(typeof(Class))]
        [HttpGet]
        [Route("api/DanceClassData/FindClass/{classId}")]
        public IHttpActionResult FindClass(int classId)
        {
            Class classes = db.Class.Find(classId);
            ClassDto classDto = new ClassDto()
            {
                ClassID = classes.ClassID,
                ClassName = classes.ClassName,
                Instructor = classes.Instructor,
                Schedule = classes.Schedule,
                Duration = classes.Duration,
                Price = classes.Price,
                Status = classes.Status,
                
                StudioID = classes.Studios.StudioID
            };
            if (classDto == null)
            {
                return NotFound(); // HTTP Status Code 404
            }
            return Ok(classDto); // return class Object
        }

        /// <summary>
        /// Adds a new class to the system.
        /// </summary>
        /// <param name="class">The class object containing the information to add.</param>
        /// <returns>
        /// HEADER: 200 (OK) if the class is added successfully.
        /// </returns>
        // POST: api/danceclassData/Addclass
        [ResponseType(typeof(Class))]
        [HttpPost]
        [Route("api/DanceClassData/AddClass")]
        public IHttpActionResult AddClass(Class classes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Class.Add(classes);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Updates the information of an existing class in the system.
        /// </summary>
        /// <param name="id">The ID of the class to update.</param>
        /// <param name="class">The updated class object with new information.</param>
        /// <returns>
        /// HEADER: 204 (No Content) if the class is updated successfully.
        /// </returns>
        // POST: api/danceclassData/Updateclass/2
        [ResponseType(typeof(Class))]
        [HttpPost]
        [Route("api/DanceClassData/UpdateClass/{id}")]
        public IHttpActionResult Updateclass(int id, Class classes)
        {
            Debug.WriteLine("Check!! Is It Reached to the update method or not!!");
            // curl -H "Content-Type:application/json" -d @class.json https://localhost:
            // /api/classData/updateclass/6
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("State is not valid for this Model");
                return BadRequest(ModelState);
            }

            if (id != classes.ClassID)
            {
                Debug.WriteLine("ID is not match!!");
                Debug.WriteLine("Fetch ID: " + id);
                Debug.WriteLine("POST Parameter - Class ID: " + classes.ClassID);
                Debug.WriteLine("POST Parameter - Class Name: " + classes.ClassName);
                Debug.WriteLine("POST Parameter - Type Of Class: " + classes.Instructor);
                Debug.WriteLine("POST Parameter - Price of Class: " + classes.Price);
                Debug.WriteLine("POST Parameter - Status of Class: " + classes.Status);
                Debug.WriteLine("POST Parameter - Studio Name of that Class: " + classes.StudioName);
                Debug.WriteLine("POST Parameter - Studio ID of that Class: " + classes.Studios.StudioID);
            }

            db.Entry(classes).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!ClassExists(id))
                {
                    Debug.WriteLine("class not found");
                    return NotFound();
                }
                else
                {
                    Debug.WriteLine(e);
                    throw;
                }
            }
            Debug.WriteLine("Nothing is trigger in update function !!");
            return (StatusCode(HttpStatusCode.NoContent));
        }

        /// <summary>
        /// Deletes a class from the system by its ID.
        /// </summary>
        /// <param name="id">The ID of the class to delete.</param>
        /// <returns>
        /// HEADER: 200 (OK) if the class is deleted successfully.
        /// HEADER: 404 (Not Found) if the class with the given ID is not found.
        /// </returns>
        // POST: api/danceclassData/Deleteclass/2
        [ResponseType(typeof(Class))]
        [HttpPost]
        [Route("api/DanceClassData/DeleteClass/{id}")]
        public IHttpActionResult Deleteclass(int id)
        {
            Class classes = db.Class.Find(id);
            if (classes == null)
            {
                return NotFound();
            }
            db.Class.Remove(classes);
            db.SaveChanges();

            return Ok();
        }

        // Check Is class Already Exists and return boolean value either true or false
        private bool ClassExists(int id)
        {
            return db.Class.Count(e => e.ClassID == id) > 0;
        }
    }
}