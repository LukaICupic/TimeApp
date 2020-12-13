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
        Task<Report> ChangeStatus(Report report, int status);
        public Task<int> SaveChangesAsync();
    }
}
