namespace KeyRails.BankingApi.Infrastructure.Data;

using System.Reflection;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using KeyRails.BankingApi.Application.Common.Interfaces;
using KeyRails.BankingApi.Domain.Entities;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<ApiRequestLog> ApiRequestLogs => Set<ApiRequestLog>();
    public DbSet<User> AppUsers => Set<User>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<Activity> Activities => Set<Activity>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
