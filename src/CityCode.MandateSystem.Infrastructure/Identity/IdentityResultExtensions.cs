namespace CityCode.MandateSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

using CityCode.MandateSystem.Application.Common.Models;

public static class IdentityResultExtensions
{
    public static Result ToApplicationResult(this IdentityResult result) => result.Succeeded
            ? Result.Success()
            : Result.Failure(result.Errors.Select(e => e.Description));
}
