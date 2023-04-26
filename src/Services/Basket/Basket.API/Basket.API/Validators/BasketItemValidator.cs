using Basket.API.Models;
using FluentValidation;

namespace Basket.API.Validators
{
    public class BasketItemValidator : AbstractValidator<BasketItem>
    {
        public BasketItemValidator()
        {
            RuleFor(p => p.Id).NotEmpty();
            RuleFor(p => p.PizzaId).NotEmpty();
            RuleFor(p => p.PizzaName).NotEmpty().MinimumLength(5).MaximumLength(50);
            RuleFor(p => p.Price).NotNull();
            RuleFor(p => p.Quantity).NotNull();
        }
    }
}
