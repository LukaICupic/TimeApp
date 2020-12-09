using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeApp.Data
{
    public class Report
    {
        public Report(string firstName, string lastName, DateTime time)
        {
            FirstName = firstName;
            LastName = lastName;
            Time = time;
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Time { get; set; }
        public bool Approved { get; set; }
        public bool Remove { get; set; }
        public bool IsHidden { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
