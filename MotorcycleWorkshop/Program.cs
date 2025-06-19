using Microsoft.EntityFrameworkCore;
using MotorcycleWorkshop.Infrastructure;

namespace MotorcycleWorkshop
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<WorkshopDBContext>()
                .UseSqlite("Data Source=workshop.db")
                .Options;

            using var context = new WorkshopDBContext(options);

            try
            {
                context.Database.Migrate();  //----------- Migration TODO

                Console.WriteLine("Database created and ready for use.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while applying the migration: {ex.Message}");
            }
        }
    }
}