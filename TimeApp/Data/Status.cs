using System.Collections.Generic;

namespace TimeApp.Data
{
    public class Status
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public ICollection<Report> Reports { get; set; }
    }
}
