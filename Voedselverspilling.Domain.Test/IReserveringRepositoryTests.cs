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
    public class ReserveringRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IReserveringRepository _repository;

        public ReserveringRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique in-memory database per test
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new ReserveringRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted(); // Cleanup the in-memory database
            _context.Dispose();
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsReservering_WhenExists()
        {
            // Arrange
            var reservering = new Reservering
            {
                ReserveringId = 1,
                ReservaringDatum = DateTime.Now,
                IsOpgehaald = false,
                TijdOpgehaald = DateTime.MinValue,
                StudentId = 101,
                PakketId = 201
            };
            await _repository.AddAsync(reservering);

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(reservering.StudentId, result.StudentId);
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
        public async Task GetAllAsync_ReturnsAllReserveringen()
        {
            // Arrange
            var reserveringen = new List<Reservering>
            {
                new Reservering { ReserveringId = 1, ReservaringDatum = DateTime.Now, IsOpgehaald = false, TijdOpgehaald = DateTime.MinValue, StudentId = 101, PakketId = 201 },
                new Reservering { ReserveringId = 2, ReservaringDatum = DateTime.Now, IsOpgehaald = true, TijdOpgehaald = DateTime.Now, StudentId = 102, PakketId = 202 }
            };

            foreach (var reservering in reserveringen)
            {
                await _repository.AddAsync(reservering);
            }

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(reserveringen.Count, result.Count());
        }

        [Fact]
        public async Task AddAsync_AddsReservering()
        {
            // Arrange
            var reservering = new Reservering
            {
                ReserveringId = 1,
                ReservaringDatum = DateTime.Now,
                IsOpgehaald = false,
                TijdOpgehaald = DateTime.MinValue,
                StudentId = 101,
                PakketId = 201
            };

            // Act
            await _repository.AddAsync(reservering);
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(reservering.StudentId, result.StudentId);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesReservering()
        {
            // Arrange
            var reservering = new Reservering
            {
                ReserveringId = 1,
                ReservaringDatum = DateTime.Now,
                IsOpgehaald = false,
                TijdOpgehaald = DateTime.MinValue,
                StudentId = 101,
                PakketId = 201
            };
            await _repository.AddAsync(reservering);

            // Act
            reservering.IsOpgehaald = true;
            reservering.TijdOpgehaald = DateTime.Now;
            await _repository.UpdateAsync(reservering);
            var updatedReservering = await _repository.GetByIdAsync(1);

            // Assert
            Assert.True(updatedReservering.IsOpgehaald);
        }

        [Fact]
        public async Task DeleteAsync_RemovesReservering_WhenExists()
        {
            // Arrange
            var reservering = new Reservering
            {
                ReserveringId = 1,
                ReservaringDatum = DateTime.Now,
                IsOpgehaald = false,
                TijdOpgehaald = DateTime.MinValue,
                StudentId = 101,
                PakketId = 201
            };
            await _repository.AddAsync(reservering);

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
            var allReserveringen = await _repository.GetAllAsync();
            Assert.Empty(allReserveringen);
        }
    }
}
