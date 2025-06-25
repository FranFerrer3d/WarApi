using System;
using System.Threading.Tasks;
using MatchReportNamespace.Services;
using Microsoft.AspNetCore.Mvc;
using WarApi.Dtos;
using WarApi.Services.Interfaces;
using WarApi.Models;

namespace MatchReportNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchReportsController : ControllerBase
    {
        private readonly IMatchReportService _service;
        private readonly IPlayerService _Playerservice;
        private readonly IArmyListService _listService;

        public MatchReportsController(IMatchReportService service, IPlayerService PlayerService, IArmyListService listService)
        {
            _service = service;
            _Playerservice = PlayerService;
            _listService = listService;
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

        [HttpGet("GetByPlayerId")]
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

            string ExtractFaction(string list)
            {
                if (string.IsNullOrWhiteSpace(list)) return string.Empty;
                var lines = list.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                return lines.Length > 0 ? lines[0].Trim() : string.Empty;
            }

            var factionA = string.IsNullOrWhiteSpace(dto.ArmyA) ? ExtractFaction(dto.ListA) : dto.ArmyA;
            var factionB = string.IsNullOrWhiteSpace(dto.ArmyB) ? ExtractFaction(dto.ListB) : dto.ArmyB;

            await _listService.CreateAsync(new ArmyList
            {
                PlayerId = dto.PlayerAId,
                Faction = factionA,
                Content = dto.ListA
            });

            await _listService.CreateAsync(new ArmyList
            {
                PlayerId = dto.PlayerBId,
                Faction = factionB,
                Content = dto.ListB
            });

            var report = new MatchReport
            {
                Id = Guid.NewGuid(),
                PlayerAId = dto.PlayerAId,
                PlayerBId = dto.PlayerBId,
                PlayerA = playerA,
                PlayerB = playerB,
                ListA = dto.ListA,
                ListB = dto.ListB,
                ArmyA = factionA,
                ArmyB = factionB,
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
                SecondaryWinB = dto.SecondaryWinB,
                FinalScoreA = dto.FinalScoreA,
                FinalScoreB = dto.FinalScoreB
            };

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
