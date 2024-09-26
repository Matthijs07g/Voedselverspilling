using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Voedselverspilling.Web.Models
{ 
    public class MealboxModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naam is verplicht.")]
        [StringLength(100, ErrorMessage = "De naam mag niet langer zijn dan 100 tekens.")]
        public string Naam { get; set; } = string.Empty;

        [Required(ErrorMessage = "Stad is verplicht.")]
        [StringLength(100, ErrorMessage = "De stad mag niet langer zijn dan 100 tekens.")]
        public string Stad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Prijs is verplicht.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Prijs moet groter zijn dan 0.")]
        public double Prijs { get; set; }

        [StringLength(50, ErrorMessage = "Het type mag niet langer zijn dan 50 tekens.")]
        public string? Type { get; set; }

        [Display(Name = "Is 18+")]
        public bool Is18 { get; set; }

        // Eventueel een lijst van Producten als je dat wilt
        public List<int> ProductenId { get; set; } = new List<int>(); // Dit kan een lijst van IDs zijn van de producten in de mealbox
    }
}
