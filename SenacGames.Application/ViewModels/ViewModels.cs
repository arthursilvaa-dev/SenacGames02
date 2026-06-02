using System;
using System.Collections.Generic;
using System.Text;
using SenacGames.Application.DTOs;

namespace SenacGames.Application.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<GameDto> FeaturedGames { get; set; } = new List<GameDto>();
        public IEnumerable<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
        public IEnumerable<GameDto> RecentGames { get; set; } = new List<GameDto>();
    }

    public class  GameDetailsViewModel
    {
        public GameDto Game { get; set; } = new GameDto();
        public IEnumerable<GameDto> RelatedGames { get; set; } = new List<GameDto>();
    }

    public class DashboardViewModel
    {
        public int TotalGames { get; set; }
        public int TotalCategories { get; set; }
        public int FeaturedGames { get; set; }
        public IEnumerable<GameDto> RecentGames { get; set; } = new List<GameDto>();
    }

    public class GameFormViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public string CoverImageUrl { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public bool IsFeatured { get; set; }

        public IEnumerable<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
    }

    /// <summary>
    /// ViewModel para a lista de games com filtro por categoria.
    /// </summary>
    public class GameListViewModel
    {
        public IEnumerable<GameDto> Games { get; set; } = new List<GameDto>();
        public IEnumerable<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
        public int? SelectedCategoryId { get; set; }
    }
}
