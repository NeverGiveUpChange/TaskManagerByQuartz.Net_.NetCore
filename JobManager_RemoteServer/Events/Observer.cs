using System;
using System.Collections.Generic;
using System.Text;
using JobManager_Infrastructure.HttpClientUtil;
using JobManager_Infrastructure.LogUtil;
using JobManager_Infrastructure.MailUtil;
using JobManager_RemoteServer.Models;

namespace JobManager_RemoteServer.Events
{
    public class Observer : ObserverBase
    {

        public Observer(SubjectBase subjectBase) : base(subjectBase)
        {
        }

        public override void Post(JobExcutedCallBackModel jobExcutedCallBackModel)
        {
            var dateStart = DateTime.Now;
            try
            {

                var _httpClient = new HttpClientHelper();
                var result = _httpClient.PostAsync(jobExcutedCallBackModel.RequestBody, jobExcutedCallBackModel.RequestUrl).Result;
                var dateEnd = DateTime.Now;
                CustomerLogUtil.Info(CustomerLogFormatUtil.LogJobMsgFormat(jobExcutedCallBackModel.JobName, jobExcutedCallBackModel.JobState, jobExcutedCallBackModel.OperateType), new CustomerLogParameters(jobExcutedCallBackModel.GuId, "POST", Newtonsoft.Json.JsonConvert.SerializeObject(jobExcutedCallBackModel.RequestBody), result, dateStart.ToString("yyyy-MM-dd HH:mm:ss.ffff"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss.ffff"), $"{((dateEnd.Ticks - dateStart.Ticks) / 10000)}ms"));
            }
            catch (Exception ex)
            {
                var dateEnd = DateTime.Now;
                CustomerLogUtil.Error(ex, CustomerLogFormatUtil.LogJobMsgFormat(jobExcutedCallBackModel.JobName, jobExcutedCallBackModel.JobState, jobExcutedCallBackModel.OperateType), new CustomerLogParameters(jobExcutedCallBackModel.GuId, "POST", Newtonsoft.Json.JsonConvert.SerializeObject(jobExcutedCallBackModel.RequestBody), "", dateStart.ToString("yyyy-MM-dd HH:mm:ss.ffff"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss.ffff"), $"{((dateEnd.Ticks - dateStart.Ticks) / 10000)}ms"));

            }
        }

        public override void SendMail(SchedulerExecutedCallBackModel schedulerExecutedCallBackModel)
        {
            var dateStart = DateTime.Now;
            var mailClient = new MailClient();
            mailClient.SendMail(new MailMessageConfigurationInfo { Body = CustomerLogFormatUtil.LogSchedulerMsgFormat(SchedulerExecutedCallBackModel.LocalIP, SchedulerExecutedCallBackModel.QuartzServerName, schedulerExecutedCallBackModel.OperateType, schedulerExecutedCallBackModel.OperateState), Subject = schedulerExecutedCallBackModel.Subject, ToMailAddressList = schedulerExecutedCallBackModel.ToMailAddressList });
            var dateEnd = DateTime.Now;
            var message = CustomerLogFormatUtil.LogSchedulerMsgFormat(SchedulerExecutedCallBackModel.LocalIP, SchedulerExecutedCallBackModel.QuartzServerName, schedulerExecutedCallBackModel.OperateType, schedulerExecutedCallBackModel.OperateState);
            CustomerLogUtil.Info(message, new CustomerLogParameters(schedulerExecutedCallBackModel.GuId, "SendMail", message, "", dateStart.ToString("yyyy-MM-dd HH:mm:ss.ffff"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss.ffff"), $"{((dateEnd.Ticks - dateStart.Ticks) / 10000)}ms"));
        }
    }
}
