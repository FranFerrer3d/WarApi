using WarApi.Models;

namespace WarApi.Services.Interfaces
{
    public interface IListaService
    {
        IEnumerable<Lista> GetAll();
        Lista? GetById(Guid id);
        Lista Create(Lista lista);
        bool Update(Guid id, Lista lista);
        bool Delete(Guid id);
    }
}
