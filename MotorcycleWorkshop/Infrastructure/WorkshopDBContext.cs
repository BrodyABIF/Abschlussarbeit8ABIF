using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MotorcycleWorkshop.model;

namespace MotorcycleWorkshop.Infrastructure;

public class WorkshopDBContext : DbContext
{
    public WorkshopDBContext(DbContextOptions<WorkshopDBContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Mechanic> Mechanics { get; set; }
    public DbSet<Repair> Repairs { get; set; }
    public DbSet<Part> Parts { get; set; }
    public DbSet<Motorcycle> Motorcycles { get; set; }
    public DbSet<Invoice> Invoices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasKey(p => p.Id);
        modelBuilder.Entity<Person>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Person>()
            .HasAlternateKey(p => p.AlternateId);
        
// ------------------- Vererbung mit Discriminator 
        modelBuilder.Entity<Person>()
            .HasDiscriminator<string>("PersonType")
            .HasValue<Customer>("Customer")
            .HasValue<Mechanic>("Mechanic");

        
//--------------------- GUID as alternate Key
        modelBuilder.Entity<Motorcycle>()
            .HasKey(m => m.Id);
        modelBuilder.Entity<Motorcycle>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Motorcycle>()
            .HasAlternateKey(m => m.AlternateId);

        modelBuilder.Entity<Part>()
            .HasKey(p => p.Id);
        modelBuilder.Entity<Part>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Part>()
            .HasAlternateKey(p => p.AlternateId);

        modelBuilder.Entity<Repair>()
            .HasKey(r => r.Id);
        modelBuilder.Entity<Repair>()
            .Property(r => r.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Repair>()
            .HasAlternateKey(r => r.AlternateId);

        modelBuilder.Entity<Repair>()
            .HasMany(r => r.UsedParts)
            .WithMany()
            .UsingEntity<Dictionary<int, int>>(
                "RepairParts",
                r => r.HasOne<Part>()
                    .WithMany()
                    .HasForeignKey("PartId"),
                rp => rp.HasOne<Repair>()
                    .WithMany()
                    .HasForeignKey("RepairId")
            );
        
        //------------ Invoice Beziehung TODO
        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.Repair)
            .WithMany()                        // oder .WithOne(r => r.Invoice) wenn Du in Repair eine Nav-Property anlegst
            .HasForeignKey(i => i.RepairId);  // jetzt stimmt’s vom Typ her
        modelBuilder.Entity<Invoice>()
            .HasKey(i => i.Id);
        modelBuilder.Entity<Invoice>()
            .Property(i => i.Id)
            .ValueGeneratedOnAdd();


//------------ Beidseitige Navigation
        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Motorcycles)
            .WithOne(m => m.Owner)
            .HasForeignKey(m => m.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Mechanic>()
            .HasMany(m => m.Repairs)
            .WithOne(r => r.Mechanic)
            .HasForeignKey(r => r.MechanicId)
            .OnDelete(DeleteBehavior.Cascade);

        
//................... Value Object TODO
        modelBuilder.Entity<Customer>().OwnsOne(c => c.Address, address =>
        {
            address.Property(a => a.Street).HasColumnName("CustomerStreet");
            address.Property(a => a.City).HasColumnName("CustomerCity");
            address.Property(a => a.PostalCode).HasColumnName("CustomerPostalCode");
        });

        modelBuilder.Entity<Mechanic>().OwnsOne(m => m.Address, address =>
        {
            address.Property(a => a.Street).HasColumnName("MechanicStreet");
            address.Property(a => a.City).HasColumnName("MechanicCity");
            address.Property(a => a.PostalCode).HasColumnName("MechanicPostalCode");
        });


//-------------------SEED TODO
        var customerId = Guid.NewGuid();
        var mechanicId = Guid.NewGuid();
        var motorcycleId = Guid.NewGuid();
        var repairId = Guid.NewGuid();
        var part1Id = Guid.NewGuid();
        var part2Id = Guid.NewGuid();

        modelBuilder.Entity<Customer>().HasData(
            new
            {
                Id = 1,
                AlternateId = customerId,
                Name = "Customer Horst",
                PhoneNumber = "012345",
                Email = "customer@mail.at",
            }
        );

        modelBuilder.Entity<Customer>().OwnsOne(c => c.Address).HasData(
            new
            {
                CustomerId = 1,
                Street = "Customerstrasse 1",
                City = "Vienna",
                PostalCode = "1010"
            }
        );

        modelBuilder.Entity<Mechanic>().HasData(
            new
            {
                Id = 2,
                AlternateId = mechanicId,
                Name = "Jane Smith",
                PhoneNumber = "012345",
                Email = "jane.smith@example.com",
                HourlyRate = 50.0m,
                Certification = "Certified Mechanic",
            }
        );

        modelBuilder.Entity<Mechanic>().OwnsOne(m => m.Address).HasData(
            new
            {
                MechanicId = 2,
                Street = "Repair St. 456",
                City = "Vienna",
                PostalCode = "1020"
            }
        );

        modelBuilder.Entity<Motorcycle>().HasData(
            new
            {
                Id = 1,
                AlternateId = motorcycleId,
                Model = "Honda CBR600RR",
                Year = 2020,
                Mileage = 5000.00M,
                OwnerId = 1
            }
        );

        modelBuilder.Entity<Part>().HasData(
            new
            {
                Id = 1,
                AlternateId = part1Id,
                Name = "Oil Filter",
                Price = 20.99m
            },
            new
            {
                Id = 2,
                AlternateId = part2Id,
                Name = "Brake Pads",
                Price = 45.50m
            }
        );

        modelBuilder.Entity<Repair>()
            .HasData(
                new
                {
                    Id = 1,
                    AlternateId = repairId,
                    RepairDate = new DateTime(2023, 12, 15),
                    CustomerId = 1,
                    MechanicId = 2
                }
            );

        modelBuilder.Entity("RepairParts").HasData(
            new
            {
                RepairId = 1,
                PartId = 1
            },
            new
            {
                RepairId = 1,
                PartId = 2
            }
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)  // Nur konfigurieren, wenn noch nicht konfiguriert
        {
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseSqlite("Data Source=motorcycle.db");
        }

        optionsBuilder.EnableSensitiveDataLogging();
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

}