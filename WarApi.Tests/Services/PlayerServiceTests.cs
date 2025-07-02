using System;
using System.Linq;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Identity;
using WarApi.Models;
using WarApi.Repositories.Interfaces;
using WarApi.Services;
using WarApi.Services.Security;

namespace WarApi.Tests.Services
{
    public class PlayerServiceTests
    {
        private PlayerService CreateService(out Mock<IPlayerRepository> repoMock, Player storedPlayer)
        {
            repoMock = new Mock<IPlayerRepository>();
            repoMock.Setup(r => r.GetAll(true)).Returns(new[] { storedPlayer });
            repoMock.Setup(r => r.GetById(storedPlayer.ID)).Returns(storedPlayer);

            var encryption = new Base64EncryptionService();
            var hasher = new PasswordHasher<Player>();
            return new PlayerService(repoMock.Object, encryption, hasher);
        }

        [Fact]
        public void Login_ReturnsPlayer_WhenPasswordMatches()
        {
            var player = new Player { ID = Guid.NewGuid(), Email = "test@example.com", Nombre = "John" };
            var encryption = new Base64EncryptionService();
            player.Nombre = encryption.Encrypt(player.Nombre);
            var hasher = new PasswordHasher<Player>();
            player.Contraseña = hasher.HashPassword(player, "secret");

            var service = CreateService(out _, player);

            var result = service.Login("test@example.com", "secret");

            Assert.NotNull(result);
            Assert.Equal("John", result!.Nombre);
        }

        [Fact]
        public void Login_ReturnsNull_WhenPasswordDoesNotMatch()
        {
            var player = new Player { ID = Guid.NewGuid(), Email = "test@example.com", Nombre = "John" };
            var encryption = new Base64EncryptionService();
            player.Nombre = encryption.Encrypt(player.Nombre);
            var hasher = new PasswordHasher<Player>();
            player.Contraseña = hasher.HashPassword(player, "secret");

            var service = CreateService(out _, player);

            var result = service.Login("test@example.com", "wrong");

            Assert.Null(result);
        }

        [Fact]
        public void Create_EncryptsSensitiveFields_AndHashesPassword()
        {
            var repoMock = new Mock<IPlayerRepository>();
            var encryption = new Base64EncryptionService();
            var hasher = new PasswordHasher<Player>();
            var service = new PlayerService(repoMock.Object, encryption, hasher);

            var player = new Player
            {
                Nombre = "John",
                Apellidos = "Doe",
                Alias = "JD",
                Equipo = "TeamA",
                Email = "test@example.com",
                Contraseña = "secret"
            };

            service.Create(player);

            repoMock.Verify(r => r.Add(It.Is<Player>(p =>
                p.Nombre == encryption.Encrypt("John") &&
                p.Apellidos == encryption.Encrypt("Doe") &&
                p.Alias == encryption.Encrypt("JD") &&
                p.Equipo == encryption.Encrypt("TeamA") &&
                hasher.VerifyHashedPassword(p, p.Contraseña, "secret") == PasswordVerificationResult.Success
            )), Times.Once);
        }
    }
}
