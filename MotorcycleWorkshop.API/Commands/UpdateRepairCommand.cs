namespace MotorcycleWorkshop.API.Commands;

public class UpdateRepairCommand
{
    public DateTime? RepairDate { get; set; }
    public Guid? MechanicId { get; set; }
    public List<Guid>? PartIdsToAdd { get; set; }
    public List<Guid>? PartIdsToRemove { get; set; }
}