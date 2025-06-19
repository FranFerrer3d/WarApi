using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatchReportNamespace.Services
{
    public interface IMatchReportService
    {
        Task<IEnumerable<MatchReport>> GetAllAsync();
        Task<MatchReport?> GetByIdAsync(Guid id);
        Task<MatchReport> CreateAsync(MatchReport report);
        Task UpdateAsync(Guid id, MatchReport report);
        Task DeleteAsync(Guid id);
    }
}
