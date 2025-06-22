using Microsoft.AspNetCore.Mvc;
using WarApi.Models;
using WarApi.Services.Interfaces;

namespace WarApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _PlayerService;

        public PlayerController(IPlayerService PlayerService)
        {
            _PlayerService = PlayerService;
        }

        // GET /jugadores
        [HttpGet("Login")]
        public ActionResult<bool> Login()
        {
            return Ok(_PlayerService.GetAll());
        }

        // GET /jugadores
        [HttpGet]
        public ActionResult<IEnumerable<Player>> GetAll()
        {
            return Ok(_PlayerService.GetAll());
        }

        // GET /jugadores/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Player> GetById(Guid id)
        {
            var jugador = _PlayerService.GetById(id);
            return jugador is null ? NotFound() : Ok(jugador);
        }

        // POST /jugadores
        [HttpPost]
        public ActionResult<Player> Create(Player nuevoJugador)
        {
            var creado = _PlayerService.Create(nuevoJugador);
            return CreatedAtAction(nameof(GetById), new { id = creado.ID }, creado);
        }

        // PUT /jugadores/{id}
        [HttpPut("{id:guid}")]
        public IActionResult Update(Guid id, Player jugador)
        {
            if (!_PlayerService.Update(id, jugador))
                return NotFound();

            return NoContent();
        }

        // DELETE /jugadores/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            if (!_PlayerService.Delete(id))
                return NotFound();

            return NoContent();
        }
    }
}
