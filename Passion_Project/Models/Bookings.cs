using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Passion_Project.Models
{
    public class Bookings
    {
        //
        // ID is primary key for booking Datatable
        [Key]
        public int BookingID { get; set; }
        // User ID is primary key in User table but in booking table it is foreign key.
        // However it represents one to many relationship
        // Ex. ABC user have n Numbers of booking.
        [ForeignKey("Users")]
        public int UserID { get; set; }
        public virtual User Users { get; set; }
        //
        //
        // ID is primary key in class table but in booking table it is foreign key.
        // However it represents one to many relationship
        // Ex. PQR class have n Numbers of booking on different dates or different time.
        [ForeignKey("Classes")]
        public int ClassID { get; set; }
        public virtual Class Classes { get; set; }

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