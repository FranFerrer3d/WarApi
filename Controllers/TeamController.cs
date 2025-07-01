using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatchReportNamespace.Services;
using WarApi.Services.Interfaces;

namespace WarApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TeamController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly IMatchReportService _reportService;

        public TeamController(IPlayerService playerService, IMatchReportService reportService)
        {
            _playerService = playerService;
            _reportService = reportService;
        }

        [HttpGet("{teamName}/players")]
        public ActionResult GetPlayers(string teamName)
        {
            var players = _playerService.GetAll().Where(p => p.Equipo == teamName);
            return Ok(players);
        }

        [HttpGet("{teamName}/reports")]
        public async Task<ActionResult> GetReports(string teamName)
        {
            var reports = await _reportService.GetReportsByTeam(teamName);
            return Ok(reports);
        }
    }
}
