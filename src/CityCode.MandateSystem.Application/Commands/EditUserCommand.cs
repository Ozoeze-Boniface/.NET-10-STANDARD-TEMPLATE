using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Domain.DomainDto;
using CityCode.MandateSystem.Domain.Enums;

namespace CityCode.MandateSystem.Application.Commands
{
    public class EditUserCommand : IRequest<Common.Models.View.Result<User>>
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public Role Role { get; set; }
        public string? InitiatedBy { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsSuperAdmin { get; set; } = false;
        public List<PermissionDto>? Permission { get; set; }
    }

    public class EditUserCommandHandler(IApplicationDbContext context) : IRequestHandler<EditUserCommand, Common.Models.View.Result<User>>
    {
        private readonly IApplicationDbContext context = context;

        public async Task<Common.Models.View.Result<User>> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            var user = context.AppUsers.FirstOrDefault(u => u.UserId == request.Id);
            if (user == null)
                throw new NotFoundException(request.Email, "User not found");

            user.Update(request.FirstName, request.LastName, request.Email, request.PhoneNumber, request.Username, request.IsActive, request.LastLogin, request.Role, request.IsSuperAdmin, request.Permission);

            await context.SaveChangesAsync(cancellationToken);
            return Common.Models.View.Result<User>.Success(DateTime.Now, user);
        }
    }
}