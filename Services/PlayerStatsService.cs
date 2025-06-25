using MatchReportNamespace;
using MatchReportNamespace.Services;
using WarApi.Services.Interfaces;

namespace WarApi.Services
{
    public class PlayerStatsService : IPlayerStatsService
    {
        private readonly IMatchReportService _matchReportService;

        public PlayerStatsService(IMatchReportService matchReportService)
        {
            _matchReportService = matchReportService;
        }

        private async Task<List<MatchReport>> GetReports(Guid playerId)
        {
            return await _matchReportService.GetReportsByUser(playerId);
        }

        private static bool IsDraw(MatchReport report)
        {
            return report.FinalScoreA == report.FinalScoreB;
        }

        private static bool DidPlayerWin(MatchReport report, Guid playerId)
        {
            if (IsDraw(report)) return false;

            bool isA = report.PlayerAId == playerId;
            return isA ? report.FinalScoreA > report.FinalScoreB
                        : report.FinalScoreB > report.FinalScoreA;
        }

        private static bool DidPlayerLose(MatchReport report, Guid playerId)
        {
            if (IsDraw(report)) return false;
            return !DidPlayerWin(report, playerId);
        }

        private static bool PlayerUsedArmy(MatchReport report, Guid playerId, string army)
        {
            return (report.PlayerAId == playerId && string.Equals(report.ListA, army, StringComparison.OrdinalIgnoreCase)) ||
                   (report.PlayerBId == playerId && string.Equals(report.ListB, army, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<int> GetTotalGames(Guid playerId)
        {
            var reports = await GetReports(playerId);
            return reports.Count;
        }

        public async Task<int> GetWins(Guid playerId)
        {
            var reports = await GetReports(playerId);
            return reports.Count(r => DidPlayerWin(r, playerId));
        }

        public async Task<int> GetLosses(Guid playerId)
        {
            var reports = await GetReports(playerId);
            return reports.Count(r => DidPlayerLose(r, playerId));
        }

        public async Task<int> GetDraws(Guid playerId)
        {
            var reports = await GetReports(playerId);
            return reports.Count(IsDraw);
        }

        public async Task<int> GetGamesByArmy(Guid playerId, string army)
        {
            var reports = await GetReports(playerId);
            return reports.Count(r => PlayerUsedArmy(r, playerId, army));
        }

        public async Task<int> GetWinsByArmy(Guid playerId, string army)
        {
            var reports = await GetReports(playerId);
            return reports.Where(r => PlayerUsedArmy(r, playerId, army))
                          .Count(r => DidPlayerWin(r, playerId));
        }

        private static double CalculateWinRate(int wins, int games)
        {
            return games == 0 ? 0 : (double)wins / games * 100.0;
        }

        public async Task<double> GetWinRateOnMap(Guid playerId, string map)
        {
            var reports = (await GetReports(playerId)).Where(r => string.Equals(r.Map, map, StringComparison.OrdinalIgnoreCase)).ToList();
            int wins = reports.Count(r => DidPlayerWin(r, playerId));
            return CalculateWinRate(wins, reports.Count);
        }

        public async Task<double> GetWinRateByDeployment(Guid playerId, string deployment)
        {
            var reports = (await GetReports(playerId)).Where(r => string.Equals(r.Deployment, deployment, StringComparison.OrdinalIgnoreCase)).ToList();
            int wins = reports.Count(r => DidPlayerWin(r, playerId));
            return CalculateWinRate(wins, reports.Count);
        }

        public async Task<double> GetWinRateByPrimary(Guid playerId, string primary)
        {
            var reports = (await GetReports(playerId)).Where(r => string.Equals(r.PrimaryMission, primary, StringComparison.OrdinalIgnoreCase)).ToList();
            int wins = reports.Count(r => DidPlayerWin(r, playerId));
            return CalculateWinRate(wins, reports.Count);
        }

        public async Task<string?> GetBestOpponentFaction(Guid playerId)
        {
            var reports = await GetReports(playerId);
            var stats = new Dictionary<string, (int Wins, int Games)>();

            foreach (var r in reports)
            {
                string faction = r.PlayerAId == playerId ? r.ListB : r.ListA;
                if (!stats.ContainsKey(faction)) stats[faction] = (0, 0);
                var val = stats[faction];
                if (DidPlayerWin(r, playerId)) val.Wins++;
                val.Games++;
                stats[faction] = val;
            }

            if (stats.Count == 0) return null;

            return stats.OrderByDescending(kv => CalculateWinRate(kv.Value.Wins, kv.Value.Games)).First().Key;
        }

        public async Task<string?> GetWorstOpponentFaction(Guid playerId)
        {
            var reports = await GetReports(playerId);
            var stats = new Dictionary<string, (int Wins, int Games)>();

            foreach (var r in reports)
            {
                string faction = r.PlayerAId == playerId ? r.ListB : r.ListA;
                if (!stats.ContainsKey(faction)) stats[faction] = (0, 0);
                var val = stats[faction];
                if (DidPlayerWin(r, playerId)) val.Wins++;
                val.Games++;
                stats[faction] = val;
            }

            if (stats.Count == 0) return null;

            return stats.OrderBy(kv => CalculateWinRate(kv.Value.Wins, kv.Value.Games)).First().Key;
        }
private static void Accumulate(Dictionary<string, (int Wins, int Games)> dict, string key, bool win)
        {
            if (!dict.ContainsKey(key)) dict[key] = (0, 0);
            var val = dict[key];
            if (win) val.Wins++;
            val.Games++;
            dict[key] = val;
        }

        public async Task<PlayerIdealScenarioDto> GetIdealScenario(Guid playerId, int top)
        {
            var reports = await GetReports(playerId);

            var opponents = new Dictionary<string, (int Wins, int Games)>();
            var maps = new Dictionary<string, (int Wins, int Games)>();
            var primaries = new Dictionary<string, (int Wins, int Games)>();
            var secondaries = new Dictionary<string, (int Wins, int Games)>();

            foreach (var r in reports)
            {
                bool win = DidPlayerWin(r, playerId);
                var opp = r.PlayerAId == playerId ? r.ListB : r.ListA;
                var sec = r.PlayerAId == playerId ? r.SecondaryA : r.SecondaryB;

                Accumulate(opponents, opp, win);
                Accumulate(maps, r.Map, win);
                Accumulate(primaries, r.PrimaryMission, win);
                Accumulate(secondaries, sec, win);
            }

            int limit = Math.Clamp(top, 1, 12);

            PlayerIdealScenarioDto result = new()
            {
                OpponentFactions = opponents
                    .OrderByDescending(kv => CalculateWinRate(kv.Value.Wins, kv.Value.Games))
                    .Take(limit)
                    .Select(kv => kv.Key)
                    .ToList(),
                Maps = maps
                    .OrderByDescending(kv => CalculateWinRate(kv.Value.Wins, kv.Value.Games))
                    .Take(limit)
                    .Select(kv => kv.Key)
                    .ToList(),
                PrimaryMissions = primaries
                    .OrderByDescending(kv => CalculateWinRate(kv.Value.Wins, kv.Value.Games))
                    .Take(limit)
                    .Select(kv => kv.Key)
                    .ToList(),
                SecondaryMissions = secondaries
                    .OrderByDescending(kv => CalculateWinRate(kv.Value.Wins, kv.Value.Games))
                    .Take(limit)
                    .Select(kv => kv.Key)
                    .ToList()
            };

            return result;
        }

    }
}
