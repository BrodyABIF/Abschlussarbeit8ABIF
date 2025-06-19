using MotorcycleWorkshop.model;

namespace MotorcycleWorkshop;

public class PartTests
{
    [Fact]
    public void SetName_ShouldThrowException_WhenNameIsEmpty()
    {
        var part = (Part)Activator.CreateInstance(typeof(Part), true)!;

        Assert.Throws<ArgumentException>(() => part.Name = "");
    }

    [Fact]
    public void SetName_ShouldSetName_WhenValid()
    {
        var part = (Part)Activator.CreateInstance(typeof(Part), true)!;

        part.Name = "Oil Filter";

        Assert.Equal("Oil Filter", part.Name);
    }

    [Fact]
    public void SetPrice_ShouldSetPriceCorrectly()
    {
        var part = (Part)Activator.CreateInstance(typeof(Part), true)!;

        part.Price = 20.99m;

        Assert.Equal(20.99m, part.Price);
    }
}