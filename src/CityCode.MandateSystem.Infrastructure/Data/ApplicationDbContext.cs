namespace CityCode.MandateSystem.Infrastructure.Data;
using System.Reflection;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using CityCode.MandateSystem.Application.Common.Interfaces;
using CityCode.MandateSystem.Domain;
using CityCode.MandateSystem.Domain.Entities;
using CityCode.MandateSystem.Infrastructure.Identity;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options), IApplicationDbContext
{
    public DbSet<TodoList> TodoLists => this.Set<TodoList>();

    public DbSet<TodoItem> TodoItems => this.Set<TodoItem>();
    public DbSet<ApiRequestLog> ApiRequestLogs => this.Set<ApiRequestLog>();
    public DbSet<User> AppUsers => this.Set<User>();
    public DbSet<Permission> Permissions => this.Set<Permission>();
    public DbSet<Mandate> Mandates => this.Set<Mandate>();
    public DbSet<MandateRequest> MandateRequests => this.Set<MandateRequest>();
    public DbSet<Activity> Activities => this.Set<Activity>();
    public DbSet<MandateSchedule> MandateSchedules => this.Set<MandateSchedule>();
    public DbSet<Bank> Banks => this.Set<Bank>();
    public DbSet<MandateTransaction> MandateTransactions => this.Set<MandateTransaction>();
    public DbSet<RetryTransaction> RetryTransactions => this.Set<RetryTransaction>();
    public DbSet<Document> Documents => this.Set<Document>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
