namespace KeyRails.BankingApi.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

using KeyRails.BankingApi.Application.Common.Models;

public static class IdentityResultExtensions
{
    public static Result ToApplicationResult(this IdentityResult result) => result.Succeeded
            ? Result.Success()
            : Result.Failure(result.Errors.Select(e => e.Description));
}
