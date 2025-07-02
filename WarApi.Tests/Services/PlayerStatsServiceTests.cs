using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using MatchReportNamespace;
using MatchReportNamespace.Services;
using WarApi.Services;
using WarApi.Services.Interfaces;
using WarApi.Dtos;

namespace WarApi.Tests.Services
{
    public class PlayerStatsServiceTests
    {
        private static MatchReport CreateReport(Guid playerId, bool asPlayerA, int scoreA, int scoreB, string map = "Map1")
        {
            return new MatchReport
            {
                Id = Guid.NewGuid(),
                PlayerAId = asPlayerA ? playerId : Guid.NewGuid(),
                PlayerBId = asPlayerA ? Guid.NewGuid() : playerId,
                ListA = "ArmyA",
                ListB = "ArmyB",
                Map = map,
                FinalScoreA = scoreA,
                FinalScoreB = scoreB
            };
        }

        [Fact]
        public async Task GetTotalGames_ReturnsNumberOfReports()
        {
            var playerId = Guid.NewGuid();
            var reports = new List<MatchReport>
            {
                CreateReport(playerId, true, 10, 5),
                CreateReport(playerId, false, 7, 12)
            };

            var service = SetupService(playerId, reports);
            var total = await service.GetTotalGames(playerId);
            Assert.Equal(2, total);
        }

        [Fact]
        public async Task GetWins_ReturnsOnlyWins()
        {
            var playerId = Guid.NewGuid();
            var reports = new List<MatchReport>
            {
                CreateReport(playerId, true, 15, 10), // win
                CreateReport(playerId, false, 8, 12)  // loss
            };

            var service = SetupService(playerId, reports);
            var wins = await service.GetWins(playerId);
            Assert.Equal(1, wins);
        }

        [Fact]
        public async Task GetWinRateOnMap_CalculatesCorrectPercentage()
        {
            var playerId = Guid.NewGuid();
            var reports = new List<MatchReport>
            {
                CreateReport(playerId, true, 10, 5, "Map1"),  // win
                CreateReport(playerId, true, 8, 12, "Map1"),  // loss
                CreateReport(playerId, false, 7, 3, "Map2")    // win but different map
            };

            var service = SetupService(playerId, reports);
            var rate = await service.GetWinRateOnMap(playerId, "Map1");
            Assert.Equal(50d, rate);
        }

        private static PlayerStatsService SetupService(Guid playerId, List<MatchReport> reports)
        {
            var mock = new Mock<IMatchReportService>();
            mock.Setup(m => m.GetReportsByUser(playerId)).ReturnsAsync(reports);
            return new PlayerStatsService(mock.Object);
        }
    }
}
