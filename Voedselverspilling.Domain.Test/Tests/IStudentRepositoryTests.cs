using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Voedselverspilling.Domain.Interfaces;
using Voedselverspilling.Domain.IRepositories;
using Voedselverspilling.Domain.Models;
using Voedselverspilling.Infrastructure;
using Voedselverspilling.Infrastructure.Repositories;
using Xunit;

namespace Voedselverspilling.Domain.Test.Tests
{
    public class StudentRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IStudentRepository _repository;

        public StudentRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique in-memory database per test
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new StudentRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted(); // Cleanup the in-memory database
            _context.Dispose();
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsStudent_WhenExists()
        {
            // Arrange
            var student = new Student
            {
                Id = 1,
                Naam = "John Doe",
                GeboorteDatum = new DateOnly(2000, 1, 1),
                StudentNummer = 12345,
                Emailaddress = "john.doe@example.com",
                Stad = "Amsterdam",
                TelefoonNr = 123456789
            };
            await _repository.AddAsync(student);

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(student.Naam, result.Naam);
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
        public async Task GetAllAsync_ReturnsAllStudents()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Id = 1, Naam = "John Doe", GeboorteDatum = new DateOnly(2000, 1, 1), StudentNummer = 12345, Emailaddress = "john.doe@example.com", Stad = "Amsterdam", TelefoonNr = 123456789 },
                new Student { Id = 2, Naam = "Jane Doe", GeboorteDatum = new DateOnly(1999, 5, 5), StudentNummer = 12346, Emailaddress = "jane.doe@example.com", Stad = "Rotterdam", TelefoonNr = 987654321 }
            };

            foreach (var student in students)
            {
                await _repository.AddAsync(student);
            }

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(students.Count, result.Count());
        }

        [Fact]
        public async Task AddAsync_AddsStudent()
        {
            // Arrange
            var student = new Student
            {
                Id = 1,
                Naam = "John Doe",
                GeboorteDatum = new DateOnly(2000, 1, 1),
                StudentNummer = 12345,
                Emailaddress = "john.doe@example.com",
                Stad = "Amsterdam",
                TelefoonNr = 123456789
            };

            // Act
            await _repository.AddAsync(student);
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(student.Naam, result.Naam);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesStudent()
        {
            // Arrange
            var student = new Student
            {
                Id = 1,
                Naam = "John Doe",
                GeboorteDatum = new DateOnly(2000, 1, 1),
                StudentNummer = 12345,
                Emailaddress = "john.doe@example.com",
                Stad = "Amsterdam",
                TelefoonNr = 123456789
            };
            await _repository.AddAsync(student);

            // Act
            student.Naam = "John Updated";
            await _repository.UpdateAsync(student);
            var updatedStudent = await _repository.GetByIdAsync(1);

            // Assert
            Assert.Equal("John Updated", updatedStudent.Naam);
        }

        [Fact]
        public async Task DeleteAsync_RemovesStudent_WhenExists()
        {
            // Arrange
            var student = new Student
            {
                Id = 1,
                Naam = "John Doe",
                GeboorteDatum = new DateOnly(2000, 1, 1),
                StudentNummer = 12345,
                Emailaddress = "john.doe@example.com",
                Stad = "Amsterdam",
                TelefoonNr = 123456789
            };
            await _repository.AddAsync(student);

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
            var allStudents = await _repository.GetAllAsync();
            Assert.Empty(allStudents);
        }
    }
}

