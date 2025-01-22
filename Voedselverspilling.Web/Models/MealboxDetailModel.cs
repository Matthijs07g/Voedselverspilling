using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.Web.Models
{
    public class MealboxDetailModel
    {
        public Pakket MealboxModel { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
