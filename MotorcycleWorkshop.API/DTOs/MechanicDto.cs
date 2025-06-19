namespace MotorcycleWorkshop.API.DTOs;

public class MechanicDto
{
    public Guid AlternateId { get; set; }
    public required string Name { get; set; }
    public required string Certification { get; set; }
    public decimal HourlyRate { get; set; }
    public required AddressDto Address { get; set; }
}
