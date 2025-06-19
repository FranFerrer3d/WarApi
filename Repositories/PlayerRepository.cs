using WarApi.Models;
using WarApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WarApi.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly AppDbContext _context;

        public PlayerRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Player> GetAll() =>
            _context.Players.ToList();

        public Player? GetById(Guid id) =>
            _context.Players.Find(id);

        public void Add(Player player)
        {
            player.ID = Guid.NewGuid();
            _context.Players.Add(player);
            _context.SaveChanges();
        }

        public void Update(Player player)
        {
            _context.Players.Update(player);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var player = _context.Players.Find(id);
            if (player != null)
            {
                _context.Players.Remove(player);
                _context.SaveChanges();
            }
        }
    }
}
