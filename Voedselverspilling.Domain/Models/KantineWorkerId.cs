using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voedselverspilling.Domain.Models
{
    public class KantineWorkerId
    {
        public required int WorkNumber { get; set; }
        public required string Pass {  get; set; }
        public required KantineWorker KantineWorker { get; set; }
    }
}
