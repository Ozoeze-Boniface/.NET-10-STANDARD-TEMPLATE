using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityCode.MandateSystem.Infrastructure.Data.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .HasMaxLength(250);

            builder.Property(p => p.Resource)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Action)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.IsActive)
                .IsRequired();

            builder.Property(p => p.UserId)
                .IsRequired();

            // Define relationship with User
            builder
                .HasOne<User>() // No navigation property in Permission
                .WithMany(u => u.Permission)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Or .SetNull / .Restrict as needed

        }
    }
}