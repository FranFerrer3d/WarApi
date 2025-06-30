using Microsoft.AspNetCore.Identity;
using WarApi.Models;
using WarApi.Repositories.Interfaces;
using WarApi.Services.Interfaces;
using WarApi.Services.Security;

namespace WarApi.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _repository;
        private readonly IEncryptionService _encryption;
        private readonly PasswordHasher<Player> _hasher;

        public PlayerService(IPlayerRepository repository,
                             IEncryptionService encryption,
                             PasswordHasher<Player> hasher)
        {
            _repository = repository;
            _encryption = encryption;
            _hasher = hasher;
        }

        private Player Decode(Player player, bool includePassword)
        {
            player.Nombre = _encryption.Decrypt(player.Nombre);
            player.Apellidos = _encryption.Decrypt(player.Apellidos);
            player.Alias = _encryption.Decrypt(player.Alias);
            player.Equipo = _encryption.Decrypt(player.Equipo);
            if (!includePassword)
                player.Contraseña = null;
            return player;
        }

        public Player? Login(string email, string password)
        {
            var player = _repository.GetAll(true).SingleOrDefault(p => p.Email == email);
            if (player == null) return null;

            var result = _hasher.VerifyHashedPassword(player, player.Contraseña, password);
            if (result == PasswordVerificationResult.Success)
                return Decode(player, false);

            return null;
        }

        public IEnumerable<Player> GetAll(bool includeSensitive = false)
        {
            return _repository.GetAll(includeSensitive)
                .Select(p => Decode(p, includeSensitive));
        }

        public IEnumerable<Player> GetAllRaw()
        {
            return _repository.GetAll(true).Select(p => Decode(p, true));
        }

        public Player? GetById(Guid id)
        {
            var player = _repository.GetById(id);
            return player == null ? null : Decode(player, false);
        }

        public Player Create(Player jugador)
        {
            jugador.Nombre = _encryption.Encrypt(jugador.Nombre);
            jugador.Apellidos = _encryption.Encrypt(jugador.Apellidos);
            jugador.Alias = _encryption.Encrypt(jugador.Alias);
            jugador.Equipo = _encryption.Encrypt(jugador.Equipo);
            jugador.Contraseña = _hasher.HashPassword(jugador, jugador.Contraseña);
            _repository.Add(jugador);
            return Decode(jugador, false);
        }

        public bool Update(Guid id, Player jugador)
        {
            jugador.Nombre = _encryption.Encrypt(jugador.Nombre);
            jugador.Apellidos = _encryption.Encrypt(jugador.Apellidos);
            jugador.Alias = _encryption.Encrypt(jugador.Alias);
            jugador.Equipo = _encryption.Encrypt(jugador.Equipo);
            if (!string.IsNullOrEmpty(jugador.Contraseña))
                jugador.Contraseña = _hasher.HashPassword(jugador, jugador.Contraseña);
            _repository.Update(jugador);
            return true;
        }

        public bool Delete(Guid id)
        {
            var jugador = _repository.GetById(id);
            if (jugador == null) return false;

            _repository.Delete(id);
            return true;
        }
    }
}
