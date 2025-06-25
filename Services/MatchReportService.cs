using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatchReportNamespace.Repositories;

namespace MatchReportNamespace.Services
{
    public class MatchReportService : IMatchReportService
    {
        private readonly IMatchReportRepository _repository;

        public MatchReportService(IMatchReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MatchReport>> GetAllAsync() =>
            await _repository.GetAllAsync();

        public async Task<MatchReport?> GetByIdAsync(Guid id) =>
            await _repository.GetByIdAsync(id);

        public async Task<MatchReport> CreateAsync(MatchReport report)
        {
            report.Id = Guid.NewGuid();
            await _repository.AddAsync(report);
            return report;
        }


        public async Task<List<MatchReport>> GetReportsByUser(Guid id)
        {
            var allReports = await _repository.GetAllAsync();
            return allReports.Where(x => x.PlayerAId == id || x.PlayerBId == id).ToList();
        }

        public async Task UpdateAsync(Guid id, MatchReport updatedReport)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return;

            updatedReport.Id = id;
            await _repository.UpdateAsync(updatedReport);
        }

        public async Task DeleteAsync(Guid id) =>
            await _repository.DeleteAsync(id);
    }
}
