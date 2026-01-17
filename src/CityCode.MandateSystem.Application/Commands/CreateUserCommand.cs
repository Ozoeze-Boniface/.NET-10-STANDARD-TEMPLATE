using CityCode.MandateSystem.Application.Common.Exceptions;
using CityCode.MandateSystem.Application.Dtos;
using CityCode.MandateSystem.Application.Services.UtilityServices.Interfaces;
using CityCode.MandateSystem.Application.Settings;
using CityCode.MandateSystem.Domain.DomainDto;
using CityCode.MandateSystem.Domain.Events.ActivityLog;
using Microsoft.Extensions.Options;

namespace CityCode.MandateSystem.Application.Commands
{
    public class CreateUserCommand : IRequest<Common.Models.View.Result<User>>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string? Role { get; set; }
        public string? InitiatedBy { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsSuperAdmin { get; set; } = false;
        public virtual List<PermissionDto>? Permission { get; set; }
    }

    public class CreateUserCommandHandler(IApplicationDbContext context, IEmailService emailService, IOptions<SystemSettings> options)
        : IRequestHandler<CreateUserCommand, Common.Models.View.Result<User>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IEmailService _emailService = emailService;

        public async Task<Common.Models.View.Result<User>> Handle(CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            var userExists = await _context.AppUsers.AnyAsync(x => x.Email == request.Email);
            if (userExists)
                throw new BadRequestException("User already exists");

            var user = new User(request.FirstName, request.LastName, request.Email, request.PhoneNumber,
                request.Username, null!, true, DateTime.UtcNow, request.Role ?? "System User", request.IsSuperAdmin,
                request.InitiatedBy);
            user.WithPermissions(request.Permission);

            await _context.AppUsers.AddAsync(user);
            user.AddDomainEvent(new ActivityLogEvent(new Activity
                { Action = "Created User", DateCreated = DateTime.UtcNow, Entity = "Users" }));

            SystemSettings systemSettings = options.Value;
            var url = systemSettings.FrontEndUrl;
            var sent = await _emailService.SendEmail(
                user.Email,
                new MailContent
                {
                    Header = "User Profile Created",
                    Subject = "CityCode Direct Debit Mandate Application",
                    Body = $@"
            Dear {user.FullName},<br/><br/>
            You’ve been successfully profiled on <b>CityCode’s Direct Debit Mandate Application</b>.<br/><br/>
            The email you should use to access the application is: <b>{user.Email}</b>.<br/>
            Also note that the password you use on first login will be stored as your password for subsequent access to the application.<br/><br/>
            <a href='{url}' target='_blank'>Click here to access your dashboard</a>.<br/><br/>
            For support, please reach out to the IT Department.<br/><br/>
            Thank you.
        "
                });

            if (!sent)
            {
                return Common.Models.View.Result<User>.Failure("Failed to send notification");
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Common.Models.View.Result<User>.Success(DateTime.Now, user);
        }
    }
}