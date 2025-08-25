using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Common.Exceptions;
using CityCode.MandateSystem.Application.Common.Models.View;
using CityCode.MandateSystem.Application.Dtos;
using CityCode.MandateSystem.Application.Settings;
using CityCode.MandateSystem.Domain.DomainDto;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CityCode.MandateSystem.Application.Commands
{
    public class AuthenticateUserCommand : IRequest<Common.Models.View.Result<AuthResponse>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class AuthenticateUserCommandHandler(IApplicationDbContext context, IOptions<JwtSettings> jwtSettings) : IRequestHandler<AuthenticateUserCommand, Common.Models.View.Result<AuthResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        public async Task<Common.Models.View.Result<AuthResponse>> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.AppUsers.Include(s => s.Permission).FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user is null)
                throw new BadRequestException("Invalid credentials provided");

            if(!user.IsActive)
                throw new BadRequestException("User has been deactivated. Please contact the administrator.");

            if (user.PasswordHash is null)
            {
                var hashedPassword = HashPassword(request.Password);
                user.SetPassword(hashedPassword);
                var token = GenerateToken(user);
                var response = new AuthResponse(token, string.Empty, user);
                await _context.SaveChangesAsync(cancellationToken);
                return Common.Models.View.Result<AuthResponse>.Success(DateTime.Now, response);
            }
            {
                var hashedPassword = HashPassword(request.Password);
                if (hashedPassword == user.PasswordHash)
                {
                    var token = GenerateToken(user);
                    var response = new AuthResponse(token, string.Empty, user);
                    return Common.Models.View.Result<AuthResponse>.Success(DateTime.Now, response);
                }
                throw new BadRequestException("Invalid credentials provided");
                // hash inputed password and compare with the existsing hash; If match, log user in, otherwise, return unauthorized

            }
        }

        private static string HashPassword(string password)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = SHA256.HashData(bytes);
            return Convert.ToBase64String(hash);
        }

        private string GenerateToken(User user)
        {
            List<Permission> permission = null!;
            if (user.Permission is not null)
            {
                permission = user.Permission.Any() ? user.Permission : null!;
            }

            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.FullName),
                new(ClaimTypes.UserData, JsonConvert.SerializeObject(new UserTokenData(true, user,
                    permission, nameof(user.Role))))
            };

            string jwtSecret = _jwtSettings.JwtSecret;
            double expirationMinutes = _jwtSettings.TokenExpirationMinutes;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}