using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TimeApp.Data
{
    public class ApplicationUser : IdentityUser<int>
    {
        public virtual ICollection<Report> Reports { get; set; }
    }
}
