using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voedselverspilling.Domain.Models
{
    public class StudentID
    {
        public required int StudentNummer { get; set; }
        public required string Wachtwoord {  get; set; }
        public required Student Student { get; set; }
    }
}
