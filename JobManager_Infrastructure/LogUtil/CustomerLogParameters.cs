using System;
using System.Collections.Generic;
using System.Text;

namespace JobManager_Infrastructure.LogUtil
{
    public class CustomerLogParameters
    {
        public CustomerLogParameters(string uniqueId, string reuqestMethodType, string callParameters, string callResult,string callStartTime,string callEndTime,string elapsedTime)
        {
            if (!string.IsNullOrWhiteSpace(uniqueId))
            {
                UniqueId = uniqueId;
            }
            ReuqestMethodType = string.IsNullOrWhiteSpace(reuqestMethodType) ? "" : reuqestMethodType;
            CallParameters = string.IsNullOrWhiteSpace(callParameters) ? "" : callParameters;
            CallResult =string.IsNullOrWhiteSpace( callResult)?"": callResult;
            CallStartTime = string.IsNullOrWhiteSpace(callStartTime) ? "" : callStartTime;
            CallEndTime = string.IsNullOrWhiteSpace(callEndTime) ? "" : callEndTime;
            ElapsedTime = string.IsNullOrWhiteSpace(elapsedTime) ? "" : elapsedTime;
        }
        /// <summary>
        /// 当前请求唯一流水号
        /// </summary>
        public string UniqueId { get; private set; }
        /// <summary>
        /// 当前请求方式
        /// </summary>
        public string ReuqestMethodType { get; private set; }
        /// <summary>
        /// 调用参数
        /// </summary>
        public string CallParameters { get; private set; }
        /// <summary>
        /// 调用结果
        /// </summary>
        public string CallResult { get; private set; }
        /// <summary>
        /// 调用开始时间
        /// </summary>
        public string  CallStartTime { get; set; }
       /// <summary>
       /// 调用结束时间
       /// </summary>
        public string CallEndTime { get; set; }
        /// <summary>
        /// 调用耗时
        /// </summary>
        public string ElapsedTime { get; set; }
    }
}
