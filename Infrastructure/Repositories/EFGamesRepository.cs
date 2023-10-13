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
    }
}
