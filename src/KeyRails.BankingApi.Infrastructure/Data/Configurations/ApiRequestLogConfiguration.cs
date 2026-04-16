namespace KeyRails.BankingApi.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KeyRails.BankingApi.Domain.Entities;

public class ApiRequestLogConfiguration : IEntityTypeConfiguration<ApiRequestLog>
{
    public void Configure(EntityTypeBuilder<ApiRequestLog> builder)
    {
        builder.Property(t => t.ApiRequestLogId)
            .IsRequired()
            .UseIdentityColumn()
            .HasColumnName("API_REQUEST_LOG_ID");
        builder.Property(t => t.ApiUserId)
               .IsRequired()
               .HasColumnType("varchar(100)")
               .HasMaxLength(100)
               .HasColumnName("API_USER_ID");
        builder.Property(t => t.ClientUniqueRequestId)
            .IsRequired()
            .HasColumnType("varchar(50)")
            .HasColumnName("CLIENT_UNIQUE_REQUEST_ID")
            .HasMaxLength(50);
        builder.Property(t => t.ApiCallStatus)
            .IsRequired()
            .HasColumnType("varchar(50)")
            .HasColumnName("API_CALL_STATUS")
            .HasMaxLength(50);
        builder.Property(t => t.EndPointUrl)
            .IsRequired()
            .HasColumnType("varchar(200)")
            .HasColumnName("API_END_POINT")
            .HasMaxLength(200);
        builder.Property(t => t.ApiRequestJson)
            .IsRequired()
            .HasColumnType("jsonb")
            .HasColumnName("API_REQUEST_JSON");
        builder.Property(t => t.ApiResponseJson)
            .IsRequired()
            .HasColumnType("jsonb")
            .HasColumnName("API_RESPONSE_JSON");
        builder.Property(t => t.ApiRequestDate)
            .IsRequired()
            .HasColumnType("date")
            .HasColumnName("API_REQUEST_DATE");
        builder.Property(t => t.ApiRequestTime)
            .IsRequired()
            .HasColumnName("API_REQUEST_TIME");
        builder.Property(t => t.ApiResponseDate)
            .HasColumnType("date")
            .HasColumnName("API_RESPONSE_DATE")
            .IsRequired(false);
        builder.Property(t => t.ApiResponseTime)
            .HasColumnName("API_RESPONSE_TIME")
            .IsRequired(false);
        builder.Property(t => t.RowVersion)
             .HasColumnName("ROW_VERSION")
             .IsRequired();

        builder.ToTable("API_REQUEST_LOGS");
    }
}
