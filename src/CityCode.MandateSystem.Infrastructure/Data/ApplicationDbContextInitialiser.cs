namespace CityCode.MandateSystem.Infrastructure.Data;
//using System.Runtime.InteropServices;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using CityCode.MandateSystem.Domain.Constants;
using CityCode.MandateSystem.Domain.Entities;
using CityCode.MandateSystem.Infrastructure.Identity;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        // await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger = logger;
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public async Task InitialiseAsync()
    {
        try
        {
            await this._context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await this.TrySeedAsync();
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole(Roles.Administrator);

        if (this._roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await this._roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (this._userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await this._userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await this._userManager.AddToRolesAsync(administrator, [administratorRole.Name]);
            }
        }

        // Default data
        // Seed, if necessary
        if (!this._context.TodoLists.Any())
        {
            this._context.TodoLists.Add(new TodoList
            {
                Title = "Todo List",
                Items =
                {
                    new TodoItem { Title = "Make a todo list 📃" },
                    new TodoItem { Title = "Check off the first item ✅" },
                    new TodoItem { Title = "Realise you've already done two things on the list! 🤯"},
                    new TodoItem { Title = "Reward yourself with a nice, long nap 🏆" },
                }
            });

            await this._context.SaveChangesAsync();
        }
    }
}
