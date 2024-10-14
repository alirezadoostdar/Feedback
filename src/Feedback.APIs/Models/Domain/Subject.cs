﻿namespace Feedback.APIs.Models.Domain;

public class Subject
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public bool Locked { get; set; }
    public DateTime? ExpiredOn { get; set; }
    public int TenantId { get; set; }
    public ICollection<Review> Reviews { get; set; }
}
