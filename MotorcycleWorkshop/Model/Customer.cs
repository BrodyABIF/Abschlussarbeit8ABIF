namespace MotorcycleWorkshop.model;

public class Customer : Person
{
    public virtual ICollection<Repair> Repairs { get; private set; } = new List<Repair>();
    public virtual ICollection<Motorcycle> Motorcycles { get; private set; } = new List<Motorcycle>();

    public static Customer Create(string name, Address address, string phoneNumber, string email)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (address == null) throw new ArgumentNullException(nameof(address));
        if (string.IsNullOrWhiteSpace(phoneNumber)) throw new ArgumentNullException(nameof(phoneNumber));
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));

        var customer = CreateWithId<Customer>(
            id: 0, // ID wird von der Datenbank vergeben
            alternateId: Guid.NewGuid(),
            name: name,
            address: address,
            phoneNumber: phoneNumber,
            email: email
        );

        return customer;
    }

    // Überladene Version für einfachere Verwendung
    public static Customer Create(string name, string street, string city, string postalCode, string phoneNumber, string email)
    {
        return Create(
            name,
            new Address(street, city, postalCode),
            phoneNumber,
            email
        );
    }

    public IEnumerable<Repair> GetRepairHistory()
    {
        return Repairs.OrderByDescending(r => r.RepairDate).ToList();
    }





#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected Customer()
    {
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}