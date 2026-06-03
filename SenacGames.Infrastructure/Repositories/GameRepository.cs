using Microsoft.EntityFrameworkCore;
using SenacGames.Domain.Entities;
using SenacGames.Domain.Interfaces;
using SenacGames.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace SenacGames.Infrastructure.Repositories
{
    /// <summary>
    /// Implementação do repositório de Games usando o 
    /// Entity Framework Core para acessar o banco de dados.
    /// </summary>
    public class GameRepository : IGameRepository
    {
        private readonly SenacGamesDbContext _context;

        public GameRepository(SenacGamesDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os games incluindo categoria relacionada 
        /// por data de criação
        /// OBS: Include() = carrega dados de tabelas relacionadas
        /// </summary>
        /// <returns>
        /// Lista de Games
        /// </returns>
        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _context.Games
                .Include(g => g.Category) // Faz JOIN com a tabela Categories
                .OrderByDescending(g => g.CreatedAt) // Ordena por data de criação
                .ToListAsync();
        }

        /// <summary>
        /// Retorna um game por ID, incluindo a categoria relacionada
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Game com id específico.</returns>
        public async Task<Game?> GetByIdAsync(int id)
        {
            return await _context.Games
                .Include(g => g.Category) // Faz JOIN com a tabela Categories
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        /// <summary>
        /// Retorna apenas os games marcados como destaque
        /// OBS: WHERE() = filtra os dados com base em uma condição(Equivalente ao WHERE do SQL)
        /// </summary>
        public async Task<IEnumerable<Game>> GetFeaturedAsync()
        {
            return await _context.Games
                .Include(g => g.Category) // Faz JOIN com a tabela Categories
                .Where(g => g.IsFeatured) // Filtra apenas os games em destaque
                .ToListAsync();
        }

        /// <summary>
        ///  Retorna todos os games de uma categoria especifica   
        /// </summary>
        public async Task<IEnumerable<Game>> GetByCategoryAsync(int categoryId)
        {
            return await _context.Games
                .Include(g => g.Category) // Faz JOIN com a tabela Categories
                .Where(g => g.CategoryId == categoryId) // Filtra apenas os games em destaque
                .ToListAsync();
        }

        /// <summary>
        /// Adiciona um novo game ao banco de dados e salva as alterações.
        /// OBS: AddAsync() = adiciona um novo registro ao banco de dados de forma assíncrona
        /// OBS: SaveChangesAsync() = equivalente ao INSERT, salva as alterações feitas no contexto do banco de dados de forma assíncrona
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public async Task AddAsync(Game game)
        {
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Atualiza um game existente no banco de dados e salva as alterações.
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Game game)
        {
            _context.Games.Update(game); // o Update não necessita de await pois ele apenas marca a entidade como modificada, não executa uma operação no banco de dados imediatamente
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Remove um game do banco de dados com base no ID e salva as alterações.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if(game != null)
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        ///  Retorna a contagem total de games no banco de dados.
        /// </summary>
        /// <returns></returns>
        public async Task<int> CountAsync()
        {
            return await _context.Games.CountAsync();
        }



    }
}
