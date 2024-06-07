using Passion_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Passion_Project.Controllers
{
    public class StudioDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/StudioData/ListStudios
        [HttpGet]
        [Route("api/StudioData/ListStudios")]
        public IEnumerable<StudioDto> ListStudios()
        {
            List<Studio> Studios = db.Studios.ToList();
            List<StudioDto> StudioDtos = new List<StudioDto>();

            Studios.ForEach(Studio => StudioDtos.Add(new StudioDto()
            {
               StudioID = Studio.StudioID,
                Name = Studio.Name,
                Location = Studio.Location,
                Capacity = Studio.Capacity,
                Facilities = Studio.Facilities
            }));

            return StudioDtos;
        }
    }
}