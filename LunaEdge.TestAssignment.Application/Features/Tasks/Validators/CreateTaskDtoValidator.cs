using FluentValidation;
using LunaEdge.TestAssignment.Application.Features.Tasks.Dtos;

namespace LunaEdge.TestAssignment.Application.Features.Tasks.Validators;

public class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
{
    public CreateTaskDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.DueDate).GreaterThan(DateTime.Now);
        RuleFor(x => x.Status).IsInEnum();
        RuleFor(x => x.Priority).IsInEnum();
    }
}