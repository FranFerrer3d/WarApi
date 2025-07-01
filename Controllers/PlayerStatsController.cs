using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WarApi.Services.Interfaces;

using WarApi.Dtos;


namespace WarApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlayerStatsController : ControllerBase
    {
        private readonly IPlayerStatsService _statsService;

        public PlayerStatsController(IPlayerStatsService statsService)
        {
            _statsService = statsService;
        }

        [HttpGet("{playerId}/total")]
        public async Task<ActionResult<int>> GetTotal(Guid playerId)
            => Ok(await _statsService.GetTotalGames(playerId));

        [HttpGet("{playerId}/wins")]
        public async Task<ActionResult<int>> GetWins(Guid playerId)
            => Ok(await _statsService.GetWins(playerId));

        [HttpGet("{playerId}/losses")]
        public async Task<ActionResult<int>> GetLosses(Guid playerId)
            => Ok(await _statsService.GetLosses(playerId));

        [HttpGet("{playerId}/draws")]
        public async Task<ActionResult<int>> GetDraws(Guid playerId)
            => Ok(await _statsService.GetDraws(playerId));

        [HttpGet("{playerId}/army/{army}/total")]
        public async Task<ActionResult<int>> GetGamesByArmy(Guid playerId, string army)
            => Ok(await _statsService.GetGamesByArmy(playerId, army));

        [HttpGet("{playerId}/army/{army}/wins")]
        public async Task<ActionResult<int>> GetWinsByArmy(Guid playerId, string army)
            => Ok(await _statsService.GetWinsByArmy(playerId, army));

        [HttpGet("{playerId}/map/{map}/winrate")]
        public async Task<ActionResult<double>> GetMapWinRate(Guid playerId, string map)
            => Ok(await _statsService.GetWinRateOnMap(playerId, map));

        [HttpGet("{playerId}/deployment/{deployment}/winrate")]
        public async Task<ActionResult<double>> GetDeploymentWinRate(Guid playerId, string deployment)
            => Ok(await _statsService.GetWinRateByDeployment(playerId, deployment));

        [HttpGet("{playerId}/primary/{mission}/winrate")]
        public async Task<ActionResult<double>> GetPrimaryWinRate(Guid playerId, string mission)
            => Ok(await _statsService.GetWinRateByPrimary(playerId, mission));

        [HttpGet("{playerId}/best-opponent")]
        public async Task<ActionResult<string?>> GetBestOpponent(Guid playerId)
            => Ok(await _statsService.GetBestOpponentFaction(playerId));

        [HttpGet("{playerId}/worst-opponent")]
        public async Task<ActionResult<string?>> GetWorstOpponent(Guid playerId)
            => Ok(await _statsService.GetWorstOpponentFaction(playerId));

        [HttpGet("{playerId}/ideal-scenario/{top?}")]
        public async Task<ActionResult<PlayerIdealScenarioDto>> GetIdealScenario(Guid playerId, int top = 1)
            => Ok(await _statsService.GetIdealScenario(playerId, top));

    }
}
