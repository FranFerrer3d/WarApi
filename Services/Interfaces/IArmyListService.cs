using WarApi.Models;

namespace WarApi.Services.Interfaces
{
    public interface IArmyListService
    {
        Task<ArmyList> CreateAsync(ArmyList list);
    }
}
