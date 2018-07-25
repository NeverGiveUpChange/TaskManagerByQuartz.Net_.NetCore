using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace JobManager_RemoteServer
{
    public class Configuration
    {
        //public static string Description { get { return ConfigurationManager.AppSettings["Description"]; } }
        public static string Description { get { return "Bh_Crm定时任务服务节点"; } }

        //public static string DisplayName { get { return ConfigurationManager.AppSettings["DisplayName"]; } }
        public static string DisplayName { get { return "Bh_Crm_QuartzServer"; } }


        //public static string ServiceName { get { return ConfigurationManager.AppSettings["ServiceName"]; } }
        public static string ServiceName { get { return "Bh_Crm_QuartzServer"; } }

    }
}
