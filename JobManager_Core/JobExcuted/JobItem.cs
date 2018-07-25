using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobManager_Core.JobExcuted
{
    internal class JobItem : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            //TODO:调用基础设施层方法达到调用接口
            throw new NotImplementedException();
        }
    }
}
