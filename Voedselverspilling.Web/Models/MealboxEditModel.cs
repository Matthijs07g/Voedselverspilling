using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.Web.Models
{
    public class MealboxEditModel
    {
        public Pakket Mealbox { get; set; } = null!;
        public IEnumerable<Product> AvailableProducts { get; set; } = new List<Product>();
        public ICollection<int> SelectedProductIds { get; set; } = new List<int>();
    }
}
