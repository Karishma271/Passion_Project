using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Passion_Project.Models; // Assuming this is where your DTOs are defined

namespace Passion_Project.Models.ViewModels
{
    public class AddBookingViewModel
    {
        public IEnumerable<ClassDto> Classes { get; set; }
        public IEnumerable<UserDto> Users { get; set; }

        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; }

        public int BookingID { get; set; }

        public int UserID { get; set; }
        public string Username { get; set; } // Add Username property

        public int ClassID { get; set; }
        public string ClassName { get; set; } // Add ClassName property

        [Display(Name = "Class Date")]
        public DateTime ClassDate { get; set; }

        public string Status { get; set; }

        // Constructors if necessary
        public AddBookingViewModel()
        {
            // Initialize collections if necessary
            Classes = new List<ClassDto>();
            Users = new List<UserDto>();
        }
    }
}
