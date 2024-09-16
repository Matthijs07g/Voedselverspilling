using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voedselverspilling.Domain.Models
{
    public class Reservering
    {
        public int ReservationId { get; set; }
        public DateTime ReservationDate { get; set; }
        public Boolean Collected { get; set; }

        public required Student Student { get; set; }
        public required Pakket Pakket { get; set; }
    }

}
