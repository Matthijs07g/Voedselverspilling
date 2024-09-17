using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voedselverspilling.Domain.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public required string Naam { get; set; }
        public DateOnly GeboorteDatum { get; set; }
        public required int StudentNummer { get; set; }
        public required string Emailaddress { get; set; }
        public string? Stad { get; set; }
        public int TelefoonNr { get; set; }
    }
}
