using Microsoft.AspNetCore.Mvc;
using Voedselverspilling.Application.Services;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudenten()
        {
            IEnumerable<Student> students = await _studentService.GetAllStudentsAsync();

            if (students == null)
            {
                return BadRequest("No Students found");
            }
            else
            {
                return Ok(students);
            }
        }

        //GET one
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReserveringById(int id)
        {
            Student student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return BadRequest("Not student found");
            }
            else
            {
                return Ok(student);
            }
        }
    }
}
