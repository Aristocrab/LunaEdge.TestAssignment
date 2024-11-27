using FluentValidation;
using LunaEdge.TestAssignment.Application.Features.Users.Dtos;

namespace LunaEdge.TestAssignment.Application.Features.Users.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(50)
            .Must(UseSpecialCharacters);
    }
    
    private static bool UseSpecialCharacters(string password)
    {
        const string specialCharacters = "!@#$%^&*()-+_=";
        return password.Any(c => specialCharacters.Contains(c));
    }
}