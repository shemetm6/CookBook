using CookBook.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CookBook.Database.Configurations;

public class IngredientEntityConfiguration : IEntityTypeConfiguration<Ingredient>
{
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(128);

        builder.HasIndex(i => i.Name)
            .IsUnique();

        builder.HasMany(i => i.Recipes)
            .WithOne(ir => ir.Ingredient)
            .HasForeignKey(ir => ir.IngredientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
