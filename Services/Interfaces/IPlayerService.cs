using WarApi.Models;

namespace WarApi.Services.Interfaces
{
    public interface IPlayerService
    {
        IEnumerable<Player> GetAll();
        Player? GetById(Guid id);
        Player Create(Player jugador);
        bool Update(Guid id, Player jugador);
        bool Delete(Guid id);

        Player? Login(string email, string password);
    }
}
