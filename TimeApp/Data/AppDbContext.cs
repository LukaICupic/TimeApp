using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TimeApp.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Report> Reports { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Report>()
               .HasKey(i => i.Id);

            builder.Entity<ApplicationUser>()
                .HasMany(r => r.Reports)
                .WithOne(i => i.ApplicationUser)
                .HasForeignKey(i => i.ApplicationUserId);

            //builder.Entity<Report>()
            //    .HasData(
            //    new Report("Marko", "Marić", DateTime.Now) {Approved = true, Remove = false, IsHidden = false },
            //    new Report("Ana", "Anić", DateTime.Now) { Approved = true, Remove = false, IsHidden = false });
       
        }

    }
}
