using WarApi.Models;
using WarApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WarApi.Repositories
{
    public class ArmyListRepository : IArmyListRepository
    {
        private readonly AppDbContext _context;

        public ArmyListRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ArmyList> AddAsync(ArmyList list)
        {
            list.Id = Guid.NewGuid();
            _context.ArmyLists.Add(list);
            await _context.SaveChangesAsync();
            return list;
        }

        public async Task<IEnumerable<ArmyList>> GetAllAsync()
        {
            return await _context.ArmyLists.ToListAsync();
        }

        public async Task<ArmyList?> GetByIdAsync(Guid id)
        {
            return await _context.ArmyLists.FindAsync(id);
        }
    }
}
