using FluentValidation;
using MyTrack.Contracts.Requests;

namespace MyTrack.Application.Validators;

/// <summary>
/// Validates update project requests.
/// </summary>
public class UpdateProjectRequestValidator : AbstractValidator<UpdateProjectRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProjectRequestValidator"/> class.
    /// </summary>
    public UpdateProjectRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Project name is required.")
            .MaximumLength(100)
            .WithMessage("Project name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Project description cannot exceed 500 characters.");

        RuleFor(x => x.ColorCode)
            .Matches("^#(?:[0-9a-fA-F]{3}){1,2}$")
            .When(x => !string.IsNullOrWhiteSpace(x.ColorCode))
            .WithMessage("Color code must be a valid hex color value.");

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Display order cannot be negative.");
    }
}