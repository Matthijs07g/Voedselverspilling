using Domain.Models;
using Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BordspelAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FoodController : ControllerBase
    {

        public FoodController()
        {

        }
        [HttpGet(Name = "GetAllFood")]
        public IEnumerable<Food> GetFood()
        {
            throw NotImplementedException();
        }

        [HttpGet(Name = "GetFood")]
        public Food GetFood(int id)
        {
            throw NotImplementedException();
        }

        [HttpPost(Name = "AddFood")]
        public Food addFood(string Name, Boolean Lactose, Boolean Nuts, Boolean Vegetarian, Boolean Alcohol)
        {
            throw NotImplementedException();
        }

        [HttpDelete(Name = "DeleteFood")]
        public Food deleteFood(int id)
        {
            throw NotImplementedException();
        }

        [HttpPut(Name = "updateFood")]  //kan ook patch ipv put
        public Food putFood(int id, string newName, Boolean newLactose, Boolean newNuts, Boolean newVegetarian, Boolean newAlcohol)
        {
            throw NotImplementedException();
        }





        private Exception NotImplementedException()
        {
            throw new NotImplementedException();
        }
    }
}
