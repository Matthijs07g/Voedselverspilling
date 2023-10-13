using Domain.Models;
using Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BordspelAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameEventEventController : ControllerBase
    {
        [HttpGet(Name = "GetAllGameEvents")]
        public IEnumerable<GameEvent> GetGameEvents()
        {
            throw NotImplementedException();
        }

        [HttpGet(Name = "GetGameEvent")]
        public GameEvent GetGameEvent(int id)
        {
            throw NotImplementedException();
        }

        [HttpPost(Name = "AddGameEvent")]
        public GameEvent AddGameEvent()
        {
            throw NotImplementedException();
        }

        [HttpDelete(Name = "DeleteGameEvent")]
        public GameEvent DeleteGameEvent(int id)
        {
            throw NotImplementedException();
        }

        [HttpPut(Name = "updateGameEvent")]  //kan ook patch ipv put
        public GameEvent PutGameEvent()
        {
            throw NotImplementedException();
        }





        private Exception NotImplementedException()
        {
            throw new NotImplementedException();
        }
    }
}
