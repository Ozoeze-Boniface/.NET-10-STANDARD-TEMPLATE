using KeyRails.BankingApi.Application.Common.Exceptions;
using KeyRails.BankingApi.Domain.DomainDto;
using KeyRails.BankingApi.Domain.Events.ActivityLog;

namespace KeyRails.BankingApi.Application.Commands
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

    public class CreateUserCommandHandler(IApplicationDbContext context)
        : IRequestHandler<CreateUserCommand, Common.Models.View.Result<User>>
    {
        private readonly IApplicationDbContext _context = context;

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

            // send email notification to user

            await _context.SaveChangesAsync(cancellationToken);
            return Common.Models.View.Result<User>.Success(DateTime.Now, user);
        }
    }
}