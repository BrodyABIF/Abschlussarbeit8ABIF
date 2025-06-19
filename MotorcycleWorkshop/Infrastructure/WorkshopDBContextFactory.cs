using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MotorcycleWorkshop.Infrastructure;

/// <summary>
/// Creates instances for WorkshopDBCOntext
/// </summary>
public class WorkshopDBContextFactory : IDesignTimeDbContextFactory<WorkshopDBContext>
{
    public WorkshopDBContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WorkshopDBContext>();
        optionsBuilder.UseSqlite("Data Source=workshop.db");

        return new WorkshopDBContext(optionsBuilder.Options);
    }
}