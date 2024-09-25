using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voedselverspilling.Domain.Models
{
    public class KantineWorkerId
    {
        [Key]
        public int Id { get; set; }
        public required int Personeelsnummer { get; set; }
        public required string Wachtwoord {  get; set; }
        public required int WorkerId { get; set; }
    }
}
