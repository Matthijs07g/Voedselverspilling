namespace Voedselverspilling.Web.Models
{
    public class MealboxDetailModel
    {
        public MealboxModel MealboxModel { get; set; }
        public List<ProductModel> Products { get; set; } = new List<ProductModel>();
    }
}
