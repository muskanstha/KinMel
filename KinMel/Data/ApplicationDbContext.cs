using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KinMel.Models;

namespace KinMel.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<KinMel.Models.ClassifiedAd> ClassifiedAd { get; set; }

        public DbSet<KinMel.Models.Car> Car { get; set; }

        public DbSet<KinMel.Models.Mobile> Mobile { get; set; }

        public DbSet<KinMel.Models.Motorcycle> Motorcycle { get; set; }

        public DbSet<KinMel.Models.RealEstate> RealState { get; set; }

        public DbSet<KinMel.Models.Computer> Computer { get; set; }

        public DbSet<KinMel.Models.Jobs> Jobs { get; set; }

        public DbSet<KinMel.Models.BooksAndLearning> BooksAndLearning { get; set; }

        public DbSet<KinMel.Models.BeautyAndHealth> BeautyAndHealth { get; set; }

        public DbSet<KinMel.Models.Electronics> Electronics { get; set; }

        public DbSet<KinMel.Models.Furnitures> Furnitures { get; set; }

        public DbSet<KinMel.Models.Camera> Camera { get; set; }

        public DbSet<KinMel.Models.MusicInstruments> MusicInstruments { get; set; }

        public DbSet<KinMel.Models.PetsAndPetCare> PetsAndPetCare { get; set; }

        public DbSet<KinMel.Models.SportsAndFitness> SportsAndFitness { get; set; }

        public DbSet<KinMel.Models.TabletsAndIPads> TabletsAndIPads { get; set; }

        public DbSet<KinMel.Models.ToysAndGames> ToysAndGames { get; set; }

        public DbSet<KinMel.Models.TravelAndTours> TravelAndTours { get; set; }

        public DbSet<KinMel.Models.HelpAndServices> HelpAndServices { get; set; }

        public DbSet<KinMel.Models.MobileAccessories> MobileAccessories { get; set; }

        public DbSet<KinMel.Models.ComputerParts> ComputerParts { get; set; }

        public DbSet<KinMel.Models.ApparelsAndAccessories> ApparelsAndAccessories { get; set; }

        public DbSet<KinMel.Models.VehiclesParts> VehiclesParts { get; set; }

        public DbSet<KinMel.Models.Rating> Rating { get; set; }

        public DbSet<KinMel.Models.Notification> Notification { get; set; }

        public DbSet<KinMel.Models.Question> Question { get; set; }

        public DbSet<KinMel.Models.Answer> Answer { get; set; }
    }
}
