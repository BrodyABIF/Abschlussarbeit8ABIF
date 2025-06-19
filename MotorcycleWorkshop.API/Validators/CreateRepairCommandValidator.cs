using FluentValidation;
using MotorcycleWorkshop.API.Commands;

namespace MotorcycleWorkshop.API.Validators;

public class CreateRepairCommandValidator : AbstractValidator<CreateRepairCommand>
{
    public CreateRepairCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer ID is required");
        RuleFor(x => x.MechanicId).NotEmpty().WithMessage("Mechanic ID is required");
        RuleFor(x => x.RepairDate).NotEmpty().WithMessage("Repair date is required");
    }
}

