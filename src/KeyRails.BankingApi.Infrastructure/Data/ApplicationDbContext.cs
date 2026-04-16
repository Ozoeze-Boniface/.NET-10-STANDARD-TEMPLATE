namespace KeyRails.BankingApi.Infrastructure.Data;
using System.Reflection;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using KeyRails.BankingApi.Application.Common.Interfaces;
using KeyRails.BankingApi.Domain;
using KeyRails.BankingApi.Domain.Entities;
using KeyRails.BankingApi.Infrastructure.Identity;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options), IApplicationDbContext
{
    public DbSet<TodoList> TodoLists => this.Set<TodoList>();

    public DbSet<TodoItem> TodoItems => this.Set<TodoItem>();
    public DbSet<ApiRequestLog> ApiRequestLogs => this.Set<ApiRequestLog>();
    public DbSet<User> AppUsers => this.Set<User>();
    public DbSet<Permission> Permissions => this.Set<Permission>();
    public DbSet<Activity> Activities => this.Set<Activity>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
