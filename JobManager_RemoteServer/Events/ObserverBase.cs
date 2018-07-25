using JobManager_RemoteServer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobManager_RemoteServer.Events
{
    public abstract class ObserverBase
    {
        public ObserverBase(SubjectBase subjectBase) {

            subjectBase.JobExecutedCallBackEvent += new SubjectBase.JobExecutedCallBack(Post);
            subjectBase.SchedulerExecutedCallBackEvent += new SubjectBase.SchedulerExecutedCallBack(SendMail);
        }
        public abstract void Post(JobExcutedCallBackModel jobExcutedCallBackModel);
        public abstract void SendMail(SchedulerExecutedCallBackModel schedulerExecutedCallBackModel);
    }
}
