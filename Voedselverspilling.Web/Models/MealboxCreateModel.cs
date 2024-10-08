namespace Voedselverspilling.Web.Models
{
    public class MealboxCreateModel
    {
        public MealboxModel Mealbox { get; set; } = new MealboxModel();
        public List<ProductModel> AvailableProducts { get; set; } = new List<ProductModel>();
        public List<int> SelectedProductIds { get; set; } = new List<int>(); // List to hold selected product IDs
    }

}
