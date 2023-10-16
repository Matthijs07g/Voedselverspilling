using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IGamesRepository
    {

        IQueryable<Game> Games { get; }

        public IEnumerable<Game> GetGames();

        public Game GetGameById(int id);

        public Game AddGame(Game game);

        public Game UpdateGame(Game game);

        public void DeleteGame(int id);




    }
}
