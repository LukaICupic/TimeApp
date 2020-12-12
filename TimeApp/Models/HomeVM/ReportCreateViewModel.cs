using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeApp.Models.HomeVM
{
    public class ReportCreateViewModel
    {
        [Required(ErrorMessage = "Firstname is a required field")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Lastname is a required field")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Time is a required field")]
        public TimeSpan Time { get; set; }
    }
}
