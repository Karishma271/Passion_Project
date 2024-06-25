using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Passion_Project.Models
{
    public class Studio
    {
        // Key - Represents Primary Key Of table
        //
        // ID is primary key for Studio Datatable
        [Key]
        public int StudioID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public string Facilities { get; set; }
    }

    public class StudioDto
    {
        public int StudioID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public string Facilities { get; set; }
        // Navigation property for classes associated with this studio
        

    }
}