using MatchReportNamespace;
using MatchReportNamespace.Services;
using WarApi.Services.Interfaces;
using WarApi.Dtos;

namespace WarApi.Services
{
    public class TeamStatsService : ITeamStatsService
    {
        private readonly IMatchReportService _matchReportService;
        private readonly IPlayerService _playerService;

        public TeamStatsService(IMatchReportService matchReportService, IPlayerService playerService)
        {
            _matchReportService = matchReportService;
            _playerService = playerService;
        }

        private async Task<List<MatchReport>> GetReports(string teamName)
        {
            return await _matchReportService.GetReportsByTeam(teamName);
        }

        private HashSet<Guid> GetPlayerIds(string teamName)
        {
            return _playerService.GetAll().Where(p => p.Equipo == teamName).Select(p => p.ID).ToHashSet();
        }

        private static bool IsDraw(MatchReport report)
        {
            return report.FinalScoreA == report.FinalScoreB;
        }

        private static bool DidTeamWin(MatchReport report, HashSet<Guid> players)
        {
            bool a = players.Contains(report.PlayerAId);
            bool b = players.Contains(report.PlayerBId);
            if (a == b) return false;
            if (IsDraw(report)) return false;
            return a ? report.FinalScoreA > report.FinalScoreB
                     : report.FinalScoreB > report.FinalScoreA;
        }

        private static bool DidTeamLose(MatchReport report, HashSet<Guid> players)
        {
            bool a = players.Contains(report.PlayerAId);
            bool b = players.Contains(report.PlayerBId);
            if (a == b) return false;
            if (IsDraw(report)) return false;
            return a ? report.FinalScoreA < report.FinalScoreB
                     : report.FinalScoreB < report.FinalScoreA;
        }

        private static bool TeamUsedArmy(MatchReport report, HashSet<Guid> players, string army)
        {
            bool a = players.Contains(report.PlayerAId);
            bool b = players.Contains(report.PlayerBId);
            if (a && !b)
                return string.Equals(report.ListA, army, StringComparison.OrdinalIgnoreCase);
            if (!a && b)
                return string.Equals(report.ListB, army, StringComparison.OrdinalIgnoreCase);
            return false;
        }

        private static double CalculateWinRate(int wins, int games)
        {
            return games == 0 ? 0 : (double)wins / games * 100.0;
        }

        public async Task<int> GetTotalGames(string teamName)
        {
            var reports = await GetReports(teamName);
            return reports.Count;
        }

        public async Task<int> GetWins(string teamName)
        {
            var reports = await GetReports(teamName);
            var players = GetPlayerIds(teamName);
            return reports.Count(r => DidTeamWin(r, players));
        }

        public async Task<int> GetLosses(string teamName)
        {
            var reports = await GetReports(teamName);
            var players = GetPlayerIds(teamName);
            return reports.Count(r => DidTeamLose(r, players));
        }

        public async Task<int> GetDraws(string teamName)
        {
            var reports = await GetReports(teamName);
            return reports.Count(IsDraw);
        }

        public async Task<int> GetGamesByArmy(string teamName, string army)
        {
            var reports = await GetReports(teamName);
            var players = GetPlayerIds(teamName);
            return reports.Count(r => TeamUsedArmy(r, players, army));
        }

        public async Task<int> GetWinsByArmy(string teamName, string army)
        {
            var reports = await GetReports(teamName);
            var players = GetPlayerIds(teamName);
            return reports.Where(r => TeamUsedArmy(r, players, army))
                          .Count(r => DidTeamWin(r, players));
        }

        public async Task<double> GetWinRateOnMap(string teamName, string map)
        {
            var reports = (await GetReports(teamName)).Where(r => string.Equals(r.Map, map, StringComparison.OrdinalIgnoreCase)).ToList();
            var players = GetPlayerIds(teamName);
            int wins = reports.Count(r => DidTeamWin(r, players));
            return CalculateWinRate(wins, reports.Count);
        }

        public async Task<double> GetWinRateByDeployment(string teamName, string deployment)
        {
            var reports = (await GetReports(teamName)).Where(r => string.Equals(r.Deployment, deployment, StringComparison.OrdinalIgnoreCase)).ToList();
            var players = GetPlayerIds(teamName);
            int wins = reports.Count(r => DidTeamWin(r, players));
            return CalculateWinRate(wins, reports.Count);
        }

        public async Task<double> GetWinRateByPrimary(string teamName, string primary)
        {
            var reports = (await GetReports(teamName)).Where(r => string.Equals(r.PrimaryMission, primary, StringComparison.OrdinalIgnoreCase)).ToList();
            var players = GetPlayerIds(teamName);
            int wins = reports.Count(r => DidTeamWin(r, players));
            return CalculateWinRate(wins, reports.Count);
        }

        public async Task<string?> GetBestOpponentFaction(string teamName)
        {
            var reports = await GetReports(teamName);
            var players = GetPlayerIds(teamName);
            var stats = new Dictionary<string, (int Wins, int Games)>();

            foreach (var r in reports)
            {
                bool a = players.Contains(r.PlayerAId);
                bool b = players.Contains(r.PlayerBId);
                if (a && b) continue;
                string faction = a ? r.ListB : r.ListA;
                if (!stats.ContainsKey(faction)) stats[faction] = (0, 0);
                var val = stats[faction];
                if (DidTeamWin(r, players)) val.Wins++;
                val.Games++;
                stats[faction] = val;
            }

            if (stats.Count == 0) return null;
            return stats.OrderByDescending(kv => CalculateWinRate(kv.Value.Wins, kv.Value.Games)).First().Key;
        }

        public async Task<string?> GetWorstOpponentFaction(string teamName)
        {
            var reports = await GetReports(teamName);
            var players = GetPlayerIds(teamName);
            var stats = new Dictionary<string, (int Wins, int Games)>();

            foreach (var r in reports)
            {
                bool a = players.Contains(r.PlayerAId);
                bool b = players.Contains(r.PlayerBId);
                if (a && b) continue;
                string faction = a ? r.ListB : r.ListA;
                if (!stats.ContainsKey(faction)) stats[faction] = (0, 0);
                var val = stats[faction];
                if (DidTeamWin(r, players)) val.Wins++;
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

        public async Task<PlayerIdealScenarioDto> GetIdealScenario(string teamName, int top)
        {
            var reports = await GetReports(teamName);
            var players = GetPlayerIds(teamName);
            var opponents = new Dictionary<string, (int Wins, int Games)>();
            var maps = new Dictionary<string, (int Wins, int Games)>();
            var primaries = new Dictionary<string, (int Wins, int Games)>();
            var secondaries = new Dictionary<string, (int Wins, int Games)>();

            foreach (var r in reports)
            {
                bool a = players.Contains(r.PlayerAId);
                bool b = players.Contains(r.PlayerBId);
                if (a && b) continue;
                bool win = DidTeamWin(r, players);
                var sec = a ? r.SecondaryA : r.SecondaryB;
                var opp = a ? r.ListB : r.ListA;

                Accumulate(opponents, opp, win);
                Accumulate(maps, r.Map, win);
                Accumulate(primaries, r.PrimaryMission, win);
                Accumulate(secondaries, sec, win);
            }

            int limit = Math.Clamp(top, 0, 12);

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
