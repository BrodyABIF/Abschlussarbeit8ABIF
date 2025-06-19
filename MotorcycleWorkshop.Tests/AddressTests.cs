using MotorcycleWorkshop.model;

namespace MotorcycleWorkshop
{
    public class AddressTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            var street = "Test Street";
            var city = "Test City";
            var postalCode = "12345";

            var address = new Address(street, city, postalCode);

            Assert.Equal(street, address.Street);
            Assert.Equal(city, address.City);
            Assert.Equal(postalCode, address.PostalCode);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenStreetIsEmpty()
        {
            var city = "Test City";
            var postalCode = "12345";

            Assert.Throws<ArgumentException>(() => new Address("", city, postalCode));
        }
    }
}