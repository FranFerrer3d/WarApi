using MatchReportNamespace;
using WarApi.Dtos;


namespace WarApi.Services.Interfaces
{
    public interface IPlayerStatsService
    {
        Task<int> GetTotalGames(Guid playerId);
        Task<int> GetWins(Guid playerId);
        Task<int> GetLosses(Guid playerId);
        Task<int> GetDraws(Guid playerId);
        Task<int> GetGamesByArmy(Guid playerId, string army);
        Task<int> GetWinsByArmy(Guid playerId, string army);
        Task<double> GetWinRateOnMap(Guid playerId, string map);
        Task<double> GetWinRateByDeployment(Guid playerId, string deployment);
        Task<double> GetWinRateByPrimary(Guid playerId, string primary);
        Task<string?> GetBestOpponentFaction(Guid playerId);
        Task<string?> GetWorstOpponentFaction(Guid playerId);
        Task<PlayerIdealScenarioDto> GetIdealScenario(Guid playerId, int top);
    }
}
