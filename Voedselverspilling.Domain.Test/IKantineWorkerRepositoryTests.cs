using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Voedselverspilling.Domain.IRepositories;
using Voedselverspilling.Domain.Models;
using Voedselverspilling.Infrastructure;
using Voedselverspilling.Infrastructure.Repositories;
using Xunit;

namespace Voedselverspilling.Domain.Test
{
    public class KantineWorkerRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IKantineWorkerRepository _repository;

        public KantineWorkerRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // New in-memory database per test
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new KantineWorkerRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted(); // Delete the database after tests
            _context.Dispose();
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsKantineWorker_WhenExists()
        {
            // Arrange
            var worker = new KantineWorker { Id = 1, Naam = "John Doe", PersoneelsNummer = 123, Email = "john.doe@example.com", Stad = "Amsterdam", KantineId = 1 };
            await _repository.AddAsync(worker);

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(worker.Naam, result.Naam);
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
        public async Task GetAllAsync_ReturnsAllKantineWorkers()
        {
            // Arrange
            var workers = new List<KantineWorker>
            {
                new KantineWorker { Id = 1, Naam = "John Doe", PersoneelsNummer = 123, Email = "john.doe@example.com", Stad = "Amsterdam", KantineId = 1 },
                new KantineWorker { Id = 2, Naam = "Jane Smith", PersoneelsNummer = 124, Email = "jane.smith@example.com", Stad = "Rotterdam", KantineId = 1 }
            };

            foreach (var worker in workers)
            {
                await _repository.AddAsync(worker);
            }

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(workers.Count, result.Count());
        }

        [Fact]
        public async Task AddAsync_AddsKantineWorker()
        {
            // Arrange
            var worker = new KantineWorker { Id = 1, Naam = "John Doe", PersoneelsNummer = 123, Email = "john.doe@example.com", Stad = "Amsterdam", KantineId = 1 };

            // Act
            await _repository.AddAsync(worker);
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(worker.Naam, result.Naam);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesKantineWorker()
        {
            // Arrange
            var worker = new KantineWorker { Id = 1, Naam = "John Doe", PersoneelsNummer = 123, Email = "john.doe@example.com", Stad = "Amsterdam", KantineId = 1 };
            await _repository.AddAsync(worker);

            // Act
            worker.Naam = "John Updated";
            await _repository.UpdateAsync(worker);
            var updatedWorker = await _repository.GetByIdAsync(1);

            // Assert
            Assert.Equal("John Updated", updatedWorker.Naam);
        }

        [Fact]
        public async Task DeleteAsync_RemovesKantineWorker_WhenExists()
        {
            // Arrange
            var worker = new KantineWorker { Id = 1, Naam = "John Doe", PersoneelsNummer = 123, Email = "john.doe@example.com", Stad = "Amsterdam", KantineId = 1 };
            await _repository.AddAsync(worker);

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
            var allWorkers = await _repository.GetAllAsync();
            Assert.Empty(allWorkers);
        }
    }
}
