using Passion_Project.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Passion_Project.Controllers
{
    public class StudioDataController : ApiController
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves a list of all studios.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: All studios in the database, including their details such as ID, name, location, capacity, and facilities.
        /// </returns>
        [HttpGet]
        [Route("api/StudioData/ListStudios")]
        public IEnumerable<StudioDto> ListStudios()
        {
            // Retrieve list of studios from database
            List<Studio> studios = db.Studios.ToList();

            // Convert each Studio object to StudioDto
            List<StudioDto> studioDtos = studios.Select(s => new StudioDto
            {
                StudioID = s.StudioID,
                Name = s.Name,
                Location = s.Location,
                Capacity = s.Capacity,
                Facilities = s.Facilities
            }).ToList();

            return studioDtos;
        }
    }
}
