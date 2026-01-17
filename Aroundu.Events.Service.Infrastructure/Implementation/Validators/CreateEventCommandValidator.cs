using Aroundu.Events.Service.Application.Commands;
using Aroundu.SharedKernel.Interfaces;
using FluentValidation;

namespace Aroundu.Events.Service.Infrastructure.Implementation.Validators
{
    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>, IDependency
    {
        public CreateEventCommandValidator() {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
        }
    }
}
