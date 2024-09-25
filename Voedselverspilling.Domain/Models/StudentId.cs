using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voedselverspilling.Domain.Models
{
    public class StudentId
    {
        [Key]
        public int Id { get; set; }
        public required int StudentNummer { get; set; }
        public required string Wachtwoord {  get; set; }
        public required int Student { get; set; }
    }
}
