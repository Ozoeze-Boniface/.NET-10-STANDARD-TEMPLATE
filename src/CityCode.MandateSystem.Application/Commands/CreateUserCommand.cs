using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Common.Models.View;
using CityCode.MandateSystem.Application.Dtos;
using CityCode.MandateSystem.Domain.DomainDto;
using CityCode.MandateSystem.Domain.Enums;
using CityCode.MandateSystem.Domain.Events.ActivityLog;

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
        public Role Role { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsSuperAdmin { get; set; } = false;
        public virtual List<PermissionDto>? Permission { get; set; }
    }

    public class CreateUserCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateUserCommand, Common.Models.View.Result<User>>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<Common.Models.View.Result<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _context.AppUsers.AnyAsync(x => x.Email == request.Email);
            if (userExists)
                return Common.Models.View.Result<User>.Failure("User already exists");

            var user = new User(request.FirstName, request.LastName, request.Email, request.PhoneNumber, request.Username, null!, true, DateTime.UtcNow, request.Role, request.IsSuperAdmin);
            user.WithPermissions(request.Permission);

            await _context.AppUsers.AddAsync(user);
            user.AddDomainEvent(new ActivityLogEvent(new Activity { Action = "Created User", DateCreated = DateTime.UtcNow, Entity = "Users" }));
            await _context.SaveChangesAsync(cancellationToken);
            return Common.Models.View.Result<User>.Success(DateTime.Now, user);
        }
    }
}