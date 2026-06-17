using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SenacGames.Application.DTOs;
using SenacGames.Application.Interfaces;

namespace SenacGames.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : Controller
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        /// <summary>
        /// Obtém a lista de todos os jogos cadastrados.
        /// GET api/games
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetAll()
        {
            var games = await _gameService.GetAllAsync();
            return Ok(games);
        }

        /// <summary>
        /// Busca game por Id específico
        /// GET api/games/{id}
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetById(int id)
        {
            var game = await _gameService.GetByIdAsync(id);

            if (game == null)
                return NotFound(new { message = "Game não encontrado." });

            return Ok(game);
        }

        /// <summary>
        /// Cria um novo game
        /// POST api/games
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GameDto>> Create([FromBody] CreateGameDto dto)
        {
            var game = await _gameService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = game.Id }, game);
        }


        /// <summary>
        /// Atualiza um game existente
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GameDto>> Update(int id, [FromBody] UpdateGameDto dto)
        {
            var game = await _gameService.UpdateAsync(id, dto);

            if (game == null)
                return NotFound(new { message = "Game não encontrado." });

            return Ok(game);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _gameService.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = "Game não encontrado" });

            return NoContent(); // 204 No Content é a resposta padrão para deleção bem-sucedida sem retornar dados


        }



    }
}
