namespace MotorcycleWorkshop.model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Invoice
{
    public int Id { get; private set; }

    public decimal TotalPrice { get; set; }

    [NotMapped]
    public Dictionary<string, decimal> Positions { get; set; } = new();

    public required string CustomerId { get; set; }

    // ← neu: echte FK-Property
    public int? RepairId { get; set; }

    [ForeignKey(nameof(RepairId))]
    public virtual Repair? Repair { get; set; }
}