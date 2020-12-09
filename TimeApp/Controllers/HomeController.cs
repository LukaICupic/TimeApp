using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TimeApp.Data;
using TimeApp.Infrastructure.Interfaces;
using TimeApp.Models;
using TimeApp.Models.HomeVM;

namespace TimeApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IReport reportRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(IReport reportRepo, UserManager<ApplicationUser> userManager)
        {
            this.reportRepo = reportRepo;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var reports = await reportRepo.GetReports();

            List<Report> reportsList = new List<Report>();

            foreach (var rep in reports)
            {
                if (rep.Approved == true && rep.Remove == false && rep.IsHidden == false)
                    reportsList.Add(rep);
            }

            return View(reportsList);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            var report = await reportRepo.GetReport(Id);

            if (report == null)
            {
                ViewBag.ErrorMessage = "The report does not exist";
                return View("NotFound");
            }

            ReportViewModel model = new ReportViewModel(report);

            return View(model);
        }

        [HttpGet]
        public IActionResult AddReport()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddReport(ReportCreateViewModel model)
        {
            if (ModelState.IsValid)
            {

                //definirati da ovakvi "in review" reporti idu u zaseban page u sučelju adminovom
                //definirati ujedno da se nedefinirani reporti pojavljuju kod usera u zasebnoj rubrici
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                Report newReport = new Report(model.FirstName, model.LastName, model.Time)
                {
                    //hardkodirano zasad
                    Approved = true,
                    Remove = false,
                    IsHidden = false,
                    ApplicationUserId = user.Id,
                    ApplicationUser = user
                   
                };

                await reportRepo.AddReport(newReport);
                return RedirectToAction("index");
            }

            return View(model);
        }

        public async Task<IActionResult> RemoveReport(ReportViewModel model)
        {
            var report = await reportRepo.GetReport(model.Id);

            if (report == null)
            {
                ViewBag.ErrorMessage = "The report does not exist";
                return View("NotFound");
            }

            await reportRepo.RemoveReport(report);
            return RedirectToAction("index");

        }
    }
}
