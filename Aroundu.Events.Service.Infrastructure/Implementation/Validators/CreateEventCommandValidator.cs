using Aroundu.Events.Service.Application.Commands;
using FluentValidation;

namespace Aroundu.Events.Service.Infrastructure.Implementation.Validators
{
    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        public CreateEventCommandValidator() {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
        }
    }
}
