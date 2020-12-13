using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TimeApp.Data;

namespace TimeApp.Models.HomeVM
{
    public class ReportViewModel
    {
        public ReportViewModel(Report r)
        {
            Id = r.Id;
            FirstName = r.FirstName;
            LastName = r.LastName;
            Time = r.Time;
            StatusId = r.StatusId;
            ApplicationUserId = r.ApplicationUserId;
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public TimeSpan Time { get; set; }
        public int StatusId { get; set; }
        public int ApplicationUserId { get; set; }
    }
}
