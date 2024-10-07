using System;
using System.ComponentModel.DataAnnotations;

namespace Voedselverspilling.Web.Models
{
    public class ReserveringModel
    {
        public int ReserveringId { get; set; } // Primary key

        [Required(ErrorMessage = "Reserveringsdatum is verplicht.")]
        [Display(Name = "Reserveringsdatum")]
        public DateTime ReservaringDatum { get; set; }

        [Required(ErrorMessage = "Opgehaald status is verplicht.")]
        [Display(Name = "Opgehaald")]
        public bool IsOpgehaald { get; set; }

        [Display(Name = "Tijd Opgehaald")]
        public DateTime? TijdOpgehaald { get; set; } // Nullable because it might not be picked up yet

        [Required(ErrorMessage = "Student ID is verplicht.")]
        [Display(Name = "Student ID")]
        public int StudentId { get; set; } // Foreign key linking to Student

        [Required(ErrorMessage = "Pakket ID is verplicht.")]
        [Display(Name = "Pakket ID")]
        public int PakketId { get; set; } // Foreign key linking to Pakket
    }
}
