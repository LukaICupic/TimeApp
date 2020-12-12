using Microsoft.EntityFrameworkCore;
using System;
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
            var report = await context.Reports.SingleOrDefaultAsync(i => i.Id == Id);

            return report;
        }

        public async Task<ICollection<Report>> GetReports()
        {
            var model = await context.Reports
                                     .Where(r => r.IsHidden == false)
                                     .OrderBy(r => r.Time).ToListAsync();
            return model;
        }

        public async Task<ICollection<Report>> GetUnacceptedReports()
        {
            var model = await context.Reports.Where(r => r.IsHidden == false && r.Remove == true || r.Pending == true).ToListAsync();
            return model;
        }

        public async Task<Report> AddReport(Report report)
        {
            context.Reports.Add(report);
            await context.SaveChangesAsync();
            return report;
        }

        public async Task<Report> ApproveReport(Report report)
        {
            var newReport = await context.Reports.SingleAsync(i => i.Id == report.Id);

            newReport.Approved = true;
            newReport.Remove = false;
            newReport.IsHidden = report.IsHidden;
            newReport.Pending = false;

            await context.SaveChangesAsync();
            return newReport;
        }

        public async Task<Report> RemoveReport(Report report)
        {
            var newReport = await context.Reports.SingleAsync(i => i.Id == report.Id);

            newReport.Approved = false;
            newReport.Remove = true;
            newReport.IsHidden = report.IsHidden;
            newReport.Pending = false;

            await context.SaveChangesAsync();
            return newReport;
        }

        public async Task<Report> DeleteReport(Report report)
        {
            var newReport = await context.Reports.SingleAsync(i => i.Id == report.Id);

            newReport.Approved = report.Approved;
            newReport.Remove = report.Remove;
            newReport.IsHidden = true;
            newReport.Pending = false;

            await context.SaveChangesAsync();
            return newReport;
        }

        public async Task<Report> ResendReport(Report report)
        {
            var newReport = await context.Reports.SingleAsync(i => i.Id == report.Id);

            newReport.Approved = false;
            newReport.Remove = false;
            newReport.IsHidden = false;
            newReport.Pending = true;

            await context.SaveChangesAsync();
            return newReport;
        }
    }
}
