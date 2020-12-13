using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TimeApp.Data;
using TimeApp.Infrastructure.Interfaces;
using TimeApp.Infrastructure.Repositories;

namespace TimeApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        async Task CreateRole(IServiceProvider serviceProvider)
        {
            var db = serviceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();

            //obrisati nakon što završim
            //db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string role = "Admin";
            string userRole = "User";

            bool existingRole = await RoleManager.RoleExistsAsync(role);
            bool existingSecondRole = await RoleManager.RoleExistsAsync(userRole);


            if (!existingRole)
                await RoleManager.CreateAsync(new IdentityRole<int>(role));

            if (!existingSecondRole)
                await RoleManager.CreateAsync(new IdentityRole<int>(userRole));

            var adminUser = await UserManager.FindByEmailAsync("admin@gmail.com");

            if (adminUser == null)
            {
                var superAdmin = new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com"
                };

                string adminPass = "123456A";

                var createsuperAdmin = await UserManager.CreateAsync(superAdmin, adminPass);

                if (createsuperAdmin.Succeeded)
                {
                    await UserManager.AddToRoleAsync(superAdmin, role);
                }
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //prilagoditi
            services.AddIdentity<ApplicationUser, IdentityRole<int>>(options => {

                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

            }).AddEntityFrameworkStores<AppDbContext>();

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddScoped<IReport, ReportRepository>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //proučiti
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            CreateRole(serviceProvider).Wait();
        }
    }
}
