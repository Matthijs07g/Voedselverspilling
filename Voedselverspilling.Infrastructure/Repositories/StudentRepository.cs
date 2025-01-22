using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.DomainServices.Interfaces;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            var student = await _context.Studenten.FindAsync(id);
            return student ?? throw new KeyNotFoundException($"Student with id {id} not found");
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            var students = await _context.Studenten.ToListAsync();
            return students ?? throw new Exception("No students found");
        }

        public async Task<Student> AddAsync(Student student)
        {
            _context.Studenten.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student> UpdateAsync(Student student)
        {
            var studentOld = await _context.Studenten.FindAsync(student.Id);
            if(studentOld == null)
            {
                throw new Exception("Student not found");
            }
            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task DeleteAsync(int id)
        {
            var student = await _context.Studenten.FindAsync(id);
            if (student == null)
            {
                throw new Exception("Student not found");
            }

            _context.Studenten.Remove(student);
            await _context.SaveChangesAsync();
        }
    }
}
