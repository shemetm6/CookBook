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

        // чатгпт говорит надо явно указать, что работать с листом надо как с массивом,
        // т.к. postgresql с Npgsql не знает как хранить лист, а как хранить массив знает.
        // Я ХУЙ ЗНАЕТ НЕОБХОДИМО ЛИ ЭТО
        builder.Property(r => r.Ratings)
            .HasColumnType("integer[]");

        builder.HasMany(r => r.Ingredients)
            .WithOne(ir => ir.Recipe)
            .HasForeignKey(ir => ir.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
