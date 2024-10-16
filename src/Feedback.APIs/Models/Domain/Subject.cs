namespace Feedback.APIs.Models.Domain;

public class Subject
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public bool Locked { get; set; }
    public DateTime? ExpiredOn { get; set; }
    public required string Title  { get; set; }
    public int TenantId { get; set; }
    public ICollection<Review> Reviews { get; set; }

    public static Subject Create(DateTime? expired , string title , int tenantId)=> new Subject
    {
        Title = title , TenantId = tenantId 
    };

    internal void AddReview(int rate, string comment, string reviewName)
    {
        Reviews ??= new List<Review>();
        Reviews.Add(new Review
        {
            Comment = comment,
            ReviewerName = reviewName,
            Rate = rate
        });
    }
}
