using JobManager_Infrastructure.IPUtil;
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
    public class MySchedulerListener : SubjectBase, ISchedulerListener
    {
       static Dictionary<string, string> guidDic = new Dictionary<string, string>();
        public MySchedulerListener() {

            new Observer(this);
        }
        public Task JobAdded(IJobDetail jobDetail, CancellationToken cancellationToken = default(CancellationToken))
        {
            Console.WriteLine("任务被部署");
            var jobState = -2;
            var operateType = "部署";
            _postAsync(jobDetail.Key.Name, jobState, operateType);
            return Task.FromResult<int>(1);
        }

        public Task JobDeleted(JobKey jobKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            Console.WriteLine("任务被删除,删除时间为：" + DateTime.Now);
            var jobState = 5;
            var operateType = "删除";
            byte isDelete = 1;
            _postAsync(jobKey.Name, jobState, operateType, isDelete);
            return Task.FromResult<int>(1);
        }

        public Task JobInterrupted(JobKey jobKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task JobPaused(JobKey jobKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            Console.WriteLine("任务被暂停");
            var jobState = 1;
            var operateType = "暂停";
            _postAsync(jobKey.Name, jobState, operateType);
            return Task.FromResult<int>(1);
        }

        public Task JobResumed(JobKey jobKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            Console.WriteLine("任务被恢复");

            var jobState = 6;
            var operateType = "运行";
            _postAsync(jobKey.Name, jobState, operateType);
            return Task.FromResult<int>(1);
        }

        public Task JobScheduled(ITrigger trigger, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task JobsPaused(string jobGroup, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task JobsResumed(string jobGroup, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task JobUnscheduled(TriggerKey triggerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SchedulerError(string msg, SchedulerException cause, CancellationToken cancellationToken = default(CancellationToken))
        {
            var operateType = "运行";
            var operateState = "异常";
            var subject = "调度器运行发生异常";
            _sendMail(subject, cause.GetBaseException(), operateState, operateType);
            return Task.FromResult<int>(1);
        }

        public Task SchedulerInStandbyMode(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SchedulerShutdown(CancellationToken cancellationToken = default(CancellationToken))
        {
            var operateType = "关闭";
            var operateState = "正常";
            var subject = "调度器被关闭";
            _sendMail(subject, null, operateState, operateType);
            Console.WriteLine("调度器被关闭");
            return Task.FromResult<int>(1);
        }

        public Task SchedulerShuttingdown(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SchedulerStarted(CancellationToken cancellationToken = default(CancellationToken))
        {
            var operateType = "启动";
            var operateState = "正常";
            var subject = "调度器被启动";
            _sendMail(subject, null, operateState, operateType);
            Console.WriteLine("调度器被启动");
            return Task.FromResult<int>(1);
        }

        public Task SchedulerStarting(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SchedulingDataCleared(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task TriggerFinalized(ITrigger trigger, CancellationToken cancellationToken = default(CancellationToken))
        {
            Console.WriteLine("任务完成使命，不在被执行");
            return Task.FromResult<int>(1);
        }

        public Task TriggerPaused(TriggerKey triggerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task TriggerResumed(TriggerKey triggerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task TriggersPaused(string triggerGroup, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task TriggersResumed(string triggerGroup, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
        private void _postAsync(string jobName, int jobState, string operateType, byte IsDelete = 0)
        {
            this.NotifyAsync(new JobExcutedCallBackModel { IsJobDeleted = IsDelete, JobName = jobName, JobState = jobState, OperateType = operateType, RequestUrl = "", RequestBody = new { JobName = jobName, JobState = jobState, Deleted = IsDelete } });

        }

        private void _sendMail(string subject, Exception ex, string operateState, string operateType)
        {
            var ip = IPHelper.IpAddress;
            if (!guidDic.ContainsKey(IPHelper.IpAddress)) {
                guidDic.Add(ip, Guid.NewGuid().ToString("N"));
            }
            this.NotifyAsync(new SchedulerExecutedCallBackModel { GuId = guidDic[ip], CCMailAddressList = new List<string>(), Exception = ex, OperateState = operateState, OperateType = operateType, Subject = subject, ToMailAddressList = new List<string> { "chenlong@91bihu.com" } });
        }
    }
}
