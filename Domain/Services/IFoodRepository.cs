using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IFoodRepository
    {
        public IEnumerable<Food> GetFoods();

        public Food GetFoodById(int id);

        public Food AddFood(Food food);

        public Food UpdateFood(Food food);

        public void DeleteFoodById(int id);
    }
}
