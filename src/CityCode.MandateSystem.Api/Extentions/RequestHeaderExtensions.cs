using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Dtos;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace CityCode.MandateSystem.Api.Extentions
{
    public static class RequestHeaderExtensions
    {

        public static (bool IsValid, IEnumerable<Claim>, string ErrorMessage) ValidateTokenExpiration(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                if (jwtToken == null)
                    return (false, [], "Invalid token format.");

                var utcNow = DateTimeOffset.UtcNow;
                var expirationTime = jwtToken.ValidTo;

                if (expirationTime < utcNow)
                    return (false, [], $"Token expired at {expirationTime}. Current time: {utcNow}");

                return (true, jwtToken.Claims, "Token is valid and not expired.");
            }
            catch (ArgumentException)
            {
                return (false, [], "Invalid token format.");
            }
            catch (SecurityTokenExpiredException)
            {
                return (false, [], "Token has expired.");
            }
            catch (Exception ex)
            {
                return (false, [], $"An error occurred while validating the token: {ex.Message}");
            }
        }
        public static JwtAuthenticationResponse GetCurrentUser(this HttpRequest request)
        {
            var authHeader = request.Headers["Authorization"].ToString();
            if (String.IsNullOrEmpty(authHeader))
            {
                return new JwtAuthenticationResponse(false);
            }

            string authToken = authHeader.Replace($"Bearer", "", StringComparison.InvariantCulture).Trim();
            var (isValid, claims, _) = ValidateTokenExpiration(authToken);
            if (!isValid)
            {
                return new JwtAuthenticationResponse(false);
            }

            var userDataClaims = claims.FirstOrDefault(x => x.Type == ClaimTypes.UserData);
            if (userDataClaims is null)
            {
                return new JwtAuthenticationResponse(false);
            }

            JwtAuthenticationResponse jwtAuthenticationResponse = JsonConvert.DeserializeObject<JwtAuthenticationResponse>(userDataClaims.Value) ?? new JwtAuthenticationResponse(false);
            jwtAuthenticationResponse.User.UserId = jwtAuthenticationResponse.User.UserId;
            return jwtAuthenticationResponse;
        }
    }
}