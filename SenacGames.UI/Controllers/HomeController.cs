using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SenacGames.Application.Interfaces;
using SenacGames.Application.ViewModels;
using SenacGames.UI.Models;

namespace SenacGames.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ICategoryService _categoryService;

        // Injeção de dependência no Controller.
        public HomeController(IGameService gameService, ICategoryService categoryService)
        {
            _gameService = gameService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel
            {
                FeaturedGames = await _gameService.GetFeaturedAsync(),
                Categories = await _categoryService.GetAllAsync(),
                RecentGames = await _gameService.GetAllAsync(),
            };
            return View(viewModel);
        }
    }
}
