using WarApi.Models;

namespace WarApi.Repositories.Interfaces
{
    public interface IArmyListRepository
    {
        Task<ArmyList> AddAsync(ArmyList list);
        Task<IEnumerable<ArmyList>> GetAllAsync();
        Task<ArmyList?> GetByIdAsync(Guid id);
    }
}
