using Domain.Models;
using Domain.Models.Enums;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BordspelAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FoodController : ControllerBase
    {
        private IFoodRepository _foodRepository;

        public FoodController(IFoodRepository foodRepository)
        {
            _foodRepository = foodRepository ?? throw new ArgumentNullException(nameof(foodRepository));
        }
        [HttpGet(Name = "GetAllFood")]
        public IEnumerable<Food> GetAllFood()
        {
             return _foodRepository.GetFoods();
        }

        [HttpGet(Name = "GetFood")]
        public Food GetFood(int id)
        {
            return _foodRepository.GetFoodById(id);
        }

        [HttpPost(Name = "AddFood")]
        public Food addFood(Food food)
        {
            return _foodRepository.AddFood(food);
        }

        [HttpDelete(Name = "DeleteFood")]
        public void deleteFood(int id)
        {
            _foodRepository.DeleteFoodById(id);
        }

        [HttpPut(Name = "updateFood")]  //kan ook patch ipv put
        public Food putFood(Food food)
        {
            return _foodRepository.UpdateFood(food);
        }





        private Exception NotImplementedException()
        {
            throw new NotImplementedException();
        }
    }
}
