using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Voedselverspilling.Domain.Models;
using Voedselverspilling.Infrastructure;
using Voedselverspilling.Infrastructure.Repositories;
using Xunit;

namespace Voedselverspilling.Infrastructure.Test.Tests
{
    public class KantineRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly KantineRepository _repository;

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
            Assert.Equal(kantine.Locatie, result.Locatie);
            Assert.Equal(kantine.IsWarm, result.IsWarm);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsException_WhenDoesNotExist()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _repository.GetByIdAsync(999));
            Assert.Equal("Kantine not found", exception.Message);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllKantines()
        {
            // Arrange
            var kantines = new List<Kantine>
            {
                new Kantine { Id = 1, Stad = "Breda", Locatie = "LD", IsWarm = true },
                new Kantine { Id = 2, Stad = "Tilburg", Locatie = "LA", IsWarm = false }
            };

            foreach (var kantine in kantines)
            {
                await _repository.AddAsync(kantine);
            }

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(kantines.Count, result.Count());
            Assert.Contains(result, k => k.Stad == "Breda");
            Assert.Contains(result, k => k.Stad == "Tilburg");
        }

        [Fact]
        public async Task AddAsync_AddsKantine()
        {
            // Arrange
            var kantine = new Kantine { Id = 1, Stad = "Eindhoven", Locatie = "XX", IsWarm = true };

            // Act
            var result = await _repository.AddAsync(kantine);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(kantine.Stad, result.Stad);

            var addedKantine = await _repository.GetByIdAsync(kantine.Id);
            Assert.Equal("Eindhoven", addedKantine.Stad);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesKantine_WhenExists()
        {
            // Arrange
            var kantine = new Kantine { Id = 1, Stad = "Breda", Locatie = "LD", IsWarm = true };
            await _repository.AddAsync(kantine);

            // Act
            kantine.Stad = "Rotterdam";
            kantine.IsWarm = false;
            var updatedKantine = await _repository.UpdateAsync(kantine);

            // Assert
            Assert.NotNull(updatedKantine);
            Assert.Equal("Rotterdam", updatedKantine.Stad);
            Assert.False(updatedKantine.IsWarm);

            var fetchedKantine = await _repository.GetByIdAsync(kantine.Id);
            Assert.Equal("Rotterdam", fetchedKantine.Stad);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WhenKantineDoesNotExist()
        {
            // Arrange
            var kantine = new Kantine { Id = 999, Stad = "Den Haag", Locatie = "XX", IsWarm = true };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _repository.UpdateAsync(kantine));
            Assert.Equal("Kantine not found", exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_RemovesKantine_WhenExists()
        {
            // Arrange
            var kantine = new Kantine { Id = 1, Stad = "Breda", Locatie = "LD", IsWarm = true };
            await _repository.AddAsync(kantine);

            // Act
            await _repository.DeleteAsync(1);

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _repository.GetByIdAsync(1));
            Assert.Equal("Kantine not found", exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsException_WhenKantineDoesNotExist()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _repository.DeleteAsync(999));
            Assert.Equal("Kantine not found", exception.Message);
        }
    }
}
