using FluentValidation;
using Menu.API.Infrastructure.Entities;

namespace Menu.API.Validators
{
    public class PizzaValidator : AbstractValidator<Pizza>
    {
        public PizzaValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
        }
    }
}
