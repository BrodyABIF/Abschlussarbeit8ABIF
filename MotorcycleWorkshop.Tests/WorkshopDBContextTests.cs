using Microsoft.EntityFrameworkCore;
using MotorcycleWorkshop.Infrastructure;
using Microsoft.Data.Sqlite;

namespace MotorcycleWorkshop;

public class WorkshopDBContextTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly WorkshopDBContext _context;

    public WorkshopDBContextTests()
    {
        // Verwende SQLite im In-Memory-Modus
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<WorkshopDBContext>()
            .UseSqlite(_connection)
            .EnableSensitiveDataLogging()
            .Options;

        _context = new WorkshopDBContext(options);
        _context.Database.EnsureCreated();

        SeedTestData();
    }

    private void SeedTestData()
    {
        using var transaction = _context.Database.BeginTransaction();
        
        try
        {
            var customer = model.Customer.Create(
                "Test Customer",
                "Test Street",
                "Test City",
                "12345",
                "123-456-789",
                "test@example.com"
            );
            _context.Customers.Add(customer);

            var mechanic = model.Mechanic.Create(
                "Test Mechanic",
                "Mech Street",
                "Mech City",
                "54321",
                "987-654-321",
                "mech@example.com",
                "Master",
                50m
            );
            _context.Mechanics.Add(mechanic);

            var repair = model.Repair.Create(customer, mechanic, DateTime.Today.AddDays(1));
            _context.Repairs.Add(repair);

            _context.SaveChanges();
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    [Fact]
    public void Database_ShouldSeedDataCorrectly()
    {
        var customers = _context.Customers.ToList();
        var mechanics = _context.Mechanics.ToList();
        var repairs = _context.Repairs.ToList();

        Assert.NotEmpty(customers);
        Assert.NotEmpty(mechanics);
        Assert.NotEmpty(repairs);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}