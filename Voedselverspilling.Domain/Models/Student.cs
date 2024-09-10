using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voedselverspilling.Domain.Models
{
    public class Student
    {
        public required string Name { get; set; }
        public DateOnly birthDate { get; set; }
        public required int studentNumber { get; set; }
        public required string emailaddress { get; set; }
        public string? City { get; set; }
        public int phoneNumber { get; set; }
    }
}
