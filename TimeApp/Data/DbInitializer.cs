using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeApp.Data
{
    public class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Statuses.Any())
            {
                return;
            }

            var statuses = new Status[]
            {
                new Status { Id = 1, Value = "Accepted" },
                new Status { Id = 2, Value = "Rejected" },
                new Status { Id = 3, Value = "Reviewing"},
                new Status { Id = 4, Value = "Deleted"}
            };

            foreach (Status s in statuses)
            {
                context.Statuses.Add(s);
            }

            context.SaveChanges();
        }
    }
}
