using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class GameEvent
    {
        [Key]
        public int Id { get; set; }

        [Key]
        public Person? Organiser { get; set; }

        [Required(ErrorMessage ="Geef de locatie waar de spelavond is")]
        public string? Location { get; set; }

        public ICollection<Person>? Participants { get; set; }


        public int MaxAmountParticipants { get; set; }

        public ICollection<Game>? Games { get; set; }

        public ICollection<Food>? Foods { get; set; }
    }
}
