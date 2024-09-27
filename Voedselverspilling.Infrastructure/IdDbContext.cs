using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.Infrastructure
{
    public class IdDbContext : IdentityDbContext<AppIdentity>
    {
        public IdDbContext(DbContextOptions<IdDbContext> options) : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=VoedselverspillingID;Trust Server Certificate=True;Integrated Security=True; Encrypt=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //var workerId1 = new AppIdentity()
            //{
            //    Id = 1,
            //    Email = "i.jansen@avans.nl",
            //    Rol = "Worker"
            //};

            //var studentId1 = new AppIdentity()
            //{
            //    Id = 2,
            //    Email = "mmj.vangastel@student.avans.nl",
            //    Rol = "Student"
            //};
        }
    }
}
