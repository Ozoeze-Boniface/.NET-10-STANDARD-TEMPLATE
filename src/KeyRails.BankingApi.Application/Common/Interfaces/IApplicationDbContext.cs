namespace KeyRails.BankingApi.Application.Common.Interfaces;

using KeyRails.BankingApi.Domain;
using KeyRails.BankingApi.Domain.Entities;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<ApiRequestLog> ApiRequestLogs { get; }
    DbSet<User> AppUsers { get; }
    DbSet<Permission> Permissions { get; }
    DbSet<Activity> Activities { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
