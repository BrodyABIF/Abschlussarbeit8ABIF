using FluentValidation;
using MotorcycleWorkshop.API.Commands;

namespace MotorcycleWorkshop.API.Validators;

public class UpdateRepairCommandValidator : AbstractValidator<UpdateRepairCommand>
{
    public UpdateRepairCommandValidator()
    {
        When(x => x.RepairDate.HasValue, () =>
        {
            RuleFor(x => x.RepairDate)
                .Must(date => date!.Value.Date >= DateTime.Today)
                .WithMessage("Repair date must not be in the past");
        });
        
        When(x => x.PartIdsToAdd != null, () =>
        {
            RuleFor(x => x.PartIdsToAdd)
                .Must(ids => ids!.Distinct().Count() == ids!.Count)
                .WithMessage("Duplicate part IDs are not allowed");
        });
    }
}
