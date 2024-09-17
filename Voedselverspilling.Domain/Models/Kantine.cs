using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voedselverspilling.Domain.Models
{
    public class Kantine
    {
        [Key]
        public int Id { get; set; }
        public string? Stad {  get; set; }
        public string? Locatie { get; set; }
        public Boolean IsWarm { get; set; }
    }
}
