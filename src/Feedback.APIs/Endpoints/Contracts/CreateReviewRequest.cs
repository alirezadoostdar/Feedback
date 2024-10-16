using FluentValidation;

namespace Feedback.APIs.Endpoints.Contracts;

public record CreateReviewRequest(string ReviewName,string Comment,int SubjectId,int Rate);

public class CreateReviewRequestValidator : AbstractValidator<CreateReviewRequest>
{
    public CreateReviewRequestValidator()
    {
        RuleFor(x => x.ReviewName).NotEmpty().MaximumLength(50);

        RuleFor(x => x.Comment).NotEmpty().MaximumLength(400);
        RuleFor(x => x.SubjectId).NotNull();
        RuleFor(x => x.Rate).NotNull().GreaterThan(1).LessThan(6);
    }
}