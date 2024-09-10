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
        public required string Name { get; set; }
        public required List<Product> Products { get; set; }
        public required string City {  get; set; }
        public required Kantine Kantine { get; set; }
        public DateTime collectTime { get; set; }
        public Boolean isAdult {  get; set; }
        public double Price { get; set; }
        public string? Type { get; set; }
        public Student? reservedBy { get; set; }
    }

    enum maaltijdType
    {
        Brood,
        Warme_avondmaaltijd,
        Drank
    }
}
