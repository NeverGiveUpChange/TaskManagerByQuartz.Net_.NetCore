using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;

namespace JobManager_Core.JobCommon
{
    internal sealed class SchedulerManager
    {
        static IScheduler _scheduler;
        static readonly ConcurrentDictionary<string, IScheduler> ConnectionCache = new ConcurrentDictionary<string, IScheduler>();
        static readonly string channelType = ConfigurationManager.AppSettings["channelType"];
        static readonly string localIp = ConfigurationManager.AppSettings["localIp"];
        static readonly string port = ConfigurationManager.AppSettings["port"];
        static readonly string bindName = ConfigurationManager.AppSettings["bindName"];
        private SchedulerManager() { }
        public static IScheduler Instance { get { return InstanceProvider.instance; } }
        private class InstanceProvider
        {
            internal static readonly IScheduler instance = null;
            static InstanceProvider()
            {

                instance = _getScheduler(localIp);
            }
            private static IScheduler _getScheduler(string ip)
            {
                if (!ConnectionCache.ContainsKey(ip))
                {
                    var properties = new NameValueCollection();

                    properties["quartz.scheduler.proxy"] = "true";
                    properties["quartz.scheduler.proxy.address"] = $"{channelType}://{localIp}:{port}/{bindName}";
                    var schedulerFactory = new StdSchedulerFactory(properties);
                    _scheduler = schedulerFactory.GetScheduler().Result;
                    ConnectionCache[ip] = _scheduler;
                }
                return ConnectionCache[ip];
            }
        }
    }
}
