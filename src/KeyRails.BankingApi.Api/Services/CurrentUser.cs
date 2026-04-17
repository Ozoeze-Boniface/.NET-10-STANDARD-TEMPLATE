namespace KeyRails.BankingApi.Api.Services;
using System.Security.Claims;
using KeyRails.BankingApi.Api.Extentions;
using KeyRails.BankingApi.Application.Common.Interfaces;
using KeyRails.BankingApi.Application.Dtos;
using Newtonsoft.Json;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string? Id => this._httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? UserId
    {
        get
        {
            var authHeader = _httpContextAccessor.HttpContext?.Request?.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader))
                return null;

            var token = authHeader.Replace("Bearer", "", StringComparison.InvariantCulture).Trim();
            var (isValid, claims, _) = RequestHeaderExtensions.ValidateTokenExpiration(token);
            if (!isValid)
                return null;

            var userDataClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.UserData);
            if (userDataClaim is null)
                return null;

            var jwtResponse = JsonConvert.DeserializeObject<JwtAuthenticationResponse>(userDataClaim.Value);
            return jwtResponse?.User?.UserId.ToString();
        }
    }
}
