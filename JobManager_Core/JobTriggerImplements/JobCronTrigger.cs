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
    internal class JobCronTrigger : JobBaseTrigger
    {
        public JobCronTrigger()
        {
            Scheduler = SchedulerManager.Instance;
        }
        public override bool ModifyJobCron(customer_quartzjobinfo jobInfo)
        {
            var scheduleBuilder = CronScheduleBuilder.CronSchedule(jobInfo.Cron);
            var triggerKey = KeyManager.CreateTriggerKey(jobInfo.TriggerName, jobInfo.TriggerGroupName);
            var trigger = TriggerBuilder.Create().StartAt(DateTimeOffset.Now.AddYears(-1)).WithIdentity(triggerKey).WithSchedule(scheduleBuilder.WithMisfireHandlingInstructionDoNothing()).Build();
            Scheduler.RescheduleJob(triggerKey, trigger);
            return true;
        }

        public override bool RunJob(customer_quartzjobinfo jobInfo)
        {
            var jobKey = KeyManager.CreateJobKey(jobInfo.JobName, jobInfo.JobGroupName);
            if (!Scheduler.CheckExists(jobKey).Result)
            {
                IJobDetail jobDetail = JobBuilder.Create<JobItem>().WithIdentity(jobKey).UsingJobData(KeyManager.CreateJobDataMap("requestUrl", jobInfo.RequestUrl)).Build();
                CronScheduleBuilder cronScheduleBuilder = CronScheduleBuilder.CronSchedule(jobInfo.Cron);
                ITrigger trigger = TriggerBuilder.Create().StartAt(DateTimeOffset.Now.AddYears(-1))
                 .WithIdentity(jobInfo.TriggerName, jobInfo.TriggerGroupName)
                 .ForJob(jobKey)
                 .WithSchedule(cronScheduleBuilder.WithMisfireHandlingInstructionDoNothing())
                 .Build();
                Scheduler.ScheduleJob(jobDetail, trigger);
            }
            return true;
        }
    }
}
