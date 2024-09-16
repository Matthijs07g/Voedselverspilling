using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.Application.Services;
using Voedselverspilling.Domain.Interfaces;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.Infrastructure.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            return await _studentRepository.GetByIdAsync(studentId);
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _studentRepository.GetAllAsync();
        }

        public async Task AddStudentAsync(Student student)
        {
            await _studentRepository.AddAsync(student);
        }

        public async Task UpdateStudentAsync(Student student)
        {
            await _studentRepository.UpdateAsync(student);
        }

        public async Task DeleteStudentAsync(int studentId)
        {
            await _studentRepository.DeleteAsync(studentId);
        }
    }
}
