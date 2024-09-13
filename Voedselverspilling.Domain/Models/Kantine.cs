using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voedselverspilling.Domain.Models
{
    public class Kantine
    {
        public string? Stad {  get; set; }
        public string? Locatie { get; set; }
        public Boolean WarmMeals { get; set; }
    }
}
