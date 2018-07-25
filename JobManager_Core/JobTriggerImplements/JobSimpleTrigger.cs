using JobManager_Core.JobCommon;
using JobManager_Core.JobExcuted;
using JobManager_Core.JobTriggerAbstract;
using Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobManager_Core.JobTriggerImplements
{
    internal class JobSimpleTrigger : JobBaseTrigger
    {
        public JobSimpleTrigger()
        {
            Scheduler = SchedulerManager.Instance;
        }
        public override bool ModifyJobCron(customer_quartzjobinfo jobInfo)
        {
            var triggerKey = KeyManager.CreateTriggerKey(jobInfo.TriggerName, jobInfo.TriggerGroupName);
            ITrigger trigger = TriggerBuilder.Create().StartAt(DateTimeOffset.Now)
                    .WithIdentity(jobInfo.TriggerName, jobInfo.TriggerGroupName)
                   .WithSimpleSchedule(x => x.WithIntervalInSeconds(jobInfo.Cycle.HasValue ? jobInfo.Cycle.Value : 1).WithRepeatCount(jobInfo.RepeatCount.Value - 1))
                    .Build();
            Scheduler.RescheduleJob(triggerKey, trigger);
            return true;
        }

        public override bool RunJob(customer_quartzjobinfo jobInfo)
        {
            var jobKey = KeyManager.CreateJobKey(jobInfo.JobName, jobInfo.JobGroupName);
            if (!Scheduler.CheckExists(jobKey).Result)
            {
                var job = JobBuilder.Create<JobItem>()
                    .WithIdentity(jobKey)
                    .UsingJobData(KeyManager.CreateJobDataMap("requestUrl", jobInfo.RequestUrl))
                    .Build();
                var trigger = TriggerBuilder.Create().StartAt(DateTimeOffset.Now)
                    .WithIdentity(jobInfo.TriggerName, jobInfo.TriggerGroupName)
                    .ForJob(jobKey)
            .WithSimpleSchedule(x => x.WithIntervalInSeconds(jobInfo.Cycle.HasValue ? jobInfo.Cycle.Value : 1).WithRepeatCount(jobInfo.RepeatCount.Value - 1))
                    .Build();
                Scheduler.ScheduleJob(job, trigger);
            }
            return true;
        }
    }
}
