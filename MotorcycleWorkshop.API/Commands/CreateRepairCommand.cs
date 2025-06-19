namespace MotorcycleWorkshop.API.Commands;

public class CreateRepairCommand
{
    public Guid CustomerId { get; set; }
    public Guid MechanicId { get; set; }
    public DateTime RepairDate { get; set; }
    public List<Guid> PartIds { get; set; } = new();
}
