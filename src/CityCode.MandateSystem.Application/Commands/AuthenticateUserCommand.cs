using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Common.Models.View;
using CityCode.MandateSystem.Application.Dtos;

namespace CityCode.MandateSystem.Application.Commands
{
    public class AuthenticateUserCommand : IRequest<Common.Models.View.Result<AuthResponse>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class AuthenticateUserCommandHandler(IApplicationDbContext context) : IRequestHandler<AuthenticateUserCommand, Common.Models.View.Result<AuthResponse>>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<Common.Models.View.Result<AuthResponse>> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user is null)
                return Common.Models.View.Result<AuthResponse>.Failure("User details does not exist");

            if (user.PasswordHash is null)
            {
                // Hash user password and update user details, generate token and log user in
            }
            else
            {
                // hash inputed password and compare with the existsing hash; If match, log user in, otherwise, return unauthorized
            }

            return default!;
            
        }
    }
}