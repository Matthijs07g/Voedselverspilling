using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voedselverspilling.Domain.Models
{
    public class KantineWorkerId
    {
        public required int Personeelsnummer { get; set; }
        public required string Wachtwoord {  get; set; }
        public required KantineWorker KantineWorker { get; set; }
    }
}
