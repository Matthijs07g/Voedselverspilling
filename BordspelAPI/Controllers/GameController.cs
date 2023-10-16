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
        private IGamesRepository _gamesRepository;

        public GameController(IGamesRepository gamesRepository) 
        {
            _gamesRepository = gamesRepository ?? throw new ArgumentNullException(nameof(gamesRepository));
        }

        [HttpGet]
        public IEnumerable<Game> GetGames()
        {
           return _gamesRepository.GetGames().ToArray(); 
        }

        [HttpGet("{id}")]
        public Game GetGame (int id) 
        {
            return _gamesRepository.GetGameById(id);
        }

        [HttpPost(Name ="AddGame")]
        public Game addGame(Game game)
        {
            return _gamesRepository.AddGame(game);
        }

        [HttpDelete(Name ="DeleteGame")]
        public void deleteGame(int id)
        {
            _gamesRepository.DeleteGame(id);
        }

        [HttpPut(Name ="updateGame")]  //kan ook patch ipv put
        public Game putGame(Game game)
        {
            return _gamesRepository.UpdateGame(game);
        }
             




        private Exception NotImplementedException()
        {
            throw new NotImplementedException();
        }
    }
}
