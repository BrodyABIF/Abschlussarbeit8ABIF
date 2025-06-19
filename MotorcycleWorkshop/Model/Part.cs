using System.ComponentModel.DataAnnotations;

namespace MotorcycleWorkshop.model;

public class Part
{
    public int Id { get; private set; }
    public Guid AlternateId { get; private set; } = Guid.NewGuid();
    public string _name;

    [MaxLength(20)]
    public string Name
    {
        get => _name;
        set => _name = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Part name is required");
    }

    public decimal Price { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected Part()
    {
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}