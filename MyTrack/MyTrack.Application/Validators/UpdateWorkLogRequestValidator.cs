using FluentValidation;
using MyTrack.Contracts.Requests;

namespace MyTrack.Application.Validators;

/// <summary>
/// Validates update work log requests.
/// </summary>
public class UpdateWorkLogRequestValidator : AbstractValidator<UpdateWorkLogRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateWorkLogRequestValidator"/> class.
    /// </summary>
    public UpdateWorkLogRequestValidator()
    {
        RuleFor(x => x.WorkDate)
            .NotEmpty()
            .WithMessage("Work date is required.");

        RuleFor(x => x.ProjectId)
            .GreaterThan(0)
            .WithMessage("ProjectId is required.");

        RuleFor(x => x.TaskType)
            .NotEmpty()
            .WithMessage("Task type is required.")
            .MaximumLength(100)
            .WithMessage("Task type cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(1000)
            .WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(x => x.HoursWorked)
            .GreaterThan(0)
            .WithMessage("Hours worked must be greater than zero.")
            .LessThanOrEqualTo(24)
            .WithMessage("Hours worked cannot exceed 24 hours.");

        RuleFor(x => x.TicketNumber)
            .MaximumLength(100)
            .WithMessage("Ticket number cannot exceed 100 characters.");

        RuleFor(x => x.Blockers)
            .MaximumLength(1000)
            .WithMessage("Blockers cannot exceed 1000 characters.");

        RuleFor(x => x.Learnings)
            .MaximumLength(1000)
            .WithMessage("Learnings cannot exceed 1000 characters.");

        RuleFor(x => x.NextSteps)
            .MaximumLength(1000)
            .WithMessage("Next steps cannot exceed 1000 characters.");
    }
}