namespace MotorcycleWorkshop.API.DTOs;

public class RepairDto
{
    public Guid AlternateId { get; set; }
    public DateTime RepairDate { get; set; }
    public required string MechanicName { get; set; }
    public required string CustomerName { get; set; }
    public decimal TotalPartsPrice { get; set; }
}

