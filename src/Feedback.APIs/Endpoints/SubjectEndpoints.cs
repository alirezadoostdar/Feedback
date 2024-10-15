using Feedback.APIs.Endpoints.Contracts;
using Feedback.APIs.Persistence;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Feedback.APIs.Endpoints;

public static class SubjectEndpoints
{
    public static void MapSubjectEndpoin(this IEndpointRouteBuilder endpoint)
    {
        var group = endpoint.MapGroup("/subject");

        group.MapPost("/", (
            FeedbackDbContext dbContext,
            CreateSubjectRequest request) =>
        {

        });
    }

    public static async Task<Results<ValidationProblem,Created>> CreateSubject(FeedbackDbContext dbContext,
        IValidator<CreateSubjectRequest> validator,
        CreateSubjectRequest request)
    {
        var validate = validator.Validate(request);
        if(!validate.IsValid)
        {
            return TypedResults.ValidationProblem(validate.ToDictionary());
        }
        return type
    }
}
