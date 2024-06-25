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
    public class DanceClassUserDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DanceClassUserData/ListUsers
        [HttpGet]
        [Route("api/DanceClassUserData/ListUsers")]
        public IEnumerable<UserDto> ListUsers()
        {
            List<User> users = db.Users.ToList();
            List<UserDto> userDtos = new List<UserDto>();

            users.ForEach(user => userDtos.Add(new UserDto()
            {
                UserID = user.UserID,
                Username = user.Username,
                Password = user.Password,
                Email = user.Email,
            }));

            return userDtos;
        }

        // GET: api/DanceClassUserData/FindUser/2
        [ResponseType(typeof(UserDto))]
        [HttpGet]
        [Route("api/DanceClassUserData/FindUser/{userId}")]
        public IHttpActionResult FindUser(int userId)
        {
            User user = db.Users.Find(userId);
            if (user == null)
            {
                return NotFound();
            }

            UserDto userDto = new UserDto()
            {
                UserID = user.UserID,
                Username = user.Username,
                Password = user.Password,
                Email = user.Email,
            };

            return Ok(userDto);
        }

        // POST: api/DanceClassUserData/AddUser
        [ResponseType(typeof(User))]
        [HttpPost]
        [Route("api/DanceClassUserData/AddUser")]
        public IHttpActionResult AddUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);
            db.SaveChanges();

            return Ok();
        }

        // POST: api/DanceClassUserData/UpdateUser/2
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/DanceClassUserData/UpdateUser/{id}")]
        public IHttpActionResult UpdateUser(int id, UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userDto.UserID)
            {
                return BadRequest("ID mismatch");
            }

            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Username = userDto.Username;
            user.Email = userDto.Email;

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // DELETE: api/DanceClassUserData/DeleteUser/5
        [HttpDelete]
        [Route("api/DanceClassUserData/DeleteUser/{id}")]
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }


        private bool UserExists(int id)
        {
            return db.Users.Any(e => e.UserID == id);
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
