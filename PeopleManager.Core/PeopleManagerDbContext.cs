using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PeopleManager.Model;

namespace PeopleManager.Core
{
    public class PeopleManagerDbContext : IdentityDbContext
    {

        public PeopleManagerDbContext(DbContextOptions<PeopleManagerDbContext> options) : base(options)
        {

        }

        public DbSet<Person> People => Set<Person>();
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.ResponsiblePerson)
                .WithMany(p => p.ResponsibleForVehicles)
                .HasForeignKey(v => v.ResponsiblePersonId)
                .IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }

        public void Seed()
        {
            AddDefaultUser();
            AddPeople();
            AddVehicles();

            SaveChanges();

            AssignRandomVehicles();

            SaveChanges();
        }

        private void AddDefaultUser()
        {
            var bavoUserEmail = "bavo.ketels@vives.be";
            var bavoUser = new IdentityUser
            {
                Email = bavoUserEmail,
                NormalizedEmail = bavoUserEmail.ToUpper(),
                UserName = bavoUserEmail,
                NormalizedUserName = bavoUserEmail.ToUpper(),
                ConcurrencyStamp = "9d898cd7-2227-44ed-8fb2-3468d2145aae",
                SecurityStamp = "N7URV5X4PI2IFC3I32AVDCDC4Y7XSMAQ",
                PasswordHash =
                    "AQAAAAIAAYagAAAAEL5uZw6smTCysavHMSzc4In30igjGfcG7B6b3rFgDpiTl33+D7dbEjsEevWd2a6rIQ==", //Test123$
            };
            Users.Add(bavoUser);
        }

        private void AddPeople()
        {
            var bavoPerson = new Person
            {
                FirstName = "Bavo",
                LastName = "Ketels",
                Email = "bavo.ketels@vives.be",
                Description = "Lector"
            };
            var wimPerson = new Person
            {
                FirstName = "Wim",
                LastName = "Engelen",
                Email = "wim.engelen@vives.be",
                Description = "Opleidingshoofd"
            };

            People.Add(bavoPerson);
            People.Add(wimPerson);

            for (int i = 0; i <= 1000; i++)
            {
                People.Add(new Person
                {
                    FirstName = $"First name {i}",
                    LastName = $"Last name {i}",
                    Description = $"Description for person {i}",
                    Email = $"test{i}@test.com"
                });
            }
        }

        private void AddVehicles()
        {

            for (int i = 0; i <= 1000; i++)
            {
                Vehicles.Add(new Vehicle
                {
                    Brand = $"Brand {i}",
                    Type = $"Type {i}",
                    LicensePlate = $"AAA-{i}"
                });
            }
        }

        private void AssignRandomVehicles()
        {
            var random = new Random();
            foreach (var vehicle in Vehicles)
            {
                //Diceroll 50% chance
                var chance = random.Next(2);
                if (chance == 0)
                {
                    continue;
                }

                //random Person
                var randomPerson = People.OrderBy(r => Guid.NewGuid()).FirstOrDefault();
                if (randomPerson is null)
                {
                    continue;
                }

                vehicle.ResponsiblePerson = randomPerson;
            }
        }
    }
}
