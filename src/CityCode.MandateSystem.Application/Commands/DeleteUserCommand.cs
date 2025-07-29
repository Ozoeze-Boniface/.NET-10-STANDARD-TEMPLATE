using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Common.Models.View;
using CityCode.MandateSystem.Domain.Events.ActivityLog;
using Z.EntityFramework.Plus;

namespace CityCode.MandateSystem.Application.Commands
{
    public class DeleteUserCommand : IRequest<Common.Models.View.Result<string>>
    {
        public long UserId { get; set; }
    }

    public class DeleteUserCommandHanler(IApplicationDbContext context) : IRequestHandler<DeleteUserCommand, Common.Models.View.Result<string>>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<Common.Models.View.Result<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.AppUsers.Include(s => s.Permission).FirstOrDefaultAsync(s => s.UserId == request.UserId);

            if (user is not null)
            {
                _context.AppUsers.Remove(user);

                user.AddDomainEvent(new ActivityLogEvent(new Activity { Action = "Deleted User", DateCreated = DateTime.UtcNow, Entity = "Users" }));

                await _context.SaveChangesAsync(cancellationToken);
                return Common.Models.View.Result<string>.Success(DateTime.Now, request.UserId.ToString(), ResponseDescription: "User deleted successfully");
            }
            return Common.Models.View.Result<string>.Failure("User not found");

        }
    }
}