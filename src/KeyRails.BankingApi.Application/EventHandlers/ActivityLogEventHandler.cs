using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyRails.BankingApi.Domain.Events.ActivityLog;

namespace KeyRails.BankingApi.Application.EventHandlers
{
    public class ActivityLogEventHandler(ILogger<ActivityLogEventHandler> logger, IApplicationDbContext context, IUser user) : INotificationHandler<ActivityLogEvent>
    {
        private readonly ILogger<ActivityLogEventHandler> _logger = logger;
        private readonly IApplicationDbContext _context = context;
        private readonly IUser _user = user;

        public async Task Handle(ActivityLogEvent notification, CancellationToken cancellationToken)
        {
            notification.Activity.Actor = long.Parse(_user.UserId!);
            var user = await _context.AppUsers.AsNoTracking().FirstOrDefaultAsync(s => s.UserId == long.Parse(_user.UserId!));
            notification.Activity.ActorName = user!.FullName;
            await _context.Activities.AddAsync(notification.Activity);
        }
    }
}