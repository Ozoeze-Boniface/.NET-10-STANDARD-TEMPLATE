using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyRails.BankingApi.Domain.Events.ActivityLog
{
    public class ActivityLogEvent(Activity activityLog) : BaseEvent
    {
        public Activity Activity { get; } = activityLog;
    }
}