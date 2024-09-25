using System.ComponentModel.DataAnnotations;

namespace Voedselverspilling.Web.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naam is verplicht.")]
        [StringLength(100, ErrorMessage = "De naam mag niet langer zijn dan 100 tekens.")]
        public string Naam { get; set; } = string.Empty;

        [Display(Name = "Bevat Alcohol")]
        public bool IsAlcohol { get; set; }

        [Display(Name = "Afbeelding")]
        public string? Foto { get; set; }
    }
}
