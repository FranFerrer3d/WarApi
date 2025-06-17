using WarApi.Models;

namespace WarApi.Repositories.Interfaces
{
    public interface IPlayerRepository
    {
        IEnumerable<Player> GetAll();
        Player? GetById(Guid id);
        void Add(Player jugador);
        void Update(Player jugador);
        void Delete(Guid id);
    }
}
