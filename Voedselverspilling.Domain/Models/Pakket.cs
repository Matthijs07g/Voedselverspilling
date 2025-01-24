using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voedselverspilling.Domain.Models
{
    public class Pakket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Naam { get; set; }

        [Required]
        public required string Stad {  get; set; }

        [Required]
        public required int KantineId { get; set; }

        [Required]
        public Boolean Is18 {  get; set; }

        [Required]
        public Boolean IsWarm {  get; set; }

        [Required]
        public double Prijs { get; set; }

        [Required]
        public string Type { get; set; }
        public ICollection<Product> Producten { get; set; } = null!;
        public Student? ReservedBy { get; set; }
        public int? ReservedById { get; set; }
        public DateTime? ReserveringDatum { get; set; }

        [Required]
        public Boolean IsOpgehaald { get; set; } = false;

        [Required]
        public DateTime EindDatum { get; set; }
    }
}
