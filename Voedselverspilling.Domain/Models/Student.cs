using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voedselverspilling.Domain.Models
{
    public class Student
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateOnly BirthDate { get; set; }
        public required int StudentNumber { get; set; }
        public required string Emailaddress { get; set; }
        public string? City { get; set; }
        public int PhoneNumber { get; set; }
    }
}
