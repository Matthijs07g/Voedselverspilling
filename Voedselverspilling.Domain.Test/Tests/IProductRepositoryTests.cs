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

namespace Voedselverspilling.Domain.Test.Tests
{
    public class ProductRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _repository;

        public ProductRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique in-memory database per test
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new ProductRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted(); // Cleanup the in-memory database
            _context.Dispose();
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsProduct_WhenExists()
        {
            // Arrange
            var product = new Product { Id = 1, Naam = "Bier", IsAlcohol = true };
            await _repository.AddAsync(product);

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Naam, result.Naam);
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
        public async Task GetAllAsync_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Naam = "Bier", IsAlcohol = true },
                new Product { Id = 2, Naam = "Water", IsAlcohol = false }
            };

            foreach (var product in products)
            {
                await _repository.AddAsync(product);
            }

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(products.Count, result.Count());
        }

        [Fact]
        public async Task AddAsync_AddsProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Naam = "Bier", IsAlcohol = true };

            // Act
            await _repository.AddAsync(product);
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Naam, result.Naam);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Naam = "Bier", IsAlcohol = true };
            await _repository.AddAsync(product);

            // Act
            product.Naam = "Wijn";
            await _repository.UpdateAsync(product);
            var updatedProduct = await _repository.GetByIdAsync(1);

            // Assert
            Assert.Equal("Wijn", updatedProduct.Naam);
        }

        [Fact]
        public async Task DeleteAsync_RemovesProduct_WhenExists()
        {
            // Arrange
            var product = new Product { Id = 1, Naam = "Bier", IsAlcohol = true };
            await _repository.AddAsync(product);

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
            var allProducts = await _repository.GetAllAsync();
            Assert.Empty(allProducts);
        }
    }
}
