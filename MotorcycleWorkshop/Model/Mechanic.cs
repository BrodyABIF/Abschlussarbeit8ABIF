using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using MotorcycleWorkshop.Infrastructure;

namespace MotorcycleWorkshop.model;

public class Mechanic : Person
{
    private decimal _hourlyRate;
    private string _certification;

    [MaxLength(20)] 
    public string Certification 
    { 
        get => _certification;
        private set => _certification = value ?? throw new ArgumentNullException(nameof(value));
    }

    public decimal HourlyRate 
    { 
        get => _hourlyRate;
        private set
        {
            if (value <= 0) throw new ArgumentException("Hourly rate must be greater than zero");
            _hourlyRate = value;
        }
    }

    public virtual ICollection<Repair> Repairs { get; private set; } = new List<Repair>();

    internal void Initialize(decimal hourlyRate, string certification)
    {
        HourlyRate = hourlyRate;
        Certification = certification;
    }


    public static Mechanic Create(string name, string street, string city, string postalCode, 
        string phoneNumber, string email, string certification, decimal hourlyRate)
    {
        if (hourlyRate <= 0) throw new ArgumentException("Hourly rate must be greater than zero", nameof(hourlyRate));
        if (string.IsNullOrEmpty(certification)) throw new ArgumentNullException(nameof(certification));

        var mechanic = CreateWithId<Mechanic>(
            id: 0, // ID wird von der Datenbank vergeben
            alternateId: Guid.NewGuid(),
            name: name,
            address: new Address(street, city, postalCode),
            phoneNumber: phoneNumber,
            email: email,
            hourlyRate: hourlyRate,
            certification: certification
        );

        return mechanic;
    }

    public int CalculateTotalHours(DateTime startDate, DateTime endDate)
    {
        return Repairs.Where(r => r.RepairDate >= startDate && r.RepairDate <= endDate).Count();
    }

    public IEnumerable<Mechanic> GetAvailableMechanics(DateTime startDate, DateTime endDate, WorkshopDBContext context)
    {
        var availableMechanics = context.Mechanics
            .Include(m => m.Repairs)
            .Where(m => !m.Repairs.Any(r => r.RepairDate >= startDate && r.RepairDate <= endDate))
            .ToList();

        return availableMechanics;
    }



#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected Mechanic()
    {
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}