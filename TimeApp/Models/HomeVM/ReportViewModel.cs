﻿using System;
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
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public TimeSpan Time { get; set; }
        public string Value { get; set; }
        public bool IsHidden { get; set; }
    }
}
