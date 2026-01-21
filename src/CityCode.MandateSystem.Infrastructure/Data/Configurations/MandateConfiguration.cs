using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace CityCode.MandateSystem.Infrastructure.Data.Configurations
{
    public class MandateConfiguration : IEntityTypeConfiguration<Mandate>
    {
        public void Configure(EntityTypeBuilder<Mandate> builder)
        {
            builder.ToTable("Mandates");

            builder.HasKey(m => m.MandateId);

            builder.Property(m => m.MandateReference)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.NibbsMandateCode)
                .IsRequired(false)
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

            builder.Property(m => m.WorkflowStatus)
                .HasConversion<int>() // enum to int
                .IsRequired(false);

            builder.Property(m => m.MandateStatus)
                .HasConversion<int>() // enum to int
                .IsRequired();

            builder.Property(m => m.ProgressStatus)
                .HasConversion<int>() // enum to int
                .IsRequired();

            builder.Property(m => m.StartDate)
                .IsRequired();

            builder.Property(m => m.EndDate)
                .IsRequired();

            builder.Property(m => m.Location)
                .HasMaxLength(150);

            // Auditable Properties
            builder.Property(m => m.CreatedBy)
                .HasMaxLength(100);

            builder.Property(m => m.LastModifiedBy)
                .HasMaxLength(100);

            builder.Property(m => m.CreatedById)
                .IsRequired();

            builder.Property(m => m.PaymentFrequency)
                .HasConversion<int>() // enum to int
                .IsRequired();
            
            builder.HasMany(m => m.Documents)
                .WithOne()
                .HasForeignKey(d => d.MandateReference)
                .HasPrincipalKey(m => m.MandateReference).IsRequired(false);
        }
    }
}