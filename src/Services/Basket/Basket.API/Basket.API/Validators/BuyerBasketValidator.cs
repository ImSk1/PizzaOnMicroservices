using Basket.API.Models;
using FluentValidation;

namespace Basket.API.Validators
{
    public class BuyerBasketValidator : AbstractValidator<BuyerBasket>
    {
        public BuyerBasketValidator()
        {
            RuleFor(a => a.BuyerId).NotEmpty();
            RuleForEach(a => a.Items).SetValidator(new BasketItemValidator());
        }
    }
}
