using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityCode.MandateSystem.Domain.Events.ActivityLog
{
    public class ActivityLogEvent(Activity activityLog) : BaseEvent
    {
        public Activity Activity { get; } = activityLog;
    }
}