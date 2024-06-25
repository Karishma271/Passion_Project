using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Passion_Project.Models
{
    public class Class
    {
        // class ID is primary key for
        //
        // Datatable
        [Key]
        public int ClassID { get; set; }
        // Studio ID is primary key in Studio Datatable but in class Datatable it is foreign key.
        // However it represents one to many relationship
        // Ex. XYZ Studio have n Numbers of classs.
        [ForeignKey("Studios")]
        public int StudioID { get; set; }
        public virtual Studio Studios { get; set; }
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string Instructor { get; set; }
        public DateTime Schedule { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
    }

    public class ClassDto
    {
        public int ClassID { get; set; }
        public int StudioID { get; set; }
        public virtual Studio Studios { get; set; }
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string Instructor { get; set; }
        public DateTime Schedule { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
    }
}