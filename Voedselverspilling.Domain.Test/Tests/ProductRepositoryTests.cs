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
    public class ProductRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ProductRepository _repository;

        public ProductRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new ProductRepository(_context);
        }

        public void Dispose()
        {
            _context.ChangeTracker.Clear();
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsProduct_WhenExists()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Naam = "Test Product",
                IsAlcohol = false,
                Foto = "testphoto.jpg"
            };
            _context.Producten.Add(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(product.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Naam, result.Naam);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsException_WhenNotFound()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _repository.GetByIdAsync(999));
            Assert.Equal("Product not found", exception.Message);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Naam = "Product 1", IsAlcohol = false },
                new Product { Id = 2, Naam = "Product 2", IsAlcohol = true }
            };
            _context.Producten.AddRange(products);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(products.Count, result.Count());
        }

        [Fact]
        public async Task GetAllAsync_ThrowsException_WhenNoProductsExist()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _repository.GetAllAsync());
            Assert.Equal("No products found", exception.Message);
        }

        [Fact]
        public async Task AddAsync_AddsProduct()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Naam = "New Product",
                IsAlcohol = false,
                Foto = "newphoto.jpg"
            };

            // Act
            var result = await _repository.AddAsync(product);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Naam, result.Naam);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingProduct()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Naam = "Original Product",
                IsAlcohol = false,
                Foto = "originalphoto.jpg"
            };
            _context.Producten.Add(product);
            await _context.SaveChangesAsync();

            product.Naam = "Updated Product";

            // Act
            var result = await _repository.UpdateAsync(product);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Product", result.Naam);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WhenProductNotFound()
        {
            // Arrange
            var product = new Product
            {
                Id = 999,
                Naam = "Nonexistent Product",
                IsAlcohol = false,
                Foto = "nonexistentphoto.jpg"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _repository.UpdateAsync(product));
            Assert.Equal("Product not found", exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_DeletesProduct_WhenExists()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Naam = "Product to Delete",
                IsAlcohol = false,
                Foto = "deletephoto.jpg"
            };
            _context.Producten.Add(product);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(product.Id);

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _repository.GetByIdAsync(product.Id));
            Assert.Equal("Product not found", exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsException_WhenProductNotFound()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _repository.DeleteAsync(999));
            Assert.Equal("Product not found", exception.Message);
        }
    }
}
