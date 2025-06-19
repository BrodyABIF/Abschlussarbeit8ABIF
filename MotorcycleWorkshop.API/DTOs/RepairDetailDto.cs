namespace MotorcycleWorkshop.API.DTOs;

// TODO DTO mit mehreren Ebenen
public class RepairDetailDto
{
    public Guid AlternateId { get; set; }
    public DateTime RepairDate { get; set; }
    public required MechanicDto Mechanic { get; set; }
    public required CustomerDto Customer { get; set; }
    public required ICollection<PartDto> UsedParts { get; set; }
}

