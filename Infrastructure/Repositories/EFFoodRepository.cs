using Domain.Models;
using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EFFoodRepository : IFoodRepository
    {

        public IEnumerable<Food> GetFoods()
        {
            throw new NotImplementedException();
        }

        public Food GetFoodById(int id)
        {
            throw new NotImplementedException();
        }

        public Food AddFood(Food food)
        {
            throw new NotImplementedException();
        }

        public Food UpdateFood(Food food)
        {
            throw new NotImplementedException();
        }

        public void DeleteFoodById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
