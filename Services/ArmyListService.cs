using WarApi.Models;
using WarApi.Repositories.Interfaces;
using WarApi.Services.Interfaces;

namespace WarApi.Services
{
    public class ArmyListService : IArmyListService
    {
        private readonly IArmyListRepository _repository;

        public ArmyListService(IArmyListRepository repository)
        {
            _repository = repository;
        }

        public async Task<ArmyList> CreateAsync(ArmyList list)
        {
            return await _repository.AddAsync(list);
        }
    }
}
