using Passion_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace Passion_Project.Controllers
{
    public class DanceClassDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DanceClassData/ListClasses
        [HttpGet]
        [Route("api/DanceClassData/ListClasses")]
        [ResponseType(typeof(IEnumerable<ClassDto>))]
        public IHttpActionResult ListClasses()
        {
            try
            {
                List<ClassDto> classDtos = db.Classes
                    .Include(c => c.Studios) // Assuming Studios is the correct navigation property
                    .Select(c => new ClassDto
                    {
                        ClassID = c.ClassID,
                        ClassName = c.ClassName,
                        Instructor = c.Instructor,
                        Schedule = c.Schedule,
                        Duration = c.Duration,
                        Price = c.Price,
                        Status = c.Status,
                        StudioID = c.StudioID,
                        Name = c.Studios.Name // Assuming Studios has a Name property
                    })
                    .ToList();

                return Ok(classDtos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/DanceClassData/FindClass/{id}
        [HttpGet]
        [Route("api/DanceClassData/FindClass/{id}")]
        [ResponseType(typeof(ClassDto))]
        public IHttpActionResult FindClass(int id)
        {
            try
            {
                Class danceClass = db.Classes
                    .Include(c => c.Studios) // Assuming Studios is the correct navigation property
                    .FirstOrDefault(c => c.ClassID == id);

                if (danceClass == null)
                {
                    return NotFound();
                }

                ClassDto classDto = new ClassDto
                {
                    ClassID = danceClass.ClassID,
                    ClassName = danceClass.ClassName,
                    Instructor = danceClass.Instructor,
                    Schedule = danceClass.Schedule,
                    Duration = danceClass.Duration,
                    Price = danceClass.Price,
                    Status = danceClass.Status,
                    StudioID = danceClass.StudioID,
                    Name = danceClass.Studios.Name // Assuming Studios has a Name property
                };

                return Ok(classDto);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/DanceClassData/AddClass
        [HttpPost]
        [Route("api/DanceClassData/AddClass")]
        [ResponseType(typeof(Class))]
        public IHttpActionResult AddClass(Class danceClass)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.Classes.Add(danceClass);
                db.SaveChanges();

                return Ok(danceClass);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/DanceClassData/UpdateClass/{id}
        [HttpPost]
        [Route("api/DanceClassData/UpdateClass/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateClass(int id, Class danceClass)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != danceClass.ClassID)
            {
                return BadRequest();
            }

            try
            {
                db.Entry(danceClass).State = EntityState.Modified;
                db.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                if (!ClassExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return InternalServerError(ex);
                }
            }
        }

        // POST: api/DanceClassData/DeleteClass/{id}
        [HttpPost]
        [Route("api/DanceClassData/DeleteClass/{id}")]
        [ResponseType(typeof(Class))]
        public IHttpActionResult DeleteClass(int id)
        {
            try
            {
                Class danceClass = db.Classes.Find(id);
                if (danceClass == null)
                {
                    return NotFound();
                }

                db.Classes.Remove(danceClass);
                db.SaveChanges();

                return Ok(danceClass);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private bool ClassExists(int id)
        {
            return db.Classes.Any(c => c.ClassID == id);
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
