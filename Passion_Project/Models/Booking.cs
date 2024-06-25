using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Passion_Project.Models
{
    
   


    
        public class Booking
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int BookingID { get; set; }

            [ForeignKey("User")]
            public int UserID { get; set; }
            public virtual User User { get; set; }

            [ForeignKey("Class")]
            public int ClassID { get; set; }
            public virtual Class Class { get; set; }

            public DateTime BookingDate { get; set; }
            public DateTime ClassDate { get; set; }

        
        public string Status { get; set; }
        }
    

    public class BookingDto
    {
        public int BookingID { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public int ClassID { get; set; }
        public string ClassName { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime ClassDate { get; set; }
        public string Status { get; set; }
    }
}
