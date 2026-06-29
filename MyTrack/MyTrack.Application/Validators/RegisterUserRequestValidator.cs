using FluentValidation;
using MyTrack.Contracts.Requests;

namespace MyTrack.Application.Validators;

/// <summary>
/// Validates register user requests.
/// </summary>
public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterUserRequestValidator"/> class.
    /// </summary>
    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MaximumLength(100)
            .WithMessage("First name cannot exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MaximumLength(100)
            .WithMessage("Last name cannot exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email must be valid.")
            .MaximumLength(200)
            .WithMessage("Email cannot exceed 200 characters.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters.");
    }
}