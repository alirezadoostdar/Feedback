using Feedback.APIs.Endpoints.Contracts;
using Feedback.APIs.Models.Domain;
using Feedback.APIs.Persistence;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Feedback.APIs.Endpoints;

public static class SubjectEndpoints
{
    public static void MapSubjectEndpoin(this IEndpointRouteBuilder endpoint)
    {
        var group = endpoint.MapGroup("/subject");

        group.MapPost("/", CreateSubject);
        group.MapGet("/{id}/check", CreateSubjectForReview);
        group.MapPost("/review/",);
    }

    public static async Task<Results<ValidationProblem,Created>> CreateSubject(FeedbackDbContext dbContext,
        IValidator<CreateSubjectRequest> validator,
        IUserPrincial userPrincial,
        IConfiguration configuration,
        CreateSubjectRequest request)
    {
        var validate = validator.Validate(request);
        if(!validate.IsValid)
        {
            return TypedResults.ValidationProblem(validate.ToDictionary());
        }

        var subject = Subject.Create(request.ExpirationOn, request.Title, userPrincial.TenantId);
        dbContext.Subjects.Add(subject);
        await dbContext.SaveChangesAsync();

        var longUrl = $"{configuration["BaseUrl"]}/subject/{subject.Id}/";
        //we can change long url to short url
        return TypedResults.Created(longUrl);
    }

    public static async Task<Results<NotFound,Ok,BadRequest<string>>> CreateSubjectForReview(FeedbackDbContext dbContext,
    IConfiguration configuration,
    [FromRoute] int id)
    {
        var subject = dbContext.Subjects.FirstOrDefault(x => x.Id == id);

        if(subject is null)
        {
            return TypedResults.NotFound();
        }

        if (subject.Locked)
        {
            return TypedResults.BadRequest("your subject locked");
        }

        if (subject.ExpiredOn is not null && subject.ExpiredOn < DateTime.Now)
        {
            return TypedResults.BadRequest("your subject locked");
        }

        return TypedResults.Ok();
    }
}
