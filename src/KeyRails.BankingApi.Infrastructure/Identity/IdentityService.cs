namespace KeyRails.BankingApi.Infrastructure.Identity;

using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using KeyRails.BankingApi.Application.Common.Interfaces;
using KeyRails.BankingApi.Application.Common.Models;
using KeyRails.BankingApi.Domain.Entities;

public class IdentityService(
    IApplicationDbContext context,
    IAuthorizationService authorizationService) : IIdentityService
{
    private readonly IApplicationDbContext _context = context;
    private readonly IAuthorizationService _authorizationService = authorizationService;

    public async Task<string?> GetUserNameAsync(string userId)
    {
        var user = await this.FindUserAsync(userId);

        return user?.Username ?? user?.Email ?? user?.FullName;
    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            return (Result.Failure(["Username is required."]), string.Empty);
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            return (Result.Failure(["Password is required."]), string.Empty);
        }

        var normalizedUserName = userName.Trim();
        var normalizedLookup = normalizedUserName.ToLowerInvariant();

        var existingUser = await this._context.AppUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(u =>
                u.Username.ToLower() == normalizedLookup ||
                u.Email.ToLower() == normalizedLookup);

        if (existingUser is not null)
        {
            return (Result.Failure(["User already exists."]), string.Empty);
        }

        var email = normalizedUserName.Contains('@')
            ? normalizedUserName
            : $"{normalizedUserName}@keyrails.local";

        var user = new User(
            firstName: normalizedUserName,
            lastName: "User",
            email: email,
            phoneNumber: string.Empty,
            username: normalizedUserName,
            passwordHash: HashPassword(password),
            isActive: true,
            lastLogin: DateTime.UtcNow,
            role: "System User",
            isSuperAdmin: false,
            createdBy: nameof(IdentityService));

        await this._context.AppUsers.AddAsync(user);
        await this._context.SaveChangesAsync(CancellationToken.None);

        return (Result.Success(), user.UserId.ToString());
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        if (string.IsNullOrWhiteSpace(role))
        {
            return false;
        }

        var user = await this.FindUserAsync(userId);

        if (user is null)
        {
            return false;
        }

        if (user.IsSuperAdmin)
        {
            return true;
        }

        return GetRoles(user.Role)
            .Any(userRole => string.Equals(userRole, role, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        if (string.IsNullOrWhiteSpace(policyName))
        {
            return false;
        }

        var user = await this.FindUserAsync(userId);

        if (user is null)
        {
            return false;
        }

        if (user.IsSuperAdmin)
        {
            return true;
        }

        var principal = BuildClaimsPrincipal(user);
        var result = await this._authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = await this.FindUserAsync(userId);

        if (user is null)
        {
            return Result.Success();
        }

        this._context.AppUsers.Remove(user);
        await this._context.SaveChangesAsync(CancellationToken.None);

        return Result.Success();
    }

    private async Task<User?> FindUserAsync(string userId)
    {
        if (!long.TryParse(userId, out var parsedUserId))
        {
            return null;
        }

        return await this._context.AppUsers
            .Include(u => u.Permission)
            .FirstOrDefaultAsync(u => u.UserId == parsedUserId);
    }

    private static ClaimsPrincipal BuildClaimsPrincipal(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email)
        };

        claims.AddRange(GetRoles(user.Role).Select(role => new Claim(ClaimTypes.Role, role)));

        if (user.IsSuperAdmin)
        {
            claims.Add(new Claim("is_super_admin", bool.TrueString));
        }

        return new ClaimsPrincipal(new ClaimsIdentity(claims, nameof(IdentityService)));
    }

    private static IEnumerable<string> GetRoles(string? roles) =>
        string.IsNullOrWhiteSpace(roles)
            ? []
            : roles
                .Split([',', ';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    private static string HashPassword(string password)
    {
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }
}
