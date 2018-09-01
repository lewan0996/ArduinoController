using ArduinoController.Core.Models;
using ArduinoController.Core.Models.Commands;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArduinoController.DataAccess
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<Command> Commands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Command>().HasKey(c => c.Id);
            modelBuilder.Entity<AnalogWriteCommand>();
            modelBuilder.Entity<DigitalWriteCommand>();
            modelBuilder.Entity<NegateCommand>();
            modelBuilder.Entity<WaitCommand>();

            modelBuilder.Entity<Procedure>().HasKey(p => new { p.UserId, p.Name });

            modelBuilder.Entity<Procedure>()
                .HasOne(p => p.Device)
                .WithMany();

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Procedures)
                .WithOne()
                .IsRequired();

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Devices)
                .WithOne()
                .IsRequired();

            modelBuilder.Entity<ArduinoDevice>().HasKey(d => d.MacAddress);
        }
    }
}
