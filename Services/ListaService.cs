using WarApi.Models;
using WarApi.Repositories.Interfaces;
using WarApi.Services.Interfaces;

namespace WarApi.Services
{
    public class ListaService : IListaService
    {
        private readonly IListaRepository _repository;

        public ListaService(IListaRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Lista> GetAll() => _repository.GetAll();

        public Lista? GetById(Guid id) => _repository.GetById(id);

        public Lista Create(Lista lista)
        {
            _repository.Add(lista);
            return lista;
        }

        public bool Update(Guid id, Lista lista)
        {
            var existente = _repository.GetById(id);
            if (existente == null) return false;
            lista.Id = id;
            _repository.Update(lista);
            return true;
        }

        public bool Delete(Guid id)
        {
            var existente = _repository.GetById(id);
            if (existente == null) return false;
            _repository.Delete(id);
            return true;
        }
    }
}
