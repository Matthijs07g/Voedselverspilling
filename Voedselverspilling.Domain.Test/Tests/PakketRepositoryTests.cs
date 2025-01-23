using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Voedselverspilling.Domain.Models;
using Voedselverspilling.Infrastructure;
using Voedselverspilling.Infrastructure.Repositories;
using Xunit;

namespace Voedselverspilling.Domain.Test.Tests
{
    public class PakketRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly PakketRepository _repository;

        public PakketRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique database per test
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new PakketRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted(); // Cleanup after each test
            _context.Dispose();
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsPakket_WhenExists()
        {
            // Arrange
            var pakket = new Pakket { Id = 1, Naam = "Pakket 1", Stad = "Breda", KantineId = 1, Is18 = true, IsWarm = false, Prijs = 10.0 };
            await _repository.AddAsync(pakket);

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pakket.Naam, result.Naam);
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
        public async Task GetAllAsync_ReturnsAllPakketten()
        {
            // Arrange
            var pakketten = new List<Pakket>
            {
                new Pakket { Id = 1, Naam = "Pakket 1", Stad = "Breda", KantineId = 1, Is18 = true, IsWarm = false, Prijs = 10.0 },
                new Pakket { Id = 2, Naam = "Pakket 2", Stad = "Rotterdam", KantineId = 2, Is18 = false, IsWarm = true, Prijs = 15.0 }
            };

            foreach (var pakket in pakketten)
            {
                await _repository.AddAsync(pakket);
            }

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(pakketten.Count, result.Count());
        }

        [Fact]
        public async Task AddAsync_AddsPakket()
        {
            // Arrange
            var pakket = new Pakket { Id = 1, Naam = "Pakket 1", Stad = "Breda", KantineId = 1, Is18 = true, IsWarm = false, Prijs = 10.0 };

            // Act
            await _repository.AddAsync(pakket);
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pakket.Naam, result.Naam);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesPakket()
        {
            // Arrange
            var pakket = new Pakket { Id = 1, Naam = "Pakket 1", Stad = "Breda", KantineId = 1, Is18 = true, IsWarm = false, Prijs = 10.0 };
            await _repository.AddAsync(pakket);

            // Act
            pakket.Naam = "Pakket Updated";
            await _repository.UpdateAsync(pakket);
            var updatedPakket = await _repository.GetByIdAsync(1);

            // Assert
            Assert.Equal("Pakket Updated", updatedPakket.Naam);
        }

        [Fact]
        public async Task DeleteAsync_RemovesPakket_WhenExists()
        {
            // Arrange
            var pakket = new Pakket { Id = 1, Naam = "Pakket 1", Stad = "Breda", KantineId = 1, Is18 = true, IsWarm = false, Prijs = 10.0 };
            await _repository.AddAsync(pakket);

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
            var allPakketten = await _repository.GetAllAsync();
            Assert.Empty(allPakketten);
        }
    }
}
