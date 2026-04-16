using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyRails.BankingApi.Application.Common.Models.View;
using KeyRails.BankingApi.Domain.Enums;
using KeyRails.BankingApi.Domain.Events.ActivityLog;

namespace KeyRails.BankingApi.Application.Commands;

public class ActivateDeactivateUserCommand : IRequest<Common.Models.View.Result<string>>
{
    public long UserId { get; set; }
    public UserStatus Status { get; set; }
}

public class DeactivateUserCommandHandler : IRequestHandler<ActivateDeactivateUserCommand, Common.Models.View.Result<string>>
{
    private readonly IApplicationDbContext _context;

    public DeactivateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Common.Models.View.Result<string>> Handle(ActivateDeactivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.AppUsers.FirstOrDefaultAsync(s => s.UserId == request.UserId);

        if (user is null)
        {
            return Common.Models.View.Result<string>.Failure("User not found");
        }

        bool activate = request.Status == UserStatus.ACTIVATE;
        user.ChangeUserStatus(activate);

        string responseDescription = activate
            ? "User successfully activated"
            : "User successfully deactivated";
            
        user.AddDomainEvent(new ActivityLogEvent(new Activity { Action = responseDescription, DateCreated = DateTime.UtcNow, Entity = "Users" }));

        await _context.SaveChangesAsync(cancellationToken);


        return Common.Models.View.Result<string>.Success(DateTime.Now, user.UserId.ToString(), ResponseDescription: responseDescription);
    }
}