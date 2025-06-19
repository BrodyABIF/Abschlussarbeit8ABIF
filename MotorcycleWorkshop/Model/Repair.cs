namespace MotorcycleWorkshop.model;

/// <summary>
/// Aggregate: Root Entity
/// </summary>
public class Repair
{

    public int Id { get; private set; }
    public Guid AlternateId { get; private set; }

    private DateTime _repairDate;
    public virtual Customer Customer { get; private set; }
    public int CustomerId { get; private set; }
    public virtual Mechanic Mechanic { get; private set; }
    public int MechanicId { get; private set; }
    
    private readonly List<Part> _usedParts = new();
    public virtual IReadOnlyCollection<Part> UsedParts => _usedParts.AsReadOnly();

    public DateTime RepairDate
    {
        get => _repairDate;
        set
        {
            if (value == default)
                throw new ArgumentException("RepairDate cannot be default.");
            _repairDate = value;
        }
    }


    public static Repair Create(Customer customer, Mechanic mechanic, DateTime repairDate)
    {
        if (customer == null) throw new ArgumentNullException(nameof(customer));
        if (mechanic == null) throw new ArgumentNullException(nameof(mechanic));
        
        return new Repair
        {
            AlternateId = Guid.NewGuid(),
            Customer = customer,
            Mechanic = mechanic,
            RepairDate = repairDate
        };
    }

    
    
    public void AddPart(Part part) // -------- Hinzufügen von Instanzen innerhalb einer Vererbungskette
    {
        if (part == null) throw new ArgumentNullException(nameof(part), "Part cannot be null.");
        _usedParts.Add(part);
    }

    public void RemovePart(int partId) // ----------------- entfernen von Instanzen innerhalb einer Vererbungskette
    {
        var part = _usedParts.FirstOrDefault(p => p.Id == partId);
        if (part != null) _usedParts.Remove(part);
    }

    //---------------- Aggregate Vererbung
    public bool TryDestroyPart(int partId, DestroyedPart.ReasonEnum reason, out DestroyedPart? destroyedPart) //-------------Umwandlung eines Objekts innerhabl Vererbungskette TODO
    {
        destroyedPart = null;
        var part = _usedParts.FirstOrDefault(p => p.Id == partId);

        if (part is not null)
        {
            destroyedPart = new DestroyedPart(reason);
            _usedParts.Remove(part);
            return true;
        }

        return false;
    }

//------------Methode
    public Invoice GetRepairInvoice()
    {
        var invoice = new Invoice
        {
            // Gesamtpreis aus Teilen plus Arbeitszeit
            TotalPrice  = _usedParts.Sum(p => p.Price) + Mechanic.HourlyRate,
            // Stückliste
            Positions   = _usedParts.ToDictionary(p => p.Name, p => p.Price),
            CustomerId  = Customer.AlternateId.ToString(),

            // ganz wichtig: Verknüpfung zur Repair
            Repair      = this,
            RepairId    = this.Id
        };

        // noch den Stundenlohn als eigene Position
        invoice.Positions["Arbeitszeit"] = Mechanic.HourlyRate;

        return invoice;
    }

    public void AssignMechanic(Mechanic newMechanic)
    {
        Mechanic = newMechanic ?? throw new ArgumentNullException(nameof(newMechanic));
    }

    public decimal CalculateTotalCost() => _usedParts.Sum(p => p.Price);

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected Repair()
    {
        AlternateId = Guid.NewGuid();
    }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}