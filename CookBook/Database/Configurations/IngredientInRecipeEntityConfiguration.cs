using CookBook.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CookBook.Database.Configurations;

public class IngredientInRecipeEntityConfiguration : IEntityTypeConfiguration<IngredientInRecipe>
{
    public void Configure(EntityTypeBuilder<IngredientInRecipe> builder)
    {
        builder.HasKey(ir => new { ir.IngredientId, ir.RecipeId });

        builder.HasOne(ir => ir.Ingredient)
            .WithMany(i => i.Recipes)
            .HasForeignKey(ir => ir.IngredientId);

        builder.HasOne(ir => ir.Recipe)
            .WithMany(r => r.Ingredients)
            .HasForeignKey(ir => ir.RecipeId);
    }
}
