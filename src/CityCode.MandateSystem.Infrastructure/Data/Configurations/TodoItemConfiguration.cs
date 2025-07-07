namespace CityCode.MandateSystem.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using CityCode.MandateSystem.Domain.Entities;

public class TodoItemConfiguration : BaseAuditableEntityConfiguration<TodoItem>, IEntityTypeConfiguration<TodoItem>
{
    public override void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.Property(t => t.Title)
                    .HasMaxLength(200)
                    .IsRequired();
        base.Configure(builder);
        builder.ToTable("TO_DO_ITEMS");
    }
}
