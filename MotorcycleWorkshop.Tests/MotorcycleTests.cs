using MotorcycleWorkshop.model;

namespace MotorcycleWorkshop
{
    public class MotorcycleTests
    {
        [Fact]
        public void UpdateMileage_ShouldUpdateMileage_WhenMileageIsHigher()
        {
            var motorcycle = (Motorcycle)Activator.CreateInstance(typeof(Motorcycle), true)!;
            motorcycle.UpdateMileage(5000);

            motorcycle.UpdateMileage(10000);

            Assert.Equal(10000, motorcycle.Mileage);
        }

        [Fact]
        public void UpdateMileage_ShouldThrowArgumentException_WhenMileageIsLower()
        {
            var motorcycle = (Motorcycle)Activator.CreateInstance(typeof(Motorcycle), true)!;

            motorcycle.UpdateMileage(5000);

            Assert.Throws<ArgumentException>(() => motorcycle.UpdateMileage(4000));
        }

        [Fact]
        public void GetMaintenanceStatus_ShouldReturnCorrectStatus()
        {
            var motorcycle = (Motorcycle)Activator.CreateInstance(typeof(Motorcycle), true)!;

            motorcycle.UpdateMileage(12000);

            var status = motorcycle.GetMaintenanceStatus();

            Assert.Equal("Needs Maintenance", status);
        }
    }
}