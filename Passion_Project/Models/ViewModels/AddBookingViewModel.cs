using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Passion_Project.Models.ViewModels
{
    public class AddBookingViewModel
    {
        public IEnumerable<ClassDto> Classes { get; set; }
        public IEnumerable<UserDto> Users { get; set; }
    }
}