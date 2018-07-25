using JobManager_Infrastructure.IPUtil;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobManager_RemoteServer.Models
{
    public class SchedulerExecutedCallBackModel
    {
        public string GuId { get; set; }
        public static string LocalIP { get { return IPHelper.IpAddress; } private set { } }
        private static string _localIP = IPHelper.IpAddress;

        public static string QuartzServerName { get { return "Bh_Crm_QuartzServer"; } private set { } }
        public string OperateType { get; set; }
        public string OperateState { get; set; }
        //public string Body { get; set; }
        public List<string> ToMailAddressList { get; set; }
        public string Subject { get; set; }
        public List<string> CCMailAddressList { get; set; }
        public Exception Exception { get; set; }
    }
}
