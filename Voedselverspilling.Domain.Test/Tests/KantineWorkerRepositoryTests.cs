using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Voedselverspilling.Domain.Models;
using Voedselverspilling.Infrastructure.Repositories;
using Xunit;

namespace Voedselverspilling.Infrastructure.Test.Tests
{
    public class KantineWorkerRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly KantineWorkerRepository _repository;

        public KantineWorkerRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Isolated database for each test
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new KantineWorkerRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted(); // Clean up the database after each test
            _context.Dispose();
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsKantineWorker_WhenExists()
        {
            // Arrange
            var worker = new KantineWorker
            {
                Id = 1,
                Naam = "John Doe",
                PersoneelsNummer = 12345,
                Email = "john.doe@example.com",
                Stad = "Amsterdam",
                KantineId = 1
            };
            await _repository.AddAsync(worker);

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(worker.Naam, result.Naam);
            Assert.Equal(worker.Email, result.Email);
            Assert.Equal(worker.Stad, result.Stad);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsKeyNotFoundException_WhenDoesNotExist()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _repository.GetByIdAsync(999));
            Assert.Equal("KantineWorker with id 999 not found", exception.Message);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllKantineWorkers()
        {
            // Arrange
            var workers = new List<KantineWorker>
            {
                new KantineWorker { Id = 1, Naam = "Alice", PersoneelsNummer = 12345, Email = "alice@example.com", Stad = "Breda", KantineId = 1 },
                new KantineWorker { Id = 2, Naam = "Bob", PersoneelsNummer = 67890, Email = "bob@example.com", Stad = "Rotterdam", KantineId = 2 }
            };

            foreach (var worker in workers)
            {
                await _repository.AddAsync(worker);
            }

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(workers.Count, result.Count());
            Assert.Contains(result, w => w.Naam == "Alice");
            Assert.Contains(result, w => w.Naam == "Bob");
        }

        [Fact]
        public async Task AddAsync_AddsKantineWorker()
        {
            // Arrange
            var worker = new KantineWorker
            {
                Id = 1,
                Naam = "Charlie",
                PersoneelsNummer = 11223,
                Email = "charlie@example.com",
                Stad = "Eindhoven",
                KantineId = 3
            };

            // Act
            var result = await _repository.AddAsync(worker);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(worker.Naam, result.Naam);

            var addedWorker = await _repository.GetByIdAsync(worker.Id);
            Assert.Equal("Charlie", addedWorker.Naam);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesKantineWorker_WhenExists()
        {
            // Arrange
            var worker = new KantineWorker
            {
                Id = 1,
                Naam = "Dave",
                PersoneelsNummer = 44556,
                Email = "dave@example.com",
                Stad = "Utrecht",
                KantineId = 2
            };
            await _repository.AddAsync(worker);

            // Act
            worker.Naam = "David";
            worker.Stad = "Den Haag";
            var updatedWorker = await _repository.UpdateAsync(worker);

            // Assert
            Assert.NotNull(updatedWorker);
            Assert.Equal("David", updatedWorker.Naam);
            Assert.Equal("Den Haag", updatedWorker.Stad);

            var fetchedWorker = await _repository.GetByIdAsync(worker.Id);
            Assert.Equal("David", fetchedWorker.Naam);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WhenKantineWorkerDoesNotExist()
        {
            // Arrange
            var worker = new KantineWorker
            {
                Id = 999,
                Naam = "Eve",
                PersoneelsNummer = 33445,
                Email = "eve@example.com",
                Stad = "Haarlem",
                KantineId = 4
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await _repository.UpdateAsync(worker));
        }

        [Fact]
        public async Task DeleteAsync_RemovesKantineWorker_WhenExists()
        {
            // Arrange
            var worker = new KantineWorker
            {
                Id = 1,
                Naam = "Frank",
                PersoneelsNummer = 55667,
                Email = "frank@example.com",
                Stad = "Leiden",
                KantineId = 5
            };
            await _repository.AddAsync(worker);

            // Act
            await _repository.DeleteAsync(1);

            // Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _repository.GetByIdAsync(1));
            Assert.Equal("KantineWorker with id 1 not found", exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsKeyNotFoundException_WhenKantineWorkerDoesNotExist()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _repository.DeleteAsync(999));
            Assert.Equal("KantineWorker with id 999 not found", exception.Message);
        }

        [Fact]
        public async Task GetByEmailAsync_ReturnsKantineWorker_WhenExists()
        {
            // Arrange
            var worker = new KantineWorker
            {
                Id = 1,
                Naam = "Grace",
                PersoneelsNummer = 88990,
                Email = "grace@example.com",
                Stad = "Tilburg",
                KantineId = 6
            };
            await _repository.AddAsync(worker);

            // Act
            var result = await _repository.GetByEmailAsync("grace@example.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(worker.Naam, result.Naam);
            Assert.Equal(worker.Email, result.Email);
        }

        [Fact]
        public async Task GetByEmailAsync_ThrowsKeyNotFoundException_WhenEmailDoesNotExist()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _repository.GetByEmailAsync("unknown@example.com"));
            Assert.Equal("KantineWorker with email unknown@example.com not found", exception.Message);
        }
    }
}
