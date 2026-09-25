namespace CommandApi.Application.Validators;

public class CommandCreateDtoValidator : AbstractValidator<CommandCreateDto>
{
    public CommandCreateDtoValidator()
    {
        RuleFor(x => x.HowTo)
            .NotEmpty()
            .WithMessage("HowTo is required")
            .MaximumLength(250)
            .WithMessage("HowTo must not exceed 250 characters");

        RuleFor(x => x.CommandLine)
            .NotEmpty()
            .WithMessage("CommandLine is required");

        RuleFor(x => x.PlatformId)
            .GreaterThan(0)
            .WithMessage("PlatformId must be greater than 0");
    }
}

public class CommandUpdateValidator : AbstractValidator<CommandUpdateDto>
{
    public CommandUpdateValidator()
    {
        RuleFor(x => x.HowTo)
            .NotEmpty()
            .WithMessage("HowTo is required")
            .MaximumLength(250)
            .WithMessage("HowTo must not exceed 250 characters");

        RuleFor(x => x.CommandLine)
            .NotEmpty()
            .WithMessage("CommandLine is required");
    }
}