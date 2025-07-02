using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using MatchReportNamespace;
using MatchReportNamespace.Services;
using WarApi.Services;
using WarApi.Services.Interfaces;
using WarApi.Models;

namespace WarApi.Tests.Services
{
    public class TeamStatsServiceTests
    {
        private static MatchReport CreateReport(Guid aId, Guid bId, int scoreA, int scoreB, string map = "Map1")
        {
            return new MatchReport
            {
                Id = Guid.NewGuid(),
                PlayerAId = aId,
                PlayerBId = bId,
                ListA = "ArmyA",
                ListB = "ArmyB",
                Map = map,
                FinalScoreA = scoreA,
                FinalScoreB = scoreB
            };
        }

        [Fact]
        public async Task GetWins_CountsWinsForTeam()
        {
            var team = "TeamA";
            var player1 = new Player { ID = Guid.NewGuid(), Equipo = team };
            var player2 = new Player { ID = Guid.NewGuid(), Equipo = team };
            var other = new Player { ID = Guid.NewGuid(), Equipo = "X" };

            var reports = new List<MatchReport>
            {
                CreateReport(player1.ID, other.ID, 12, 8), // win
                CreateReport(other.ID, player1.ID, 7, 15), // win
                CreateReport(other.ID, other.ID, 10, 5)    // not team
            };

            var matchMock = new Mock<IMatchReportService>();
            matchMock.Setup(m => m.GetReportsByTeam(team)).ReturnsAsync(reports);

            var playerMock = new Mock<IPlayerService>();
            playerMock.Setup(p => p.GetAll()).Returns(new[] { player1, player2, other });

            var service = new TeamStatsService(matchMock.Object, playerMock.Object);
            var wins = await service.GetWins(team);

            Assert.Equal(2, wins);
        }
    }
}
