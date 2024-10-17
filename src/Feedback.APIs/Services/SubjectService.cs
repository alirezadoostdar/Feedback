using Azure.Core;
using Feedback.APIs.Models.Domain;
using Feedback.APIs.Persistence;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Feedback.APIs.Services;

public class SubjectService(FeedbackDbContext feedbackDbContext)
{
    private readonly FeedbackDbContext _feedbackDbContext = feedbackDbContext;

    public async Task<int> Create(string title,int tenantId,DateTime? expirationOn)
    {
        if (_feedbackDbContext.Subjects.Any(x => x.TenantId == tenantId && x.Title == title))
            throw new Exception("subjec is duplicated");

        var subject = Subject.Create(expirationOn, title, tenantId);
        _feedbackDbContext.Subjects.Add(subject);
        await _feedbackDbContext.SaveChangesAsync();
        return subject.Id;
    }

    internal async Task AddReview(int subjectId,string reviewName,string comment ,int rate)
    {
        var subject = _feedbackDbContext.Subjects
                                        .Include(f => f.Reviews)
                                        .FirstOrDefault(x=>x.Id == subjectId);
        CheckActionOnReview(subject);


        subject.AddReview(rate, comment, reviewName);
        await _feedbackDbContext.SaveChangesAsync();
    }

    internal async Task CheckSubjectForReview(int id)
    {
        var subject = await _feedbackDbContext.Subjects.FirstOrDefaultAsync(x => x.Id == id);

       CheckActionOnReview(subject);
    }

    private static void CheckActionOnReview(Subject? subject)
    {
        if (subject is null)
        {
            throw new Exception("not found entity");
        }

        if (subject.Locked)
        {
            throw new Exception("locked");

        }

        if (subject.ExpiredOn is not null && subject.ExpiredOn < DateTime.Now)
        {
            throw new Exception("Expired");

        }
    }
}

