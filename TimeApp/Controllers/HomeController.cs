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
        public IActionResult proba()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var reports = await reportRepo.GetReports();

            List<ReportViewModel> reportsList = new List<ReportViewModel>();

            foreach (var rep in reports)
            {
                if (rep.Approved == true && rep.Remove == false && rep.Pending == false)
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

                //definirati da ovakvi "in review" reporti idu u zaseban page u sučelju adminovom
                //definirati ujedno da se nedefinirani reporti pojavljuju kod usera u zasebnoj rubrici

                var date = new DateTime(2000, 1, 1, model.Time.Hour, model.Time.Minute, model.Time.Second);
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                Report newReport = new Report(model.FirstName, model.LastName, date)
                {
                    //hardkodirano zasad
                    Approved = false,
                    Remove = false,
                    IsHidden = false,
                    Pending = true,
                    ApplicationUserId = user.Id,
                    ApplicationUser = user
                   
                };

                await reportRepo.AddReport(newReport);
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

            await reportRepo.DeleteReport(report);
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

            await reportRepo.ResendReport(report);
            return RedirectToAction("MyReports");
        }

        //resendreport

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

            await reportRepo.RemoveReport(report);
            return RedirectToAction("index");

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

            await reportRepo.ApproveReport(report);
            return RedirectToAction("IncomingReports");
        }
    }
}
