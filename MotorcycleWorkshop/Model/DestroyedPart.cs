namespace MotorcycleWorkshop.model;

/// <summary>
/// for simplicity not in DB
/// </summary>
public class DestroyedPart : Part
{
    public enum ReasonEnum  //--------- ENUM TODO
    {
        FaultyPartDelivered = 1,
        UselessMechanic = 2,
        CustomerBroughtWrongPart = 3,
    }

    public ReasonEnum Reason { get; set; }

    public DestroyedPart(ReasonEnum reason)
    {
        Reason = reason;
    }
}