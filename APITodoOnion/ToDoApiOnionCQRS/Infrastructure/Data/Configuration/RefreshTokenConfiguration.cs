using Domain.Dtos.User.RefreshToken;
using Infrastructure.CustomEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            // Clave primaria
            builder.HasKey(x => x.Id);

            builder.Property(b => b.Id)
                .IsRequired()
                .HasColumnName("Id_RefreshToken");

            // Índice para Token
            builder.HasIndex(t => t.Token)
                .IsUnique();

            // Campos requeridos
            builder.Property(b => b.Token)
              .IsRequired();

            builder.Property(b => b.Expires)
              .IsRequired();

            builder.Property(b => b.RevokedByIp)
            .IsRequired(false);

            builder.Property(b => b.ReplacedByToken)
            .IsRequired(false);

            builder.Property(b => b.Created)
            .IsRequired();

            builder.Property(b => b.RevokedByIp)
            .IsRequired(false);

            builder.Property(b => b.ApplicationUserId)
              .IsRequired();

            // Relación con ApplicationUser (uno-a-muchos)
            builder.HasOne<ApplicationUser>()
              .WithMany()
              .HasForeignKey(u => u.ApplicationUserId)
              .OnDelete(DeleteBehavior.Cascade)
              .HasConstraintName("FK_RefreshToken_ApplicationUser");
        }

    }
}
