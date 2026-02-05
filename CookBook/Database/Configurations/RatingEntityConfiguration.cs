using CookBook.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CookBook.Database.Configurations;

public class RatingEntityConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.HasKey(r => new { r.UserId, r.RecipeId });

        builder.Property(r => r.Value)
            .IsRequired();

        builder.HasOne(r => r.User)
            .WithMany(u => u.Ratings)
            .HasForeignKey(r => r.UserId);

        builder.HasOne(r => r.Recipe)
            .WithMany(recipe => recipe.Ratings)
            .HasForeignKey(r => r.RecipeId);
    }
}
