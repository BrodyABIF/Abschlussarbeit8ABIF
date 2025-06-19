using System.ComponentModel.DataAnnotations;

namespace MotorcycleWorkshop.model;

// Person ist BaseClass und vererbert an Customer und Mechanic. TODO

public class Person
{
    private Address _address;

    private string _phoneNumber;

//------------------ Bsp Backing-Field
    private string _name; //------------- Backing-Field TODO

    public string Name //--------------- verwendet Backing-Field und fügt Validierung hinzu TODO
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name cannot be empty.");
            _name = value;
        }
    }
//------------------

    public int Id { get; private set; }
    public Guid AlternateId { get; private set; } = Guid.NewGuid();
    [MaxLength(20)] public string Email { get; set; }


    [MaxLength(20)]
    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Phone number cannot be empty.");
            _phoneNumber = value;
        }
    }

    [MaxLength(20)]
    public virtual Address Address
    {
        get => _address;
        set => _address = value ?? throw new ArgumentNullException(nameof(Address), "Address cannot be null.");
    }


    public static T CreateWithId<T>(int id, Guid alternateId, string name, Address address, 
        string phoneNumber, string email, decimal? hourlyRate = null, string? certification = null) 
        where T : Person
    {
        var instance = (T)Activator.CreateInstance(typeof(T), nonPublic: true)!;

        instance.Id = id;
        instance.AlternateId = alternateId;
        instance.Name = name;
        instance.Address = address;
        instance.PhoneNumber = phoneNumber;
        instance.Email = email;

        if (instance is Mechanic mechanic)
        {
            if (!hourlyRate.HasValue)
                throw new ArgumentNullException(nameof(hourlyRate));
            if (string.IsNullOrEmpty(certification))
                throw new ArgumentNullException(nameof(certification));
            
            mechanic.Initialize(hourlyRate.Value, certification);
        }

        return instance;
    }


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected Person()
    {
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}