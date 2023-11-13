using Domain.Entities;
using Infrastructure.CustomEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(p => p.IdUser);

            builder.Property(p => p.IdUser)
                .IsRequired()
                .HasColumnName("id_user")
                .ValueGeneratedOnAdd();

            builder.Property(p=>p.IdAccount)
                .HasColumnName("id_account")
                .IsRequired();

            builder.HasOne<ApplicationUser>()
               .WithOne()
               .HasForeignKey<User>(u => u.IdAccount)
               .OnDelete(DeleteBehavior.Cascade)
               .HasConstraintName("FK_user_account");
        }
    }
}
