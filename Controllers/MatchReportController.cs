using System;
using System.Threading.Tasks;
using MatchReportNamespace.Services;
using Microsoft.AspNetCore.Mvc;
using WarApi.Dtos;
using WarApi.Services.Interfaces;

namespace MatchReportNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchReportsController : ControllerBase
    {
        private readonly IMatchReportService _service;
        private readonly IPlayerService _Playerservice;

        public MatchReportsController(IMatchReportService service,IPlayerService PlayerService)
        {
            _service = service;
            _Playerservice = PlayerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var report = await _service.GetByIdAsync(id);
            return report == null ? NotFound() : Ok(report);
        }

        [HttpGet("{playerId}")]
        public async Task<IActionResult> GetByPlayerId(Guid playerId)
        {
            var reports = await _service.GetReportsByUser(playerId);
            return reports == null ? NotFound() : Ok(reports);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MatchReportCreateDto dto)
        {
            var playerA = _Playerservice.GetById(dto.PlayerAId);
            var playerB = _Playerservice.GetById(dto.PlayerBId);

            if (playerA == null || playerB == null)
                return BadRequest("One or both players not found");

            var report = new MatchReport
            {
                Id = Guid.NewGuid(),
                PlayerAId = dto.PlayerAId,
                PlayerBId = dto.PlayerBId,
                PlayerA = playerA,
                PlayerB = playerB,
                ListA = dto.ListA,
                ListB = dto.ListB,
                ExpectedA = dto.ExpectedA,
                ExpectedB = dto.ExpectedB,
                Date = dto.Date,
                Map = dto.Map,
                Deployment = dto.Deployment,
                PrimaryMission = dto.PrimaryMission,
                SecondaryA = dto.SecondaryA,
                SecondaryB = dto.SecondaryB,
                MagicA = dto.MagicA,
                MagicB = dto.MagicB,
                KillsA = dto.KillsA,
                KillsB = dto.KillsB,
                PrimaryResult = Enum.TryParse<PrimaryWinner>(dto.PrimaryResult, out var result) ? result : PrimaryWinner.None,
                SecondaryWinA = dto.SecondaryWinA,
                SecondaryWinB = dto.SecondaryWinB
            };

            report.CalculateFinalScore();

            await _service.CreateAsync(report);

            return CreatedAtAction(nameof(GetById), new { id = report.Id }, report);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] MatchReport report)
        {
            await _service.UpdateAsync(id, report);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
