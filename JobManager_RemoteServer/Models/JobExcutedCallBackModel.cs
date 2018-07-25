using System;
using System.Collections.Generic;
using System.Text;

namespace JobManager_RemoteServer.Models
{
   public  class JobExcutedCallBackModel
    {
        public string GuId { get; set; }
        public string JobName { get; set; }
        public int JobState { get; set; }
        public byte IsJobDeleted { get; set; }
        public string OperateType { get; set; }
        public string RequestUrl { get; set; }
        public object RequestBody { get; set; }
    }
}
