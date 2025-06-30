using WarApi.Models;

namespace WarApi.Services.Interfaces
{
    public interface IPlayerService
    {
        IEnumerable<Player> GetAll(bool includeSensitive = false);
        Player? GetById(Guid id);
        Player Create(Player jugador);
        bool Update(Guid id, Player jugador);
        bool Delete(Guid id);

        IEnumerable<Player> GetAllRaw();

        Player? Login(string email, string password);
    }
}
