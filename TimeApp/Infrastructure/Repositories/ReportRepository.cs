using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeApp.Data;
using TimeApp.Infrastructure.Interfaces;

namespace TimeApp.Infrastructure.Repositories
{
    public class ReportRepository : IReport
    {
        private readonly AppDbContext context;

        public ReportRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Report> GetReport(int Id)
        {
            var report = await context.Reports.Include(s => s.Status)
                .Where(r => r.Status.Value != "Deleted")
                .SingleOrDefaultAsync(r => r.Id == Id);

            return report;
        }

        public async Task<ICollection<Report>> GetReports()
        {
            var model = await context.Reports
                                .Include(s => s.Status)
                                .Where(r => r.Status.Value != "Deleted")
                                .OrderBy(r => r.Time)
                                .ToListAsync();
            
            return model;
        }

        public async Task<ICollection<Report>> GetUnacceptedReports()
        {
            var model = await context.Reports
                .Include(s => s.Status)
                .Where(r => r.Status.Value != "Deleted"
            && r.Status.Value == "Reviewing")
                .ToListAsync();

            return model;
        }

        public async Task<Report> AddReport(Report report)
        {
            var statuses = await context.Statuses.ToListAsync();
            var status = await context.Statuses.SingleAsync(s => s.Value == "Reviewing");

            if (status.Reports == null)
            {
                status.Reports = new List<Report>
                {
                    report
                };
            }

            else
            {
                status.Reports.Add(report);
            }

            report.Status = status;
            report.StatusId = status.Id;

            context.Reports.Add(report);
            return report;
        }

        public async Task<Report> ChangeStatus(Report report, string status)
        {
            var stat = await context.Statuses.SingleAsync(s => s.Value == status);

            if (stat.Reports == null)
            {
                stat.Reports = new List<Report>
                {
                    report
                };
            }

            else
            {
                stat.Reports.Add(report);
            }

            //report.Status = stat;
            report.StatusId = stat.Id;

            return report;
        }

        //public async Task<Report> ApproveReport(Report report)
        //{
        //    var status = await context.Statuses.SingleAsync(s => s.Value == "Accepted");
        //    status.Reports.Add(report);

        //    report.Status = status;
        //    report.StatusId = status.Id;

        //    return report;
        //}

        //public async Task<Report> RemoveReport(Report report)
        //{
        //    var status = await context.Statuses.SingleAsync(s => s.Value == "Removed");
        //    status.Reports.Add(report);

        //    report.Status = status;
        //    report.StatusId = status.Id;

        //    return report;

        //}

        //public async Task<Report> DeleteReport(Report report)
        //{
        //    var newReport = await GetReport(report.Id);
        //    newReport.Status.Value = "Removed";
        //    return newReport;
        //}

        //public async Task<Report> ResendReport(Report report)
        //{
        //    var newReport = await GetReport(report.Id);
        //    newReport.Status.Value = "Reviewing";
        //    return newReport;
        //}


        public void SaveChanges()
        {
            context.SaveChanges();

        }
    }
}
