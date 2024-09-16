using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.Application.Services
{
    public interface IStudentService
    {
        Task<Student> GetStudentByIdAsync(int studentId);
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task AddStudentAsync(Student student);
        Task UpdateStudentAsync(Student student);
        Task DeleteStudentAsync(int studentId);
    }
}
