using System.Collections.Generic;
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
        Task<Report> ChangeStatus(Report report, string status);
        //Task<Report> RemoveReport(Report report);
        //Task<Report> DeleteReport(Report report);
        //Task<Report> ResendReport(Report report);
        void SaveChanges();
    }
}
