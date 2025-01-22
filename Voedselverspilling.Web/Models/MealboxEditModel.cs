using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.Web.Models
{
    public class MealboxEditModel
    {
        public Pakket Mealbox { get; set; }
        public IEnumerable<Product> AvailableProducts { get; set; }
        public ICollection<int> SelectedProductIds { get; set; }
    }
}
