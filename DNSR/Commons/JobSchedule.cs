using System;

namespace DNSR.Commons
{
    public class JobSchedule
    {
        public JobSchedule(Type jobType, string cronExpression, bool isEnable)
        {
            JobType = jobType;
            CronExpression = cronExpression;
            IsEnable = isEnable;
        }

        public bool IsEnable { get; }
        public Type JobType { get; }
        public string CronExpression { get; }
    }
}
