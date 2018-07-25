using JobManager_Infrastructure.LogUtil;
using JobManager_RemoteServer.Events;
using JobManager_RemoteServer.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobManager_RemoteServer.Listeners
{
    public class MyJobListener : SubjectBase, IJobListener
    {
        static Dictionary<string, string> guidDic = new Dictionary<string, string>();
        public MyJobListener()
        {
            new Observer(this);

        }


        public string Name => "customerJobListener";

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default(CancellationToken))
        {
            var jobName = context.JobDetail.Key.Name;
            if (!guidDic.ContainsKey(jobName))
            {
                guidDic[jobName] = Guid.NewGuid().ToString("N");
            }
            var jobState = 6;
            var operateType = "运行";
            string exceptionMessage = jobException == null ? null : jobException.Message;
            this.NotifyAsync(new JobExcutedCallBackModel { GuId = guidDic[jobName], IsJobDeleted = 0, JobName = jobName, JobState = jobState, OperateType = operateType, RequestUrl = "", RequestBody = new { JobName = jobName, JobState = jobState, Exception = exceptionMessage, PreTime = context.Trigger.GetPreviousFireTimeUtc().HasValue ? context.Trigger.GetPreviousFireTimeUtc().Value.LocalDateTime as DateTime? : null, NextTime = context.Trigger.GetNextFireTimeUtc().HasValue ? context.Trigger.GetNextFireTimeUtc().Value.LocalDateTime as DateTime? : null } });

            return Task.FromResult<int>(1);
        }
    }
}
