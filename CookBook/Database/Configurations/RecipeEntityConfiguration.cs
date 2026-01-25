using CookBook.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CookBook.Database.Configurations;

public class RecipeEntityConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(r => r.CookTime)
            .IsRequired();

        builder.Property(r => r.Description)
            .IsRequired()
            .HasMaxLength(32000);

        /* Отказываемся от хранения рейтинга в БД до появления соответствующей модели
        builder.Property(r => r.Ratings)
            .HasColumnType("integer[]");
        */

        builder.HasMany(r => r.Ingredients)
            .WithOne(ir => ir.Recipe)
            .HasForeignKey(ir => ir.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
