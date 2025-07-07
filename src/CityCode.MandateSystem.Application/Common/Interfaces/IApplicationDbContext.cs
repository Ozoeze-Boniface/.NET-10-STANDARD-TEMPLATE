namespace CityCode.MandateSystem.Application.Common.Interfaces;

using CityCode.MandateSystem.Domain;
using CityCode.MandateSystem.Domain.Entities;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<ApiRequestLog> ApiRequestLogs { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
