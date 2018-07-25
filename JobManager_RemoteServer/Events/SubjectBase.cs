using JobManager_RemoteServer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobManager_RemoteServer.Events
{
   public class SubjectBase
    {
        public delegate void JobExecutedCallBack(JobExcutedCallBackModel jobExcutedCallBackModel);
        public delegate void SchedulerExecutedCallBack(SchedulerExecutedCallBackModel schedulerExecutedCallBackModel);

        private JobExecutedCallBack _jobExecutedCallBack;
        private SchedulerExecutedCallBack _schedulerExecutedCallBack;
        public event JobExecutedCallBack JobExecutedCallBackEvent
        {
            add
            {
                _jobExecutedCallBack += value;
            }
            remove
            {
                if (_jobExecutedCallBack != null)
                {
                    _jobExecutedCallBack -= value;
                }
            }
        }
        public event SchedulerExecutedCallBack SchedulerExecutedCallBackEvent
        {

            add
            {
                _schedulerExecutedCallBack += value;
            }
            remove
            {
                if (_schedulerExecutedCallBack != null)
                {
                    _schedulerExecutedCallBack -= value;
                }

            }
        }
        protected void NotifyAsync(JobExcutedCallBackModel jobExcutedCallBackModel)
        {
            _jobExecutedCallBack.BeginInvoke(jobExcutedCallBackModel, null, null);
            //this._jobExecutedCallBack(jobExcutedCallBackModel);

        }
        protected void NotifyAsync(SchedulerExecutedCallBackModel schedulerExecutedCallBackModel)
        {
            _schedulerExecutedCallBack.BeginInvoke(schedulerExecutedCallBackModel, null, null);
            //this._schedulerExecutedCallBack(schedulerExecutedCallBackModel);
        }

    }
}
