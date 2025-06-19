using MotorcycleWorkshop.model;

namespace MotorcycleWorkshop
{
    public class CustomerTests
    {
        [Fact]
        public void GetRepairHistory_ShouldReturnRepairsInDescendingOrder()
        {
            var customer = (Customer)Activator.CreateInstance(typeof(Customer), true)!;
            var repair1 = (Repair)Activator.CreateInstance(typeof(Repair), true)!;
            var repair2 = (Repair)Activator.CreateInstance(typeof(Repair), true)!;

            repair1.RepairDate = new DateTime(2023, 1, 1);
            repair2.RepairDate = new DateTime(2023, 2, 1);

            customer.Repairs.Add(repair1);
            customer.Repairs.Add(repair2);

            var history = customer.GetRepairHistory();

            Assert.Equal(new[] { repair2, repair1 }, history);
        }
    }
}