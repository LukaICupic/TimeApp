using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeApp.Data;

namespace TimeApp.Infrastructure.Interfaces
{
    public interface IReport
    {
        Task<Report> GetReport(int Id);
        Task<ICollection<Report>> GetReports();
        Task<ICollection<Report>> GetUnacceptedReports();
        Task<Report> AddReport(Report report);
        Task<Report> ApproveReport(Report report);
        Task<Report> RemoveReport(Report report);
        Task<Report> DeleteReport(Report report);
        Task<Report> ResendReport(Report report);
    }
}
