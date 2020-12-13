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

        public async Task<Report> ChangeStatus(Report report, int statId)
        {
            var stat = await context.Statuses.SingleAsync(s => s.Id == statId);

            report.Status = stat;
            report.StatusId = stat.Id;

            return report;
        }
        public Task<int> SaveChangesAsync()
        {
           return context.SaveChangesAsync();
        }
    }
}
