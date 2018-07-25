
using System;
using NLog;


namespace JobManager_Infrastructure.LogUtil
{
    public class CustomerLogUtil
    {
        private static readonly ILogger _logger = LogManager.GetLogger("job_web");
        public static void Info(string message, CustomerLogParameters parameters)
        {

            _logger.Info(message, parameters.CallParameters, parameters.CallResult, parameters.ReuqestMethodType, parameters.UniqueId,parameters.CallStartTime,parameters.CallEndTime,parameters.ElapsedTime,"","");
        }
        public static void Error(Exception ex, string message, CustomerLogParameters parameters) {
          
            _logger.Error( message, parameters.CallParameters, parameters.CallResult, parameters.ReuqestMethodType, parameters.UniqueId, parameters.CallStartTime, parameters.CallEndTime, parameters.ElapsedTime, ex.Message,ex.StackTrace.Replace("\n","====>"));
        }
    }
}
