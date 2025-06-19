namespace MotorcycleWorkshop.API.DTOs;

public class PartDto
{
    public Guid AlternateId { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
}

