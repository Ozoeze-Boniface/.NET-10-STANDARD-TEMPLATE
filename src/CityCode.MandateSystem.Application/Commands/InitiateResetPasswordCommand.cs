using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Common;
using CityCode.MandateSystem.Application.Common.Models.View;
using CityCode.MandateSystem.Application.Services.UtilityServices.Interfaces;

namespace CityCode.MandateSystem.Application.Commands
{
    public class InitiateResetPasswordCommand : IRequest<Common.Models.View.Result<bool>>
    {
        public string EmailAddress { get; set; } = string.Empty;
    }

    public class InitiateResetPasswordCommandHandler(IApplicationDbContext context, IEmailService emailService) : IRequestHandler<InitiateResetPasswordCommand, Common.Models.View.Result<bool>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IEmailService _emailService = emailService;

        public async Task<Common.Models.View.Result<bool>> Handle(InitiateResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.AppUsers.FirstOrDefaultAsync(s => s.Email == request.EmailAddress);

            if (user is null)
            {
                return Common.Models.View.Result<bool>.Failure("User not found");
            }
            var otp = Helpers.GenerateUniqueOtp();
            user.SetOtp(otp);
            var sent = await _emailService.SendEmail(request.EmailAddress, new Dtos.MailContent { Header = "Reset Password", Body = $"Please use the otp {otp} to complete your password reset", Subject = "Password Reset" });
            if (sent)
            {
                await _context.SaveChangesAsync(cancellationToken);
                return Common.Models.View.Result<bool>.Success(DateTime.Now, sent);
            }
            return Common.Models.View.Result<bool>.Failure("Password reset notification failed");
        }
    }
}