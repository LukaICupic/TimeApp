using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeApp.Data
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Report> Reports { get; set; }
    }
}
