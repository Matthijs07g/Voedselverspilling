using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
        public DbSet<Student> Studenten { get; set; }
        public DbSet<Pakket> Pakketten { get; set; }
        public DbSet<Kantine> Kantines { get; set; }
        public DbSet<Product> Producten { get; set; }
        public DbSet<KantineWorker> Medewerker { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define Student-Pakket relationship if applicable
            modelBuilder.Entity<Pakket>()
                .HasOne(p => p.ReservedBy) // Assuming ReservedBy is a Student reference
                .WithMany() // If one Student can reserve multiple Pakketten
                .HasForeignKey(p => p.ReservedById); // Match your foreign key name

            // Define many-to-many relationship for Pakket-Product explicitly
            modelBuilder.Entity<Pakket>()
                .HasMany(p => p.Producten)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "PakketProduct",
                    x => x.HasOne<Product>().WithMany().HasForeignKey("ProductId"),
                    y => y.HasOne<Pakket>().WithMany().HasForeignKey("PakketId"),
                    xy =>
                    {
                        xy.HasKey("ProductId", "PakketId");
                        xy.HasData(
                            new { ProductId = 1, PakketId = 1 },
                            new { ProductId = 2, PakketId = 2 }
                        );
                    });

            // Seed data
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 1,
                    Naam = "Matthijs van Gastel",
                    GeboorteDatum = new DateOnly(2002, 11, 7),
                    StudentNummer = 2186230,
                    Emailaddress = "mmj.vangastel@student.avans.nl",
                    Stad = "Breda",
                },
                new Student
                {
                    Id = 2,
                    Naam = "Theo Jansen",
                    GeboorteDatum = new DateOnly(2010, 5, 15),
                    StudentNummer = 2286230,
                    Emailaddress = "t.jansen@student.avans.nl",
                    Stad = "Breda"
                });

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Naam = "Broodje zalm",
                    IsAlcohol = false,
                    Foto = "https://www.broodje.nl/wp-content/uploads/2022/03/lunch-broodje-zalm-luxe.jpg"
                },
                new Product
                {
                    Id = 2,
                    Naam = "Broodje kroket",
                    IsAlcohol = false,
                    Foto = "https://www.praktijkmik.nl/media/tz_portfolio_plus/article/cache/een-broodje-kroket-28_o.jpg"
                });

            modelBuilder.Entity<Kantine>().HasData(
                new Kantine
                {
                    Id = 1,
                    Stad = "Breda",
                    Locatie = "LA",
                    IsWarm = true,
                },
                new Kantine
                {
                    Id = 2,
                    Stad = "Breda",
                    Locatie = "LC",
                    IsWarm = false,
                });

            modelBuilder.Entity<KantineWorker>().HasData(
                new KantineWorker
                {
                    Id = 1,
                    Naam = "Ingrid Jansen",
                    PersoneelsNummer = 1,
                    Email = "i.jansen@avans.nl",
                    Stad = "Breda",
                    KantineId = 1
                },
                new KantineWorker
                {
                    Id = 2,
                    Naam = "Henk van Basten",
                    PersoneelsNummer = 2,
                    Email = "h.basten@avans.nl",
                    Stad = "Breda",
                    KantineId = 2
                });

            modelBuilder.Entity<Pakket>().HasData(
                new Pakket
                {
                    Id = 1,
                    Naam = "Zee eten",
                    Stad = "Breda",
                    KantineId = 1,
                    Is18 = false,
                    IsWarm = true,
                    Prijs = 10.99,
                    Type = "Warm",
                    EindDatum = new DateTime(2025, 1, 22)
                },
                new Pakket
                {
                    Id = 2,
                    Naam = "Broodje kroket",
                    Stad = "Breda",
                    KantineId = 2,
                    Is18 = false,
                    IsWarm = true,
                    Prijs = 6.99,
                    Type = "Brood",
                    ReservedById = 1, // Link to student
                    ReserveringDatum = new DateTime(2025, 1, 15),
                    EindDatum = new DateTime(2025, 1, 22)
                });
        }

    }
}
