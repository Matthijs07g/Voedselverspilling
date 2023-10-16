using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Services;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class EFGamesRepository : IGamesRepository
    {
        private BordspellenDbContext context;

        public EFGamesRepository(BordspellenDbContext ctx)
        {
            this.context = ctx;
        }

        public IQueryable<Game> Games => context.Games;

        public IEnumerable<Game> GetGames()
        {
            throw new NotImplementedException();
        }

        public Game GetGameById(int id)
        {
            throw new NotImplementedException();
        }

        public Game AddGame(Game game)
        {
            throw new NotImplementedException();
        }

        public Game UpdateGame(Game game)
        {
            throw new NotImplementedException();
        }

        public void DeleteGame(int id)
        {
            throw new NotImplementedException();
        }
    }
}
