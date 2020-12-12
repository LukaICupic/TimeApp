using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimeApp.Data;
using TimeApp.Infrastructure.Interfaces;
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

            List<ReportViewModel> reportsList = new List<ReportViewModel>();

            foreach (var rep in reports)
            {
                if (rep.Status.Value == "Accepted")
                {
                    ReportViewModel model = new ReportViewModel(rep);
                    reportsList.Add(model);
                }
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
        //action for user
        public async Task<IActionResult> AddReport(ReportCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                Report newReport = new Report(model.FirstName, model.LastName, model.Time)
                {
                    ApplicationUser = user,
                    ApplicationUserId = user.Id,
                };
                
                await reportRepo.AddReport(newReport);
                reportRepo.SaveChanges();
                return RedirectToAction("index");
            }

            return View(model);
        }

        [HttpGet]
        //action for admin
        public async Task<IActionResult> IncomingReports()
        {
            var reports = await reportRepo.GetUnacceptedReports();

            List<ReportViewModel> reportList = new List<ReportViewModel>();

            foreach (var rep in reports)
            {
                ReportViewModel model = new ReportViewModel(rep);
                reportList.Add(model);
            }

            return View(reportList);
        }

        //Action for user
        [HttpGet]
        public async Task<IActionResult> MyReports()
        {
            var user = await userManager.FindByEmailAsync(User.Identity.Name);
            var reports = await reportRepo.GetReports();

            List<Report> reportsList = new List<Report>();
            
            foreach(var report in reports)
            {
                if (report.ApplicationUserId == user.Id)
                    reportsList.Add(report);
            }

            return View(reportsList);
        }

        //action for user
        [HttpPost]
        public async Task<IActionResult> DeleteReport(int Id)
        {
            var report = await reportRepo.GetReport(Id);
            if (report == null)
            {
                ViewBag.ErrorMessage = "The report does not exist";
                return View("NotFound");
            }

            await reportRepo.ChangeStatus(report, "Deleted");
            reportRepo.SaveChanges();
            return RedirectToAction("MyReports");
        }

        //action for user
        [HttpPost]
        public async Task<IActionResult> ResendReport(int Id)
        {
            var report = await reportRepo.GetReport(Id);
            if (report == null)
            {
                ViewBag.ErrorMessage = "The report does not exist";
                return View("NotFound");
            }

            await reportRepo.ChangeStatus(report, "Reviewing");
            reportRepo.SaveChanges();
            return RedirectToAction("MyReports");
        }

        [HttpPost]
        //action for admin
        public async Task<IActionResult> RemoveReport(int Id)
        {
            var report = await reportRepo.GetReport(Id);

            if (report == null)
            {
                ViewBag.ErrorMessage = "The report does not exist";
                return View("NotFound");
            }

            await reportRepo.ChangeStatus(report, "Rejected");
            reportRepo.SaveChanges();
            return RedirectToAction("IncomingReports");

        }

        [HttpPost]
        //action for admin
        public async Task<IActionResult> AcceptReport(int Id)
        {
            var report = await reportRepo.GetReport(Id);

            if (report == null)
            {
                ViewBag.ErrorMessage = "The report does not exist";
                return View("NotFound");
            }

            await reportRepo.ChangeStatus(report, "Accepted");
            reportRepo.SaveChanges();
            return RedirectToAction("IncomingReports");
        }
    }
}
