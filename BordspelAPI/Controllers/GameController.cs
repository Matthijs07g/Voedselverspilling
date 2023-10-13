using Domain.Models;
using Domain.Models.Enums;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BordspelAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        [HttpGet(Name ="GetAllGames")]
        public IEnumerable<Game> GetGames()
        {
            throw NotImplementedException();   
        }

        [HttpGet(Name ="GetGame")]
        public Game GetGame (int id) 
        { 
            throw NotImplementedException(); 
        }

        [HttpPost(Name ="AddGame")]
        public Game addGame(string Name, string Desc, Genre genre, Boolean adult, string picture, string type)
        {
            throw NotImplementedException();
        }

        [HttpDelete(Name ="DeleteGame")]
        public Game deleteGame(int id)
        {
            throw NotImplementedException();
        }

        [HttpPut(Name ="updateGame")]  //kan ook patch ipv put
        public Game putGame(int id, string newName, string newDesc, Genre newGenre, Boolean newAdult, string newpicture, string newtype)
        {
            throw NotImplementedException();
        }
             




        private Exception NotImplementedException()
        {
            throw new NotImplementedException();
        }
    }
}
