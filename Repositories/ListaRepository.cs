using Microsoft.EntityFrameworkCore;
using WarApi.Models;
using WarApi.Repositories.Interfaces;

namespace WarApi.Repositories
{
    public class ListaRepository : IListaRepository
    {
        private readonly AppDbContext _context;
        public ListaRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Lista> GetAll() =>
            _context.Listas.Include(l => l.Player).ToList();

        public Lista? GetById(Guid id) =>
            _context.Listas.Include(l => l.Player).FirstOrDefault(l => l.Id == id);

        public void Add(Lista lista)
        {
            lista.Id = Guid.NewGuid();
            _context.Listas.Add(lista);
            _context.SaveChanges();
        }

        public void Update(Lista lista)
        {
            _context.Listas.Update(lista);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var lista = _context.Listas.Find(id);
            if (lista != null)
            {
                _context.Listas.Remove(lista);
                _context.SaveChanges();
            }
        }
    }
}
