using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voedselverspilling.Domain.Models
{
    public class Pakket
    {
        public int Id { get; set; }
        public required string Naam { get; set; }
        public List<Product> Producten { get; set; } = new List<Product>();
        public required string Stad {  get; set; }
        public required Kantine Kantine { get; set; }
        public Boolean Is18 {  get; set; }
        public double Prijs { get; set; }
        public string? Type { get; set; }
    }

    enum maaltijdType
    {
        Brood,
        Warme_avondmaaltijd,
        Drank
    }
}
