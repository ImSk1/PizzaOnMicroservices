using FluentValidation;
using Menu.API.Consts;
using Menu.API.Infrastructure.Entities;

namespace Menu.API.Validators
{
    public class PizzaValidator : AbstractValidator<Pizza>
    {
        public PizzaValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty();
            RuleFor(p => p.Name)
                .MaximumLength(ValidationConstants.PizzaNameMaxLength)
                    .WithMessage(string.Format(ValidationMessages.MaxLengthMessage,
                        ValidationConstants.PizzaNameMaxLength))
                .MinimumLength(ValidationConstants.PizzaNameMinLength)
                    .WithMessage(string.Format(ValidationMessages.MinLengthMessage, 
                        ValidationConstants.PizzaNameMinLength));
        }
    }
}
