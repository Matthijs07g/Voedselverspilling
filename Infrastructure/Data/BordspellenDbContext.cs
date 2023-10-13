using Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data
{
    public class BordspellenDbContext : DbContext
    {
        public BordspellenDbContext(DbContextOptions<BordspellenDbContext> options) : base(options) { }

        public DbSet<Game> Games => Set<Game>();
        public DbSet<Food> Foods => Set<Food>();
        public DbSet<GameEvent> GameEvents => Set<GameEvent>();
        public DbSet<Person> Persons => Set<Person>();
    }
}
