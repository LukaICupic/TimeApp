using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TimeApp.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Report> Reports { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Report>(e =>
            {
                e.HasKey(i => i.Id);
            });

            builder.Entity<ApplicationUser>(e =>
            {
                e.HasMany(r => r.Reports)
                .WithOne(i => i.ApplicationUser)
                .HasForeignKey(i => i.ApplicationUserId);
            });
                

            builder.Entity<Status>(e =>
            {
                e.HasKey(i => i.Id);
                e.HasMany(r => r.Reports)
                .WithOne(s => s.Status)
                .HasForeignKey(i => i.StatusId);
                e.HasData
                (
                new { Id = 1, Value = "Accepted" },
                new { Id = 2, Value = "Rejected"  },
                new { Id = 3, Value = "Reviewing"  },
                new { Id = 4, Value = "Deleted"  }
                );
            });
        }

    }
}
