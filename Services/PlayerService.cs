using WarApi.Models;
using WarApi.Repositories.Interfaces;
using WarApi.Services.Interfaces;

namespace WarApi.Services
{
    public class PlayerService:IPlayerService
    {
        private readonly IPlayerRepository _repository;

        public PlayerService(IPlayerRepository repository)
        {
            _repository = repository;
        }

        public bool Login(string email, string password)
        {
            return _repository.GetAll(true).Any(player => player.Email == email && player.Contraseña == password);
        }


        public IEnumerable<Player> GetAll() => _repository.GetAll();

        public Player? GetById(Guid id) => _repository.GetById(id);

        public Player Create(Player jugador)
        {
            _repository.Add(jugador);
            return jugador;
        }

        public bool Update(Guid id, Player jugador)
        {
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
