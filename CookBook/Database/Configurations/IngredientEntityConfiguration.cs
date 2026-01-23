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

        // Правильно ли я понял, что мы тут написали: у ингредиента есть много IngredientInRecipe (название Recipes)
        // Но у IngredientInRecipe есть только один ингредиент
        // И связаны они по Ingredient.Id 
        // И запрещаем удаление ингредиента, если он используется в рецептах
        builder.HasMany(i => i.Recipes)
            .WithOne(ir => ir.Ingredient)
            .HasForeignKey(ir => ir.IngredientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
