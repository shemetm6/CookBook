using CookBook.Contracts;
using FluentValidation;

namespace CookBook.Services.Validators;

public class RateRecipeValidator : AbstractValidator<RateRecipeDto>
{
    private const int _ratingMinValue = 1;
    private const int _ratingMaxValue = 5;
    public RateRecipeValidator()
    {
        RuleFor(rateDto => rateDto.Value)
            .InclusiveBetween(_ratingMinValue, _ratingMaxValue)
            .WithMessage($"Rating value must be between {_ratingMinValue} and {_ratingMaxValue}!");
    }
}
