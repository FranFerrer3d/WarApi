using Microsoft.AspNetCore.Mvc;
using WarApi.Models;
using WarApi.Services.Interfaces;

namespace WarApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListaController : ControllerBase
    {
        private readonly IListaService _service;

        public ListaController(IListaService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Lista>> GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id:guid}")]
        public ActionResult<Lista> GetById(Guid id)
        {
            var lista = _service.GetById(id);
            return lista is null ? NotFound() : Ok(lista);
        }

        [HttpPost]
        public ActionResult<Lista> Create(Lista lista)
        {
            var created = _service.Create(lista);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        public IActionResult Update(Guid id, Lista lista)
        {
            if (!_service.Update(id, lista))
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            if (!_service.Delete(id))
                return NotFound();
            return NoContent();
        }
    }
}
