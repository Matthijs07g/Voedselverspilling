namespace Voedselverspilling.Web.Models
{
    public class MealboxEditModel
    {
        public MealboxModel Mealbox { get; set; } = new MealboxModel();
        public List<ProductModel> AvailableProducts { get; set; } = new List<ProductModel>();
        public List<int> SelectedProductIds { get; set; } = new List<int>();
    }
}
