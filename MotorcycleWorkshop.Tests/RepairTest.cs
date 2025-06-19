using MotorcycleWorkshop.model;

namespace MotorcycleWorkshop;

public class RepairTests
{
    [Fact]
    public void AddPart_ShouldAddPartToUsedParts()
    {
        var repair = (Repair)Activator.CreateInstance(typeof(Repair), true)!;
        var part = (Part)Activator.CreateInstance(typeof(Part), true)!;
        part.Name = "Oil Filter";
        part.Price = 20.99m;

        repair.AddPart(part);

        Assert.Contains(part, repair.UsedParts);
    }

    [Fact]
    public void RemovePart_ShouldRemovePartFromUsedParts()
    {
        var repair = (Repair)Activator.CreateInstance(typeof(Repair), true)!;
        var part = (Part)Activator.CreateInstance(typeof(Part), true)!;
        part.Name = "Brake Pads";
        part.Price = 45.50m;

        repair.AddPart(part);

        repair.RemovePart(part.Id);

        Assert.DoesNotContain(part, repair.UsedParts);
    }

    [Fact]
    public void CalculateTotalCost_ShouldReturnSumOfPartPrices()
    {
        var repair = (Repair)Activator.CreateInstance(typeof(Repair), true)!;
        var part1 = (Part)Activator.CreateInstance(typeof(Part), true)!;
        var part2 = (Part)Activator.CreateInstance(typeof(Part), true)!;

        part1.Name = "Oil Filter";
        part1.Price = 20.99m;

        part2.Name = "Brake Pads";
        part2.Price = 45.50m;

        repair.AddPart(part1);
        repair.AddPart(part2);

        var totalCost = repair.CalculateTotalCost();

        Assert.Equal(66.49m, totalCost);
    }

    [Fact]
    public void AssignMechanic_ShouldSetMechanic()
    {
        var repair = (Repair)Activator.CreateInstance(typeof(Repair), true)!;
        var mechanic = (Mechanic)Activator.CreateInstance(typeof(Mechanic), true)!;
        mechanic.Name = "John Doe";

        repair.AssignMechanic(mechanic);

        Assert.Equal(mechanic, repair.Mechanic);
    }

    [Fact]
    public void TryDestroyPart_ShouldRemovePartAndCreateDestroyedPart()
    {
        var repair = (Repair)Activator.CreateInstance(typeof(Repair), true)!;
        var part = (Part)Activator.CreateInstance(typeof(Part), true)!;

        part.Name = "Headlight";
        part.Price = 100.50m;

        repair.AddPart(part);

        var result = repair.TryDestroyPart(part.Id, DestroyedPart.ReasonEnum.UselessMechanic, out var destroyedPart);

        Assert.True(result, "Part should be destroyed.");
        Assert.NotNull(destroyedPart);
        Assert.Equal(DestroyedPart.ReasonEnum.UselessMechanic, destroyedPart.Reason);
        Assert.DoesNotContain(part, repair.UsedParts);
    }

    [Fact]
    public void TryDestroyPart_ShouldReturnFalseIfNoUsedPart()
    {
        var repair = (Repair)Activator.CreateInstance(typeof(Repair), true)!;
        var part = (Part)Activator.CreateInstance(typeof(Part), true)!;

        part.Name = "Headlight";
        part.Price = 100.50m;

        var result = repair.TryDestroyPart(part.Id, DestroyedPart.ReasonEnum.UselessMechanic, out var destroyedPart);

        Assert.False(result, "Part should not be destroyed.");
        Assert.Null(destroyedPart);
        Assert.DoesNotContain(part, repair.UsedParts);
    }

    [Fact]
    public void GetRepairInvoice_ShouldReturnCorrectInvoice()
    {
        // Arrange
        var customer = (Customer)Activator.CreateInstance(typeof(Customer), true)!;
        var mechanic = Mechanic.Create(
            name: "Test Mechanic",
            street: "Test Street",
            city: "Test City",
            postalCode: "12345",
            phoneNumber: "123456789",
            email: "test@test.com",
            hourlyRate: 50m,
            certification: "New Cert"
        );

        var repair = Repair.Create(
            customer: customer,
            mechanic: mechanic,
            repairDate: DateTime.Today.AddDays(1)
        );

        var part1 = (Part)Activator.CreateInstance(typeof(Part), true)!;
        var part2 = (Part)Activator.CreateInstance(typeof(Part), true)!;

        part1.Name = "Oil Filter";
        part1.Price = 20.99m;

        part2.Name = "Brake Pads";
        part2.Price = 45.50m;

        repair.AddPart(part1);
        repair.AddPart(part2);

        // Act
        var invoice = repair.GetRepairInvoice();

        // Assert
        Assert.NotNull(invoice);
        Assert.Equal(customer.AlternateId.ToString(), invoice.CustomerId);
        Assert.Equal(116.49m, invoice.TotalPrice);
        Assert.Equal(3, invoice.Positions.Count);
        Assert.Contains("Oil Filter", invoice.Positions.Keys);
        Assert.Contains("Brake Pads", invoice.Positions.Keys);
        Assert.Contains("Arbeitszeit", invoice.Positions.Keys);
        Assert.Equal(20.99m, invoice.Positions["Oil Filter"]);
        Assert.Equal(45.50m, invoice.Positions["Brake Pads"]);
        Assert.Equal(50.00m, invoice.Positions["Arbeitszeit"]);

    }
}