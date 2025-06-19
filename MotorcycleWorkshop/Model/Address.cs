using System.ComponentModel.DataAnnotations;

namespace MotorcycleWorkshop.model;

//------------- Value Object

public class Address
{
    [MaxLength(20)] public string Street { get; private set; } = null!;
    [MaxLength(20)] public string City { get; private set; } = null!;
    [MaxLength(20)] public string PostalCode { get; private set; } = null!;

    public Address(string street, string city, string postalCode)
    {
        if (string.IsNullOrWhiteSpace(street)) throw new ArgumentException("Street cannot be empty.");
        if (string.IsNullOrWhiteSpace(city)) throw new ArgumentException("City cannot be empty.");
        if (string.IsNullOrWhiteSpace(postalCode)) throw new ArgumentException("PostalCode cannot be empty.");

        Street = street;
        City = city;
        PostalCode = postalCode;
    }

    protected Address()
    {
    }
}