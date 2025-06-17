using System.Xml.Linq;
using WarApi.Models;
using WarApi.Repositories.Interfaces;

namespace WarApi.Repositories
{
    public class PlayerRepository:IPlayerRepository
    {
        private static readonly List<Player> _jugadores = new();

        public IEnumerable<Player> GetAll() => _jugadores;

        public Player? GetById(Guid id) =>
            _jugadores.FirstOrDefault(j => j.ID == id);

        public void Add(Player jugador)
        {
            jugador.ID = Guid.NewGuid();
            _jugadores.Add(jugador);
        }

        public void Update(Player jugador)
        {
            var index = _jugadores.FindIndex(j => j.ID == jugador.ID);
            if (index != -1)
                _jugadores[index] = jugador;
        }

        public void Delete(Guid id)
        {
            var jugador = GetById(id);
            if (jugador != null)
                _jugadores.Remove(jugador);
        }

    }
}
