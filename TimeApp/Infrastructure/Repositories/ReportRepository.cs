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
            var model = await context.Reports.ToListAsync();
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
            newReport.Remove = report.Remove;
            newReport.IsHidden = report.IsHidden;

            await context.SaveChangesAsync();
            return newReport;
        }

        public async Task<Report> RemoveReport(Report report)
        {
            var newReport = await context.Reports.SingleAsync(i => i.Id == report.Id);

            newReport.Approved = report.Approved;
            newReport.Remove = true;
            newReport.IsHidden = report.IsHidden;

            await context.SaveChangesAsync();
            return newReport;
        }

        public async Task<Report> DeleteReport(Report report)
        {
            var newReport = await context.Reports.SingleAsync(i => i.Id == report.Id);

            newReport.Approved = report.Approved;
            newReport.Remove = report.Remove;
            newReport.IsHidden = true;

            await context.SaveChangesAsync();
            return newReport;
        }
    }
}
