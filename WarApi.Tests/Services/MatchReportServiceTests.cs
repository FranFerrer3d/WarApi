using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using MatchReportNamespace.Repositories;
using WarApi.Services;
using WarApi.Services.Interfaces;
using WarApi.Models;

namespace WarApi.Tests.Services
{
    public class MatchReportServiceTests
    {
        [Fact]
        public async Task GetReportsByUser_ReturnsOnlyUserReports()
        {
            var userId = Guid.NewGuid();
            var reports = new List<MatchReport>
            {
                new MatchReport { Id = Guid.NewGuid(), PlayerAId = userId, PlayerBId = Guid.NewGuid() },
                new MatchReport { Id = Guid.NewGuid(), PlayerAId = Guid.NewGuid(), PlayerBId = userId },
                new MatchReport { Id = Guid.NewGuid(), PlayerAId = Guid.NewGuid(), PlayerBId = Guid.NewGuid() }
            };

            var repoMock = new Mock<IMatchReportRepository>();
            repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(reports);

            var playerMock = new Mock<IPlayerService>();
            playerMock.Setup(p => p.GetById(It.IsAny<Guid>())).Returns(new Player());

            var service = new MatchReportService(repoMock.Object, playerMock.Object);
            var result = await service.GetReportsByUser(userId);

            Assert.Equal(2, result.Count);
            Assert.All(result, r => Assert.True(r.PlayerAId == userId || r.PlayerBId == userId));
        }
    }
}
