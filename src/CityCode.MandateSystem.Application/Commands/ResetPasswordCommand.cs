using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Common.Models.View;

namespace CityCode.MandateSystem.Application.Commands
{
    public class ResetPasswordCommand : IRequest<Common.Models.View.Result<bool>>
    {
        public string Email { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public class ResetPasswordCommandHandler(IApplicationDbContext context) : IRequestHandler<ResetPasswordCommand, Common.Models.View.Result<bool>>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<Common.Models.View.Result<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.AppUsers.FirstOrDefaultAsync(s => s.Email == request.Email);
            if (user is null)
            {
                return Common.Models.View.Result<bool>.Failure("User not found");
            }
            if (user.Otp != request.Otp)
            {
                return Common.Models.View.Result<bool>.Failure("Invalid Otp");
            }
            var hashedPassword = HashPassword(request.NewPassword);
            user.SetPassword(hashedPassword);
            await _context.SaveChangesAsync(cancellationToken);
            return Common.Models.View.Result<bool>.Success(DateTime.Now, true);
        }

        private static string HashPassword(string password)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = SHA256.HashData(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}