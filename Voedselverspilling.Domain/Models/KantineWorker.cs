using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voedselverspilling.Domain.Models
{
    public class KantineWorker
    {
        [Key]
        public int Id { get; set; }
        public required string Naam { get; set; }
        public required int PersoneelsNummer { get; set; }
        public required string Stad {  get; set; }
        public required string Locatie { get; set; }
    }
}
