using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityCode.MandateSystem.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("AppUsers");

            builder.HasKey(u => u.UserId);

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.PasswordHash)
                .IsRequired(false);

            builder.Property(u => u.IsActive)
                .IsRequired();

            builder.Property(u => u.LastLogin);

            // One-to-many relationship with Permission
            builder.HasMany(u => u.Permission)
                .WithOne()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade).IsRequired(false); // Or .Restrict / .SetNull based on your needs

        }
    }
}