using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using KeyRails.BankingApi.Application.Common.Exceptions;
using KeyRails.BankingApi.Application.Common.Models.View;
using KeyRails.BankingApi.Application.Dtos;
using KeyRails.BankingApi.Application.Settings;
using KeyRails.BankingApi.Domain.DomainDto;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace KeyRails.BankingApi.Application.Commands
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
            var tokenUser = new TokenUserData
            {
                UserId = user.UserId,
                IsSuperAdmin = user.IsSuperAdmin
            };

            var tokenPermissions = user.Permission?
                .Where(permission => !string.IsNullOrWhiteSpace(permission.Name))
                .Select(permission => new TokenPermissionData { Name = permission.Name })
                .ToList() ?? [];

            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.FullName),
                new(ClaimTypes.UserData, JsonConvert.SerializeObject(new UserTokenData(true, tokenUser,
                    tokenPermissions, user.Role)))
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
