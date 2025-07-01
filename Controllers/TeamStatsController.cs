using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarApi.Services.Interfaces;
using WarApi.Dtos;

namespace WarApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TeamStatsController : ControllerBase
    {
        private readonly ITeamStatsService _statsService;

        public TeamStatsController(ITeamStatsService statsService)
        {
            _statsService = statsService;
        }

        [HttpGet("{team}/total")]
        public async Task<ActionResult<int>> GetTotal(string team)
            => Ok(await _statsService.GetTotalGames(team));

        [HttpGet("{team}/wins")]
        public async Task<ActionResult<int>> GetWins(string team)
            => Ok(await _statsService.GetWins(team));

        [HttpGet("{team}/losses")]
        public async Task<ActionResult<int>> GetLosses(string team)
            => Ok(await _statsService.GetLosses(team));

        [HttpGet("{team}/draws")]
        public async Task<ActionResult<int>> GetDraws(string team)
            => Ok(await _statsService.GetDraws(team));

        [HttpGet("{team}/army/{army}/total")]
        public async Task<ActionResult<int>> GetGamesByArmy(string team, string army)
            => Ok(await _statsService.GetGamesByArmy(team, army));

        [HttpGet("{team}/army/{army}/wins")]
        public async Task<ActionResult<int>> GetWinsByArmy(string team, string army)
            => Ok(await _statsService.GetWinsByArmy(team, army));

        [HttpGet("{team}/map/{map}/winrate")]
        public async Task<ActionResult<double>> GetMapWinRate(string team, string map)
            => Ok(await _statsService.GetWinRateOnMap(team, map));

        [HttpGet("{team}/deployment/{deployment}/winrate")]
        public async Task<ActionResult<double>> GetDeploymentWinRate(string team, string deployment)
            => Ok(await _statsService.GetWinRateByDeployment(team, deployment));

        [HttpGet("{team}/primary/{mission}/winrate")]
        public async Task<ActionResult<double>> GetPrimaryWinRate(string team, string mission)
            => Ok(await _statsService.GetWinRateByPrimary(team, mission));

        [HttpGet("{team}/best-opponent")]
        public async Task<ActionResult<string?>> GetBestOpponent(string team)
            => Ok(await _statsService.GetBestOpponentFaction(team));

        [HttpGet("{team}/worst-opponent")]
        public async Task<ActionResult<string?>> GetWorstOpponent(string team)
            => Ok(await _statsService.GetWorstOpponentFaction(team));

        [HttpGet("{team}/ideal-scenario/{top?}")]
        public async Task<ActionResult<PlayerIdealScenarioDto>> GetIdealScenario(string team, int top = 1)
            => Ok(await _statsService.GetIdealScenario(team, top));
    }
}
