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
    public class StudentRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly StudentRepository _repository;

        public StudentRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new StudentRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsStudent_WhenExists()
        {
            // Arrange
            var student = new Student
            {
                Naam = "John Doe",
                GeboorteDatum = new DateOnly(2000, 1, 1),
                StudentNummer = 12345,
                Emailaddress = "johndoe@example.com",
                Stad = "Amsterdam",
                TelefoonNr = 123456789
            };
            _context.Studenten.Add(student);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(student.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(student.Naam, result.Naam);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsException_WhenNotFound()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.GetByIdAsync(999));
            Assert.Equal("Student with id 999 not found", exception.Message);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllStudents()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Naam = "Student 1", GeboorteDatum = new DateOnly(2000, 1, 1), StudentNummer = 111, Emailaddress = "student1@example.com", Stad = "Rotterdam", TelefoonNr = 12345 },
                new Student { Naam = "Student 2", GeboorteDatum = new DateOnly(2001, 2, 2), StudentNummer = 222, Emailaddress = "student2@example.com", Stad = "Den Haag", TelefoonNr = 67890 }
            };
            _context.Studenten.AddRange(students);
            await _context.SaveChangesAsync();

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
                Naam = "Jane Doe",
                GeboorteDatum = new DateOnly(1998, 5, 5),
                StudentNummer = 54321,
                Emailaddress = "janedoe@example.com",
                Stad = "Utrecht",
                TelefoonNr = 987654321
            };

            // Act
            var result = await _repository.AddAsync(student);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(student.Emailaddress, result.Emailaddress);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingStudent()
        {
            // Arrange
            var student = new Student
            {
                Naam = "Tom Smith",
                GeboorteDatum = new DateOnly(1995, 4, 4),
                StudentNummer = 12321,
                Emailaddress = "tomsmith@example.com",
                Stad = "Leiden",
                TelefoonNr = 112233445
            };
            _context.Studenten.Add(student);
            await _context.SaveChangesAsync();

            student.Naam = "Thomas Smith";

            // Act
            var result = await _repository.UpdateAsync(student);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Thomas Smith", result.Naam);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WhenStudentNotFound()
        {
            // Arrange
            var student = new Student
            {
                Id = 999,
                Naam = "Nonexistent Student",
                GeboorteDatum = new DateOnly(2000, 1, 1),
                StudentNummer = 99999,
                Emailaddress = "nonexistent@example.com",
                Stad = "Nowhere",
                TelefoonNr = 0
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _repository.UpdateAsync(student));
            Assert.Equal("Student not found", exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_DeletesExistingStudent()
        {
            // Arrange
            var student = new Student
            {
                Naam = "Anna Johnson",
                GeboorteDatum = new DateOnly(1997, 3, 3),
                StudentNummer = 55555,
                Emailaddress = "annajohnson@example.com",
                Stad = "Haarlem",
                TelefoonNr = 987654321
            };
            _context.Studenten.Add(student);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(student.Id);

            // Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.GetByIdAsync(student.Id));
            Assert.Equal($"Student with id {student.Id} not found", exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsException_WhenStudentNotFound()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _repository.DeleteAsync(999));
            Assert.Equal("Student not found", exception.Message);
        }

        [Fact]
        public async Task GetByEmailAsync_ReturnsStudent_WhenExists()
        {
            // Arrange
            var student = new Student
            {
                Naam = "David Lee",
                GeboorteDatum = new DateOnly(1999, 6, 6),
                StudentNummer = 66666,
                Emailaddress = "davidlee@example.com",
                Stad = "Eindhoven",
                TelefoonNr = 123987456
            };
            _context.Studenten.Add(student);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByEmailAsync(student.Emailaddress);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(student.Emailaddress, result.Emailaddress);
        }

        [Fact]
        public async Task GetByEmailAsync_ThrowsException_WhenNotFound()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.GetByEmailAsync("unknown@example.com"));
            Assert.Equal("Student with email unknown@example.com not found", exception.Message);
        }
    }
}
