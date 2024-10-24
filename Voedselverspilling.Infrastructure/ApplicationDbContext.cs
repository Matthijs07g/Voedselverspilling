﻿using Microsoft.EntityFrameworkCore;
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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Studenten { get; set; }
        public DbSet<Pakket> Pakketten { get; set; }
        public DbSet<Kantine> Kantines { get; set; }
        public DbSet<Reservering> Reserveringen { get; set; }
        public DbSet<Product> Producten { get; set; }
        public DbSet<KantineWorker> Medewerker { get; set; }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //    optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Voedselverspilling;Trust Server Certificate=True;Integrated Security=True; Encrypt=False");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var student1 = new Student()
            {
                Id = 1,
                Naam = "Matthijs van Gastel",
                GeboorteDatum = new DateOnly(2002, 11, 7),
                StudentNummer = 2186230,
                Emailaddress = "mmj.vangastel@student.avans.nl",
                Stad = "Breda",
            };

            var student2 = new Student()
            {
                Id = 2,
                Naam = "Theo Jansen",
                GeboorteDatum = new DateOnly(2010, 5, 15),
                StudentNummer = 2286230,
                Emailaddress = "t.jansen@student.avans.nl",
                Stad = "Breda"
            };

            var werker1 = new KantineWorker()
            {
                Id = 1,
                Naam = "Ingrid Jansen",
                PersoneelsNummer = 1,
                Email = "i.jansen@avans.nl",
                Stad = "Breda",
                KantineId = 1
            };

            var werker2 = new KantineWorker()
            {
                Id = 2,
                Naam = "Henk van Basten",
                PersoneelsNummer = 2,
                Email = "h.basten@avans.nl",
                Stad = "Breda",
                KantineId = 2
            };

            var product1 = new Product()
            {
                Id = 1,
                Naam = "Broodje zalm",
                IsAlcohol = false,
                Foto = "https://www.broodje.nl/wp-content/uploads/2022/03/lunch-broodje-zalm-luxe.jpg"
            };

            var kantineLA = new Kantine()
            {
                Id = 1,
                Stad = "Breda",
                Locatie = "LA",
                IsWarm = true,
            };

            var kantineLC = new Kantine()
            {
                Id = 2,
                Stad = "Breda",
                Locatie = "LC",
                IsWarm = false,
            };

            var pakket1 = new Pakket()
            {
                Id = 1,
                Naam = "Zee eten",
                ProductenId = new List<int> { 1 },
                Stad = "Breda",
                KantineId = 1,
                Is18 = false,
                IsWarm = true,
                Prijs = 10.99,
                Type = "Warm"
            };

            var reservering1 = new Reservering()
            {
                ReserveringId = 1,
                ReservaringDatum = new DateTime(2024, 9, 10),
                IsOpgehaald = false,
                StudentId = 1,
                PakketId = 1,
            };


            modelBuilder.Entity<Student>().HasData(
                student1,
                student2
                );

            modelBuilder.Entity<Product>().HasData(
                product1
                );

            modelBuilder.Entity<Kantine>().HasData(
                kantineLA,
                kantineLC
                );

            modelBuilder.Entity<Pakket>().HasData(
                pakket1
                );

            modelBuilder.Entity<Reservering>().HasData(
                reservering1
                );

            modelBuilder.Entity<KantineWorker>().HasData(
                werker1,
                werker2
                );
        }
    }
}
