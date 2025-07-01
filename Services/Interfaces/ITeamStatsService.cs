using WarApi.Dtos;

namespace WarApi.Services.Interfaces
{
    public interface ITeamStatsService
    {
        Task<int> GetTotalGames(string teamName);
        Task<int> GetWins(string teamName);
        Task<int> GetLosses(string teamName);
        Task<int> GetDraws(string teamName);
        Task<int> GetGamesByArmy(string teamName, string army);
        Task<int> GetWinsByArmy(string teamName, string army);
        Task<double> GetWinRateOnMap(string teamName, string map);
        Task<double> GetWinRateByDeployment(string teamName, string deployment);
        Task<double> GetWinRateByPrimary(string teamName, string primary);
        Task<string?> GetBestOpponentFaction(string teamName);
        Task<string?> GetWorstOpponentFaction(string teamName);
        Task<PlayerIdealScenarioDto> GetIdealScenario(string teamName, int top);
    }
}
