using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Food
    {
        public int? Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Boolean Lactose { get; set; }

        public Boolean Nuts { get; set; }

        public Boolean Vegetarian { get; set; }

        public Boolean Alcohol { get; set; }
    }
}
