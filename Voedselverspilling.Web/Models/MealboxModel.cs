﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.Web.Models
{ 
    public class MealboxModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naam is verplicht.")]
        [StringLength(100, ErrorMessage = "De naam mag niet langer zijn dan 100 tekens.")]
        public string Naam { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Stad is verplicht.")]
        //[StringLength(100, ErrorMessage = "De stad mag niet langer zijn dan 100 tekens.")]
        public string Stad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kies een kantine")]
        public int KantineId { get; set; }

        [Display(Name = "Alcohol")]
        public bool Is18 { get; set; }

        [Display(Name = "Warm")]
        public bool IsWarm {  get; set; }

        [Required(ErrorMessage = "Prijs is verplicht.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Prijs moet groter zijn dan 0.")]
        public double Prijs { get; set; }

        [StringLength(50, ErrorMessage = "Het type mag niet langer zijn dan 50 tekens.")]
        public string? Type { get; set; }




        public Student? ReservedBy {  get; set; } = null!;

        public DateTime? ReserveringDatum { get; set; }

        public DateTime EindDatum { get; set; }

        public int? UserKantineId { get; set; } = null;

        // Eventueel een lijst van Producten als je dat wilt
        public ICollection<Product> Producten { get; set; } = new List<Product>();
    }
}
