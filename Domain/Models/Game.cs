using Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Game
    {
        public int? Id { get; set; }

        public string? Name { get; set; } 

        public string? Description { get; set; }

        public Genre Genre { get; set; }

        public Boolean Adult { get; set; }

        public string? Picture { get; set; }

        public string? Type { get; set; }
    }
}
