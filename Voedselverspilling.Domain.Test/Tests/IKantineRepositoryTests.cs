using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Voedselverspilling.Domain.IRepositories;
using Voedselverspilling.Domain.Models;
using Voedselverspilling.Infrastructure;
using Voedselverspilling.Infrastructure.Repositories;
using Xunit;

namespace Voedselverspilling.Domain.Test.Tests
{
    public class KantineRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IKantineRepository _repository;

        public KantineRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Nieuwe in-memory database voor elke test
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new KantineRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted(); // Verwijder de database na de tests
            _context.Dispose();
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsKantine_WhenExists()
        {
            // Arrange
            var kantine = new Kantine { Id = 1, Stad = "Breda", Locatie = "LD", IsWarm = true };
            await _repository.AddAsync(kantine);

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(kantine.Stad, result.Stad);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenDoesNotExist()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllKantines()
        {
            // Arrange
            var kantines = new List<Kantine>
            {
                new Kantine { Id = 1, Stad = "Breda", Locatie = "LD", IsWarm = true },
                new Kantine { Id = 2, Stad = "Breda", Locatie = "LA", IsWarm = true }
            };

            foreach (var kantine in kantines)
            {
                await _repository.AddAsync(kantine);
            }

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(kantines.Count, result.Count());
        }

        [Fact]
        public async Task AddAsync_AddsKantine()
        {
            // Arrange
            var kantine = new Kantine { Id = 1, Stad = "Breda", Locatie = "LD", IsWarm = true };

            // Act
            await _repository.AddAsync(kantine);
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(kantine.Stad, result.Stad);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesKantine()
        {
            // Arrange
            var kantine = new Kantine { Id = 1, Stad = "Breda", Locatie = "LA", IsWarm = false };
            await _repository.AddAsync(kantine);

            // Act
            kantine.Stad = "Kantine Updated";
            await _repository.UpdateAsync(kantine);
            var updatedKantine = await _repository.GetByIdAsync(1);

            // Assert
            Assert.Equal("Kantine Updated", updatedKantine.Stad);
        }

        [Fact]
        public async Task DeleteAsync_RemovesKantine_WhenExists()
        {
            // Arrange
            var kantine = new Kantine { Id = 1, Stad = "Breda", Locatie = "LD", IsWarm = true };
            await _repository.AddAsync(kantine);

            // Act
            await _repository.DeleteAsync(1);
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_DoesNothing_WhenDoesNotExist()
        {
            // Act
            await _repository.DeleteAsync(999);

            // Assert
            var allKantines = await _repository.GetAllAsync();
            Assert.Empty(allKantines);
        }
    }
}
