namespace KeyRails.BankingApi.Infrastructure.Data.Configurations;
//using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using KeyRails.BankingApi.Domain.Entities;

public class TodoListConfiguration : BaseAuditableEntityConfiguration<TodoList>
{
    public override void Configure(EntityTypeBuilder<TodoList> builder)
    {
        builder.Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .OwnsOne(b => b.Colour);
    }
}
