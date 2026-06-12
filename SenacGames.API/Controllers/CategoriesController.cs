using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SenacGames.Application.DTOs;
using SenacGames.Application.Interfaces;
using System.Collections;

namespace SenacGames.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
      private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
         _categoryService = categoryService;
        }

        /// <summary>
        /// Retorna todas as categorias
        /// GET /api/categories
        ///</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryDto dto)
        {
            var category = await _categoryService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { id = category.Id }, category);
        }

    }
}
