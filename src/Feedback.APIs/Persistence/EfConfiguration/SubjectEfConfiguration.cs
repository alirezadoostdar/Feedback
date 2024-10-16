﻿using Feedback.APIs.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feedback.APIs.Persistence.EfConfiguration;

public class SubjectEfConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.ToTable(FeedbackDbContextSchema.SubjectTableName);
        builder.HasKey(x => x.Id);
        builder.Property(x=> x.Locked)
            .IsRequired()
            .HasDefaultValue(false) ;
        builder.Property(x => x.CreatedOn).IsRequired().HasDefaultValueSql("GETDATE()");
        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x=>x.ExpiredOn).IsRequired(false);
        builder.Property(x => x.Title).IsRequired(false).HasMaxLength(100);

        builder.OwnsMany(x => x.Reviews, reviewBuilder =>
        {
            reviewBuilder.ToTable(FeedbackDbContextSchema.ReviewTableName);

            reviewBuilder.HasKey(x=>x.Id);

            reviewBuilder.Property(x => x.Comment).HasMaxLength(400).IsRequired().IsUnicode(true);
            reviewBuilder.Property(x => x.ReviewerName).HasMaxLength(400).IsRequired().IsUnicode(true);
            reviewBuilder.Property(x => x.SubjectId).IsRequired();
            reviewBuilder.Property(x => x.Date).IsRequired().HasDefaultValueSql("GETDATE()");

        });
    }
}
