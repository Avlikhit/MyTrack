using FluentValidation;
using MyTrack.Contracts.Requests;

namespace MyTrack.Application.Validators;

/// <summary>
/// Validates login requests.
/// </summary>
public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginRequestValidator"/> class.
    /// </summary>
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email must be valid.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}