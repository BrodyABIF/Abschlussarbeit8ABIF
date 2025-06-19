namespace MotorcycleWorkshop.API.DTOs;

public class CustomerDto
{
    public Guid AlternateId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required AddressDto Address { get; set; }
}
