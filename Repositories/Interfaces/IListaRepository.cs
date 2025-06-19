using WarApi.Models;

namespace WarApi.Repositories.Interfaces
{
    public interface IListaRepository
    {
        IEnumerable<Lista> GetAll();
        Lista? GetById(Guid id);
        void Add(Lista lista);
        void Update(Lista lista);
        void Delete(Guid id);
    }
}
