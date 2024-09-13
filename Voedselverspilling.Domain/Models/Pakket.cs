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
        public required string Name { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public required string City {  get; set; }
        public required Kantine Kantine { get; set; }
        public DateTime CollectTime { get; set; }
        public Boolean IsAdult {  get; set; }
        public double Price { get; set; }
        public string? Type { get; set; }
        public Student? ReservedBy { get; set; }
    }

    enum maaltijdType
    {
        Brood,
        Warme_avondmaaltijd,
        Drank
    }
}
