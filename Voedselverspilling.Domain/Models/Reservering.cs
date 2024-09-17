using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voedselverspilling.Domain.Models
{
    public class Reservering
    {
        [Key]
        public int ReserveringId { get; set; }
        public DateTime ReservaringDatum { get; set; }
        public Boolean IsOpgehaald { get; set; }
        public DateTime TijdOpgehaald { get; set; }

        public required Student Student { get; set; }
        public required Pakket Pakket { get; set; }
    }

}
