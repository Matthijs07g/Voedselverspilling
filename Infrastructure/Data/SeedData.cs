using Domain.Models.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data
{
    public class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            BordspellenDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<BordspellenDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Games.Any())
            {
                Console.WriteLine("adding data");
                context.Games.AddRange(
                    
                    new Game
                    {
                        Name = "Monopoly",
                        Description = "test",
                        Adult = false,
                        Genre = Genre.Actie,
                        Picture = "test3",
                        Type = "bordspel"
                    },
                    new Game
                    {
                        Name = "Levensweg",
                        Description = "test",
                        Adult = false,
                        Genre = Genre.Actie,
                        Picture = "test3",
                        Type = "bordspel"
                    },
                    new Game
                    {
                        Name = "Stratego",
                        Description = "test",
                        Adult = false,
                        Genre = Genre.Actie,
                        Picture = "test3",
                        Type = "bordspel"
                    }
                );
                context.SaveChanges();
                Console.WriteLine("added data");
            }
        }
    }
}
