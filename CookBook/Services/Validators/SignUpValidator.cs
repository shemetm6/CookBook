using CookBook.Contracts;
using FluentValidation;

namespace CookBook.Services.Validators;
public class SignUpValidator : AbstractValidator<SignUpDto>
{
    public SignUpValidator()
    {
        RuleFor(createDto => createDto.Login)
            .NotNull()
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(createDto => createDto.Password)
            .NotNull()
            .NotEmpty()
            .MaximumLength(256);
    }
}
