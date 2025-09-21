using CityCode.MandateSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityCode.MandateSystem.Infrastructure.Data.Configurations;

public class MandateScheduleConfiguration : IEntityTypeConfiguration<MandateSchedule>
{
    public void Configure(EntityTypeBuilder<MandateSchedule> builder)
    {

        // One-to-many: Schedule -> Transactions
        builder.HasMany(ms => ms.MandateTransactions)
            .WithOne(mt => mt.MandateSchedule)
            .HasForeignKey(mt => mt.MandateScheduleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("MandateSchedules");
    }
}