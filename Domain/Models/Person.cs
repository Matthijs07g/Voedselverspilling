using Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Person
    {
        public int? Id { get; set; }

        public string? Emailaddress { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public Sex Sex { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
