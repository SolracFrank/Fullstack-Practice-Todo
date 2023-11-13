using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class TodoConfiguration : IEntityTypeConfiguration<Todo>
    {
        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder.ToTable(nameof(Todo));

            builder.HasKey(b => b.IdTodo);

            builder.Property(b => b.IdTodo)
                .IsRequired()
                .HasColumnName("id_todo")
                .ValueGeneratedOnAdd();

            builder.Property(b => b.Description)
                .IsRequired(false)
                .HasMaxLength(255);

            builder.Property(b => b.DateLimit)
                .IsRequired(false)
                .HasColumnType("datetime2");

            builder.Property(b => b.Estado)
                .HasConversion<string>();

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_todo_user");

        }
    }
}
