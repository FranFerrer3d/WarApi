using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarApi.Models;
using WarApi.Models.DTO;
using WarApi.Services.Interfaces;
using WarApi.Services.Security;

namespace WarApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _PlayerService;
        private readonly ITokenService _tokenService;

        public PlayerController(IPlayerService PlayerService, ITokenService tokenService)
        {
            _PlayerService = PlayerService;
            _tokenService = tokenService;
        }

        [HttpPost("Login")]
        public ActionResult<LoginResponseDto> Login(string user, string pass)
        {
            var player = _PlayerService.Login(user, pass);
            if (player == null) return Unauthorized();
            var token = _tokenService.CreateToken(player);
            return Ok(new LoginResponseDto { User = player, Token = token });
        }

        // GET /jugadores
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<Player>> GetAll()
        {
            return Ok(_PlayerService.GetAll());
        }

        [HttpGet("admin/raw")]
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult<IEnumerable<Player>> GetAllRaw()
            => Ok(_PlayerService.GetAllRaw());

        // GET /jugadores/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Player> GetById(Guid id)
        {
            var jugador = _PlayerService.GetById(id);
            return jugador is null ? NotFound() : Ok(jugador);
        }

        // GET /jugadores
        [HttpGet("GetByEmail")]
        public ActionResult<Player> GetByEmail(string email)
        {
            var jugador = _PlayerService.GetAll().Where(x=>x.Email == email).FirstOrDefault();
            return jugador is null ? NotFound() : Ok(jugador);
        }

        // POST /jugadores
        [HttpPost]
        public ActionResult<Player> Create(Player nuevoJugador)
        {
            var creado = _PlayerService.Create(nuevoJugador);
            return CreatedAtAction(nameof(GetById), new { id = creado.ID }, creado);
        }

        // PUT
        [HttpPut]
        [Authorize]
        public IActionResult Update(Player jugador)
        {
            if (!_PlayerService.Update(jugador.ID, jugador))
                return NotFound();

            return NoContent();
        }

        // DELETE /jugadores/{id}
        [HttpDelete("{id:guid}")]
        [Authorize]
        public IActionResult Delete(Guid id)
        {
            if (!_PlayerService.Delete(id))
                return NotFound();

            return NoContent();
        }
    }
}
