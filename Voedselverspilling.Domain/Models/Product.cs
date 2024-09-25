using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voedselverspilling.Domain.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public required string Naam { get; set; }
        public Boolean IsAlcohol { get; set; }
        public string? Foto { get; set; }

    }
}
