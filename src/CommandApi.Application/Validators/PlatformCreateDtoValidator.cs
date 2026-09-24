namespace CommandApi.Application.Validators;

public class PlatformCreateDtoValidator : AbstractValidator<PlatformCreateDto>
{
    public PlatformCreateDtoValidator()
    {
        RuleFor(p => p.PlatformName)
           .NotEmpty()
           .WithMessage("Platform name is required")
           .MinimumLength(2)
           .WithMessage("Platform minimum length is 2 characters long");
    }
}

public class PlatformUpdateDtoValidator : AbstractValidator<PlatformUpdateDto>
{
    public PlatformUpdateDtoValidator()
    {
        RuleFor(p => p.PlatformName)
           .NotEmpty()
           .WithMessage("Platform name is required")
           .MinimumLength(2)
           .WithMessage("Platform minimum length is 2 characters long");
    }
}