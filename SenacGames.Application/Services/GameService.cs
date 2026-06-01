using System;
using System.Collections.Generic;
using System.Text;
using SenacGames.Application.DTOs;
using SenacGames.Application.Interfaces;
using SenacGames.Domain.Entities;
using SenacGames.Domain.Interfaces;
using static System.Net.WebRequestMethods;

namespace SenacGames.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<IEnumerable<GameDto>> GetAllAsync()
        {
            var games = await _gameRepository.GetAllAsync();
            return games.Select(MapToDto);
        }

        public async Task<GameDto?> GetByIdAsync(int id)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            return game == null ? null : MapToDto(game);
        }

        public async Task<IEnumerable<GameDto>> GetFeaturedAsync()
        {
            var games = await _gameRepository.GetFeaturedAsync();
            return games.Select(MapToDto);
        }

        public async Task<IEnumerable<GameDto>> GetByCategoryAsync(int categoryId)
        {
            var games = await _gameRepository.GetByCategoryAsync(categoryId);
            return games.Select(MapToDto);
        }

        public async Task<GameDto> CreateAsync(CreateGameDto dto)
        {
            //Mapeia o DTO de criação para a entidade Game
            var game = new Game
            {
                Title = dto.Title,
                Description = dto.Description,
                ReleaseYear = dto.ReleaseYear,
                CoverImageUrl = dto.CoverImageUrl,
                CategoryId = dto.CategoryId,
                IsFeatured = dto.IsFeatured,
                CreatedAt = DateTime.Now
            };

            await _gameRepository.AddAsync(game);

            //Retorna o game criado como DTO
            return MapToDto(game);
        }

        public async Task<GameDto?> UpdateAsync(int id, UpdateGameDto dto)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            if (game == null) return null;

            game.Title = dto.Title;
            game.Description = dto.Description;
            game.ReleaseYear = dto.ReleaseYear;
            game.CoverImageUrl = dto.CoverImageUrl;
            game.CategoryId = dto.CategoryId;
            game.IsFeatured = dto.IsFeatured;

            await _gameRepository.UpdateAsync(game);
            return MapToDto(game);

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            if (game == null)
            {
                return false;
            }

            await _gameRepository.DeleteAsync(id);
            return true;
        }

        public async Task<int> CountAsync()
        {
            return await _gameRepository.CountAsync();
        }

        private static GameDto MapToDto(Game game)
        {
            return new GameDto
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                ReleaseYear = game.ReleaseYear,
                CoverImageUrl = game.CoverImageUrl,
                CategoryId = game.CategoryId,
                CategoryName = game.Category?.Name ?? string.Empty,
                IsFeatured = game.IsFeatured,
                CreatedAt = game.CreatedAt
            };

        }



    }
}
