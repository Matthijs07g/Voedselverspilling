using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voedselverspilling.Domain.Models
{
    public class KantineWorker
    {
        public required string Name { get; set; }
        public required int workNumber { get; set; }
        public required string City {  get; set; }
        public required string Location { get; set; }
    }
}
