using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voedselverspilling.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public Boolean IsAlcohol { get; set; }
        public string? Foto { get; set; }
        public List<Pakket> Pakkets { get; set; } = new List<Pakket>();
    }
}
