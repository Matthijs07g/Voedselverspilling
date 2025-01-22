using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.Web.Models
{
    public class MealboxCreateModel
    {
        public Pakket Mealbox { get; set; }
        public IEnumerable<Product> AvailableProducts { get; set; }
        public List<int> SelectedProductIds { get; set; } = new List<int>(); // List to hold selected product IDs
    }

}
