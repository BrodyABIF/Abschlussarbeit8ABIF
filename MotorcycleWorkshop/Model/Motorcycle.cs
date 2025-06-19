using System.ComponentModel.DataAnnotations;

namespace MotorcycleWorkshop.model;

public class Motorcycle
{
    public int Id { get; private set; }
    public Guid AlternateId { get; private set; } = Guid.NewGuid();

    private string _model;

    [MaxLength(20)]
    public string Model
    {
        get => _model;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Model cannot be empty.");
            _model = value;
        }
    }

    public int Year { get; set; }
    public decimal Mileage { get; private set; }
    
//--------------- Fremdschlüssel als Properties TODO
    public virtual Customer Owner { get; set; }
    public int OwnerId { get; set; }

//--------------Methode TODO
    public void UpdateMileage(decimal mileage)
    {
        if (mileage < 0)
            throw new ArgumentException("Mileage cannot be negative.");
        if (mileage < Mileage)
            throw new ArgumentException("Mileage cannot be less than current mileage.");
        Mileage = mileage;
    }

    public string GetMaintenanceStatus()
    {
        return Mileage > 10000 ? "Needs Maintenance" : "next Maintenance in " + (10000 - Mileage);
    }


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected Motorcycle()
    {
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}