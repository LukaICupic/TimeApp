using System;

namespace TimeApp.Data
{
    public class Report
    {
        public Report(string firstName, string lastName, TimeSpan time)
        {
            FirstName = firstName;
            LastName = lastName;
            Time = time;
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public TimeSpan Time { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
