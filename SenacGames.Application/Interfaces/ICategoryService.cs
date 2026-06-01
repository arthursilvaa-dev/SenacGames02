using System;
using System.Collections.Generic;
using System.Text;
using SenacGames.Application.DTOs;
using SenacGames.Domain.Entities;

namespace SenacGames.Application.Interfaces
{
    /// <summary>
    /// Contrato de serviço de Categorias
    /// </summary>
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
        Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto dto);
        Task<bool> DeleteAsync(int id);
        Task<int> CountAsync();

    }
}
