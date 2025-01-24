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
    public class PakketRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly PakketRepository _repository;

        public PakketRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new PakketRepository(_context);
        }

        public void Dispose()
        {
            _context.ChangeTracker.Clear();
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private Student CreateTestStudent(int id, string email = "student@example.com")
        {
            return new Student
            {
                Id = id,
                Naam = "Test Student",
                GeboorteDatum = new DateOnly(2000, 1, 1),
                StudentNummer = 12345,
                Emailaddress = email,
                Stad = "Amsterdam",
                TelefoonNr = 123456789
            };
        }

        private Pakket CreateTestPakket(
            int id, 
            string naam = "Test Pakket", 
            int kantineId = 1, 
            bool is18 = false, 
            List<Product> producten = null)
        {
            return new Pakket
            {
                Id = id,
                Naam = naam,
                Stad = "Amsterdam",
                KantineId = kantineId,
                Is18 = is18,
                IsWarm = true,
                Prijs = 10.0,
                Type = "Food",
                EindDatum = DateTime.Today.AddDays(2),
                IsOpgehaald = false,
                Producten = producten ?? new List<Product>
                {
                    new Product { Id = 1, Naam = "Product 1", IsAlcohol = false },
                    new Product { Id = 2, Naam = "Product 2", IsAlcohol = false }
                }
            };
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsPakket_WhenExists()
        {
            // Arrange
            var pakket = CreateTestPakket(1);
            _context.Pakketten.Add(pakket);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(pakket.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pakket.Naam, result.Naam);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsException_WhenNotFound()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _repository.GetByIdAsync(999));
            Assert.Equal("Pakket not found", exception.Message);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllPakketten()
        {
            // Arrange
            // Pre-create and add products to the context
            var product1 = new Product { Id = 1, Naam = "Product 1", IsAlcohol = false };
            var product2 = new Product { Id = 2, Naam = "Product 2", IsAlcohol = false };
            _context.Producten.Add(product1);
            _context.Producten.Add(product2);
            await _context.SaveChangesAsync();

            // Create pakketten using existing products
            var pakketten = new List<Pakket>
            {
                CreateTestPakket(2, "Pakket 1", producten: new List<Product> { product1, product2 }),
                CreateTestPakket(3, "Pakket 2", producten: new List<Product> { product1, product2 })
            };
            _context.Pakketten.AddRange(pakketten);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(pakketten.Count, result.Count());
        }

        [Fact]
        public async Task AddAsync_AddsPakket()
        {
            // Arrange
            var pakket = CreateTestPakket(4);

            // Act
            var result = await _repository.AddAsync(pakket);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pakket.Naam, result.Naam);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingPakket()
        {
            // Arrange
            var pakket = CreateTestPakket(5);
            _context.Pakketten.Add(pakket);
            await _context.SaveChangesAsync();

            pakket.Naam = "Updated Pakket";

            // Act
            var result = await _repository.UpdateAsync(pakket);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Pakket", result.Naam);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WhenPakketNotFound()
        {
            // Arrange
            var pakket = CreateTestPakket(999);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _repository.UpdateAsync(pakket));
            Assert.Equal("Pakket not found", exception.Message);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WhenPakketAlreadyReserved()
        {
            // Arrange
            var student = CreateTestStudent(1);
            var pakket = CreateTestPakket(7);
            pakket.ReservedBy = student;
            _context.Pakketten.Add(pakket);
            await _context.SaveChangesAsync();

            pakket.Naam = "Updated Pakket";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _repository.UpdateAsync(pakket));
            Assert.Equal("Pakket is already reserved", exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_DeletesPakket_WhenExists()
        {
            // Arrange
            var pakket = CreateTestPakket(8);
            _context.Pakketten.Add(pakket);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(pakket.Id);

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _repository.GetByIdAsync(pakket.Id));
            Assert.Equal("Pakket not found", exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsException_WhenPakketNotFound()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _repository.DeleteAsync(999));
            Assert.Equal("Pakket not found", exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsException_WhenPakketReserved()
        {
            // Arrange
            var student = CreateTestStudent(2);
            var pakket = CreateTestPakket(9);
            pakket.ReservedBy = student;
            _context.Pakketten.Add(pakket);
            await _context.SaveChangesAsync();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _repository.DeleteAsync(pakket.Id));
            Assert.Equal("Pakket is already reserved", exception.Message);
        }

        [Fact]
        public async Task GetByEmailAsync_ReturnsPakketten_ByEmail()
        {
            // Arrange
            var student = CreateTestStudent(10);

            // Pre-create and add products to the context
            var product1 = new Product { Id = 1, Naam = "Product 1", IsAlcohol = false };
            var product2 = new Product { Id = 2, Naam = "Product 2", IsAlcohol = false };
            _context.Producten.Add(product1);
            _context.Producten.Add(product2);
            await _context.SaveChangesAsync();

            // Create pakketten using the existing products
            var pakketten = new List<Pakket>
            {
            CreateTestPakket(100, producten: new List<Product> { product1, product2 }),
            CreateTestPakket(200, producten: new List<Product> { product1, product2 })
            };
            pakketten[0].ReservedBy = student;
            pakketten[1].ReservedBy = student;
            _context.Pakketten.AddRange(pakketten);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByEmailAsync(student.Emailaddress);

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task ReservePakketAsync_ReservesPakket_ForStudent()
        {
            // Arrange
            var student = CreateTestStudent(5);
            var pakket = CreateTestPakket(12);
            _context.Studenten.Add(student);
            _context.Pakketten.Add(pakket);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ReservePakketAsync(pakket.Id, student);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(student.Id, result.ReservedBy.Id);
        }

        [Fact]
        public async Task ReservePakketAsync_ThrowsException_WhenPakketNotFound()
        {
            // Arrange
            var student = CreateTestStudent(6);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _repository.ReservePakketAsync(999, student));
            Assert.Equal("Pakket not found", exception.Message);
        }

        [Fact]
        public async Task ReservePakketAsync_ThrowsException_WhenStudentUnder18()
        {
            // Arrange
            var student = CreateTestStudent(8);
            student.GeboorteDatum = new DateOnly(2010, 1, 1); // Under 18
            var pakket = CreateTestPakket(20, is18: true);
            _context.Studenten.Add(student);
            _context.Pakketten.Add(pakket);
            await _context.SaveChangesAsync();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.ReservePakketAsync(pakket.Id, student));
            Assert.Equal("Student must be 18 or older to reserve this package", exception.Message);
        }
    }
}
