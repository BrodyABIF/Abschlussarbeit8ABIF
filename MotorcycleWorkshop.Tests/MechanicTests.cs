using Microsoft.EntityFrameworkCore;
using MotorcycleWorkshop.Infrastructure;
using MotorcycleWorkshop.model;

namespace MotorcycleWorkshop
{
    public class MechanicTests
    {
        [Fact]
        public void CalculateTotalHours_ShouldReturnCorrectHours()
        {
            var repair1 = (Repair)Activator.CreateInstance(typeof(Repair), true)!;
            var repair2 = (Repair)Activator.CreateInstance(typeof(Repair), true)!;
            var mechanic = Mechanic.Create(
                name: "Test Mechanic",
                street: "Test Street",
                city: "Test City",
                postalCode: "12345",
                phoneNumber: "123456789",
                email: "test@test.com",
                hourlyRate: 50m,
                certification: "New Certification"
            );

            repair1.RepairDate = new DateTime(2023, 1, 10);
            repair2.RepairDate = new DateTime(2023, 1, 20);

            repair1.AssignMechanic(mechanic);
            repair2.AssignMechanic(mechanic);


            mechanic.Repairs.Add(repair1);
            mechanic.Repairs.Add(repair2);

            var totalHours = mechanic.CalculateTotalHours(new DateTime(2023, 1, 1), new DateTime(2023, 1, 31));

            Assert.Equal(2, totalHours);
        }

        [Fact]
        public void GetAvailableMechanics_ShouldReturnMechanicsWithNoRepairsInDateRange()
        {
            var connection = new Microsoft.Data.Sqlite.SqliteConnection("Filename=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<WorkshopDBContext>()
                    .UseSqlite(connection)
                    .Options;

                using var context = new WorkshopDBContext(options);

                context.Database.EnsureDeleted();
                context.Database.Migrate();

                var mechanic1 = Mechanic.Create(
                    name: "Test Mechanic",
                    street: "Test Street",
                    city: "Test City",
                    postalCode: "12345",
                    phoneNumber: "123456789",
                    email: "test@test.com",
                    hourlyRate: 50m,
                    certification: "New Certification"
                );


                var mechanic2 = Mechanic.Create(
                    name: "Dummy Mechanic",
                    street: "Dummy Street",
                    city: "Dummy City",
                    postalCode: "67890",
                    phoneNumber: "987654321",
                    email: "Dummy@test.com",
                    hourlyRate: 50m,
                    certification: "New Certification"
                );


                var customer = (Customer)Activator.CreateInstance(typeof(Customer), true)!;
                customer.Name = "Customer X";
                customer.Email = "customerX@example.com";
                customer.PhoneNumber = "0123456789";

                context.Customers.Add(customer);
                context.Mechanics.Add(mechanic1);
                context.Mechanics.Add(mechanic2);
                context.SaveChanges();

                var repair = Repair.Create(
                    customer: customer,
                    mechanic: mechanic1,
                    repairDate: new DateTime(2023, 1, 10)
                );


                mechanic1.Repairs.Add(repair);
                context.Repairs.Add(repair);
                context.SaveChanges();

                var availableMechanics = mechanic2.GetAvailableMechanics(
                    new DateTime(2023, 1, 1),
                    new DateTime(2023, 1, 31),
                    context).ToList();

                Assert.Contains(mechanic2, availableMechanics);
                Assert.DoesNotContain(mechanic1, availableMechanics);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}