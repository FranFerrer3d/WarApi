using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatchReportNamespace.Repositories
{
    public interface IMatchReportRepository
    {
        Task<IEnumerable<MatchReport>> GetAllAsync();
        Task<MatchReport> GetByIdAsync(Guid id);
        Task AddAsync(MatchReport report);
        Task UpdateAsync(MatchReport report);
        Task DeleteAsync(Guid id);
    }
}
