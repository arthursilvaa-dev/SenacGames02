using Microsoft.AspNetCore.Mvc;
using SenacGames.Application.Interfaces;
using SenacGames.Application.ViewModels;
using SenacGames.Domain.Entities;

namespace SenacGames.UI.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ICategoryService _categoryService;

        public GamesController(IGameService gameService, ICategoryService categoryService)
        {
            _gameService = gameService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            var viewModel = new GameListViewModel
            {
                Categories = await _categoryService.GetAllAsync(),
                SelectedCategoryId = categoryId
            };

            // Se uma categoria foi selecionada, filtra os games
            if (categoryId.HasValue)
            {
                viewModel.Games = await _gameService.GetByCategoryAsync(categoryId.Value);
            }
            else
            {
                viewModel.Games = await _gameService.GetAllAsync();
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var game = await _gameService.GetByIdAsync(id);

            if (game == null)
                return NotFound();

            // Busca games relacionados (mesma categoria)
            var relatedGames = await _gameService.GetByCategoryAsync(game.CategoryId);

            var viewModel = new GameDetailsViewModel
            {
                Game = game,
                RelatedGames = relatedGames.Where(g => g.Id != game.Id).Take(4)
            };

            return View(viewModel);
        }
    }
}
