using Microsoft.EntityFrameworkCore;
using WarApi.Models;

namespace MatchReportNamespace.Repositories
{
    public class MatchReportRepository : IMatchReportRepository
    {
        private readonly AppDbContext _context;

        public MatchReportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MatchReport>> GetAllAsync()
        {
            return await _context.MatchReports.ToListAsync();
        }

        public async Task<MatchReport?> GetByIdAsync(Guid id)
        {
            return await _context.MatchReports.FindAsync(id);
        }

        public async Task AddAsync(MatchReport report)
        {
            await _context.MatchReports.AddAsync(report);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MatchReport report)
        {
            _context.MatchReports.Update(report);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var report = await _context.MatchReports.FindAsync(id);
            if (report != null)
            {
                _context.MatchReports.Remove(report);
                await _context.SaveChangesAsync();
            }
        }
    }
}
