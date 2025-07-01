using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatchReportNamespace.Repositories;
using WarApi.Services.Interfaces;

namespace MatchReportNamespace.Services
{
    public class MatchReportService : IMatchReportService
    {
        private readonly IMatchReportRepository _repository;
        private readonly IPlayerService _players;

        public MatchReportService(IMatchReportRepository repository, IPlayerService players)
        {
            _repository = repository;
            _players = players;
        }

        private void DecodePlayers(MatchReport report)
        {
            var a = _players.GetById(report.PlayerAId);
            if (a != null) report.PlayerA = a;
            var b = _players.GetById(report.PlayerBId);
            if (b != null) report.PlayerB = b;
        }

        public async Task<IEnumerable<MatchReport>> GetAllAsync()
        {
            var reports = (await _repository.GetAllAsync()).ToList();
            foreach (var r in reports) DecodePlayers(r);
            return reports;
        }

        public async Task<MatchReport?> GetByIdAsync(Guid id)
        {
            var report = await _repository.GetByIdAsync(id);
            if (report != null) DecodePlayers(report);
            return report;
        }

        public async Task<MatchReport> CreateAsync(MatchReport report)
        {
            report.Id = Guid.NewGuid();
            await _repository.AddAsync(report);
            return report;
        }


        public async Task<List<MatchReport>> GetReportsByUser(Guid id)
        {
            var allReports = (await _repository.GetAllAsync()).ToList();
            foreach (var r in allReports) DecodePlayers(r);
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
