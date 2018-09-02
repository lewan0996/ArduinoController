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

            modelBuilder.Entity<Procedure>().HasKey(p => p.Id);

            modelBuilder.Entity<Procedure>()
                .HasOne(p => p.Device)
                .WithMany();

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Procedures)
                .WithOne()
                .HasForeignKey(p => p.UserId)
                .IsRequired();

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Devices)
                .WithOne()
                .HasForeignKey(d => d.UserId)
                .IsRequired();

            modelBuilder.Entity<ArduinoDevice>().HasKey(d => d.Id);
        }
    }
}
