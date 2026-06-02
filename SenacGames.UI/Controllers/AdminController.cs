using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SenacGames.Application.Interfaces;
using SenacGames.Application.ViewModels;

namespace SenacGames.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ICategoryService _categoryService;

        public AdminController(IGameService gameService, ICategoryService categoryService)
        {
            _gameService = gameService;
            _categoryService = categoryService;
        }

        // ==========================================
        // DASHBOARD ADMINISTRATIVO
        // ==========================================
        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "Dashboard";
            ViewData["Title"] = "Dashboard";
            ViewData["Subtitle"] = "Resumo do sistema SenacGames";

            var viewModel = new DashboardViewModel
            {
                TotalGames = await _gameService.CountAsync(),
                TotalCategories = await _categoryService.CountAsync(),
                FeaturedGames = (await _gameService.GetFeaturedAsync()).Count(),
                RecentGames = (await _gameService.GetAllAsync()).Take(5)
            };
            return View(viewModel);
        }

        // ==========================================
        // CRUD DE GAMES
        // ==========================================

        public async Task<IActionResult> Games()
        {
            ViewData["ActiveMenu"] = "Games";
            ViewData["Title"] = "Gerenciar Games";
            ViewData["Subtitle"] = "Cadastre, edite e exclua games do catálogo";

            var games = await _gameService.GetAllAsync();
            return View(games);
        }

        /// <summary>
        /// Formulário para criação de um novo game.
        /// GET : /Admin/CreateGame
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CreateGame()
        {
            ViewData["ActiveMenu"] = "Games";
            ViewData["Title"] = "Cadastrar Novo Game";

            var categories = await _categoryService.GetAllAsync();
            var viewModel = new GameFormViewModel
            {
                Categories = categories,
                ReleaseYear = DateTime.Now.Year
            };

            return View(viewModel);
        }
    }
}
