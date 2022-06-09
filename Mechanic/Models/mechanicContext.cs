using System.Configuration;
using Microsoft.EntityFrameworkCore;


namespace Mechanic.Models
{
    public partial class mechanicContext : DbContext
    {
        public mechanicContext()
        {
        }

        public mechanicContext(DbContextOptions<mechanicContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Part> Parts { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        public virtual DbSet<Vehicle> Vehicles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning Make sure you change the username and password sections of the connection string in the App.config file
                optionsBuilder.UseMySql(ConfigurationManager.ConnectionStrings["mechanic-db"].ConnectionString, ServerVersion.Parse("8.0.29-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer");

                entity.HasIndex(e => e.Name, "Name_Index");

                entity.Property(e => e.Contact).HasColumnType("text");

                entity.Property(e => e.Name).HasMaxLength(70);
            });

            modelBuilder.Entity<Part>(entity =>
            {
                entity.ToTable("part");

                entity.HasIndex(e => e.ServiceId, "Service_Id");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Price).HasPrecision(17, 2);

                entity.Property(e => e.ServiceId).HasColumnName("Service_Id");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.Parts)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("part_ibfk_1");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("service");

                entity.HasIndex(e => e.VehicleId, "Vehicle_Id");

                entity.Property(e => e.Details).HasColumnType("mediumtext");

                entity.Property(e => e.Fee).HasPrecision(17, 2);

                entity.Property(e => e.VehicleId).HasColumnName("Vehicle_Id");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.VehicleId)
                    .HasConstraintName("service_ibfk_1");
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.ToTable("vehicle");

                entity.HasIndex(e => e.CustomerId, "Customer_Id");

                entity.HasIndex(e => e.LicensePlate, "LicensePlate")
                    .IsUnique();

                entity.Property(e => e.Color).HasMaxLength(30);

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.LicensePlate).HasMaxLength(15);

                entity.Property(e => e.Make).HasMaxLength(50);

                entity.Property(e => e.Model).HasMaxLength(50);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("vehicle_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
