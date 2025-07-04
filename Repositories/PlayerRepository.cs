using WarApi.Models;
using WarApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace WarApi.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly AppDbContext _context;

        public PlayerRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Player> GetAll(bool forcePassword = false)
        {
            var res = _context.Players
                .AsNoTracking()
                .ToList();
            if (!forcePassword) {
                foreach (var player in res)
                {
                    player.Contraseña = null;
                }
            }
            return res;
        }

        public Player? GetById(Guid id) =>
            _context.Players
                .AsNoTracking()
                .FirstOrDefault(p => p.ID == id);

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
