using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Studenten { get; set; }
        public DbSet<Pakket> Pakketten { get; set; }
        public DbSet<Kantine> Kantines { get; set; }
    }
}
