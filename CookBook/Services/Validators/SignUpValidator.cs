using CookBook.Contracts;
using FluentValidation;

namespace CookBook.Services.Validators;
public class SignUpValidator : AbstractValidator<SignUpDto>
{
    private const int _loginMaxLength = 128;
    private const int _passwordMaxLength = 256;

    public SignUpValidator()
    {
        RuleFor(createDto => createDto.Login)
            .NotNull()
            .WithMessage("Login is required!")
            .NotEmpty()
            .WithMessage("Login cannot be empty!")
            .MaximumLength(_loginMaxLength)
            .WithMessage($"Login cannot be longer than {_loginMaxLength} characters!");

        RuleFor(createDto => createDto.Password)
            .NotNull()
            .WithMessage("Password is required!")
            .NotEmpty()
            .WithMessage("Password cannot be empty!")
            .MaximumLength(_passwordMaxLength)
            .WithMessage($"Password cannot be longer than {_passwordMaxLength} characters!");
    }
}
