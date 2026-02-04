using CookBook.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CookBook.Database.Configurations;

public sealed class JwtTokenEntityConfiguration : IEntityTypeConfiguration<JwtToken>
{
    public void Configure(EntityTypeBuilder<JwtToken> builder)
    {
        builder.HasKey(j => j.Id);

        builder.Property(j => j.UserId)
            .ValueGeneratedNever()
            .IsRequired();

        builder.HasOne(j => j.User)
            .WithOne()
            .HasForeignKey<JwtToken>()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
