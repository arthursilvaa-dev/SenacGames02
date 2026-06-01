using System;
using System.Collections.Generic;
using System.Text;
using SenacGames.Application.DTOs;
using SenacGames.Application.Interfaces;
using SenacGames.Domain.Entities;
using SenacGames.Domain.Interfaces;

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


    }
}
