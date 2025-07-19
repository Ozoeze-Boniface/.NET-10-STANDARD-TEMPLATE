using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityCode.MandateSystem.Infrastructure.Data.Configurations
{
    public class MandateRequestConfiguration : IEntityTypeConfiguration<MandateRequest>
    {
        public void Configure(EntityTypeBuilder<MandateRequest> builder)
        {
            builder.ToTable("MandateRequests");

            builder.HasKey(m => m.MandateRequestId);

            builder.Property(m => m.MandateReference)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.InitiatedBy)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.SubscriberCode)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.BankCode)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(m => m.PayerName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.PayerAddress)
                .HasMaxLength(200);

            builder.Property(m => m.PayerEmail)
                .HasMaxLength(100);

            builder.Property(m => m.PayerPhoneNumber)
                .HasMaxLength(20);

            builder.Property(m => m.AccountName)
                .HasMaxLength(100);

            builder.Property(m => m.PayerAccountNumber)
                .HasMaxLength(20);

            builder.Property(m => m.PayerBvn)
                .HasMaxLength(11);

            builder.Property(m => m.BanksAccountNumber)
                .HasMaxLength(20);

            builder.Property(m => m.BanksAccountName)
                .HasMaxLength(100);

            builder.Property(m => m.BanksBvn)
                .HasMaxLength(11);

            builder.Property(m => m.DestinationInstitutionCode)
                .HasMaxLength(20);

            builder.Property(m => m.SourceInstitutionCode)
                .HasMaxLength(20);

            builder.Property(m => m.Narration)
                .HasMaxLength(500);

            builder.Property(m => m.MandateType)
                .IsRequired();

            builder.Property(m => m.MandateRequestStatus)
                .HasConversion<int>() // Enum to int
                .IsRequired();

            builder.Property(m => m.StartDate)
                .IsRequired();

            builder.Property(m => m.EndDate)
                .IsRequired();

            builder.Property(m => m.Location)
                .HasMaxLength(150);

            // Auditable properties from BaseAuditableEntity
            builder.Property(m => m.CreatedBy)
                .HasMaxLength(100);

            builder.Property(m => m.CreatedById)
                .IsRequired();

            builder.Property(m => m.LastModifiedBy)
                .HasMaxLength(100);

            builder.Property(m => m.DateCreated)
                .IsRequired();
        }
    }
}