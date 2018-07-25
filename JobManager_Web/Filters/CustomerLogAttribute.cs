using JobManager_Infrastructure.LogUtil;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobManager_Web.Filters
{
    public class CustomerLogAttribute : ActionFilterAttribute, IExceptionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var uniqueId = Guid.NewGuid().ToString("N");
            context.HttpContext.Request.Headers.Add("UniqueId", uniqueId);
            var requestStartTime = DateTime.Now;
            context.HttpContext.Request.Headers.Add("RequestStartTime", requestStartTime.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
            context.HttpContext.Request.Headers.Add("RequestStartTimeTicks", requestStartTime.Ticks.ToString());
            base.OnActionExecuting(context);
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var requestEndTime = DateTime.Now;
            var requestStartTime = context.HttpContext.Request.Headers["RequestStartTime"];
            var requestStartTimeTicks = Convert.ToInt64(context.HttpContext.Request.Headers["RequestStartTimeTicks"]);
            var uniqueId = context.HttpContext.Request.Headers["UniqueId"];
            var url_Method_Parameters = _getRequestMethodAndParameters(context);
            var result = context.Result as JsonResult;
            if (result != null)
            {

                CustomerLogUtil.Info($"Action请求#{url_Method_Parameters.Item1}", new CustomerLogParameters(uniqueId, url_Method_Parameters.Item2, url_Method_Parameters.Item3, JsonConvert.SerializeObject(result.Value), requestStartTime, requestEndTime.ToString("yyyy-MM-dd HH:mm:ss.ffff"), $"{((requestEndTime.Ticks - requestStartTimeTicks) / 10000)}ms"));
            }
            else
            {
                CustomerLogUtil.Info($"Action请求#{url_Method_Parameters.Item1}", new CustomerLogParameters(uniqueId, url_Method_Parameters.Item2, url_Method_Parameters.Item3, "", requestStartTime, requestEndTime.ToString("yyyy-MM-dd HH:mm:ss.ffff"), $"{((requestEndTime.Ticks - requestStartTimeTicks) / 10000)}ms"));
            }

        
            base.OnActionExecuted(context);
        }
        public void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled) return;
            var requestEndTime = DateTime.Now;
            var exception = context.Exception;
            var requestStartTime = context.HttpContext.Request.Headers["RequestStartTime"];
            var requestStartTimeTicks = Convert.ToInt64(context.HttpContext.Request.Headers["RequestStartTimeTicks"]);
            var url_Method_Parameters = _getRequestMethodAndParameters(context);

            CustomerLogUtil.Error(exception, $"Action请求发生异常#{url_Method_Parameters.Item1}", new CustomerLogParameters(context.HttpContext.Request.Headers["UniqueId"], url_Method_Parameters.Item2, url_Method_Parameters.Item3, "", requestStartTime, requestEndTime.ToString("yyyy-MM-dd HH:mm:ss.ffff"), $"{((requestEndTime.Ticks - requestStartTimeTicks) / 10000)}ms"));
            context.ExceptionHandled = true;
        }
        private (string, string, string) _getRequestMethodAndParameters(ActionContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            var httpMethod = request.Method;
            var requestParameters = string.Empty;
            if (string.Compare(httpMethod, "get", true) == 0)
            {
                requestParameters = request.QueryString.Value;
            }
            else if (string.Compare(httpMethod, "post", true) == 0)
            {

                var buffer = new MemoryStream();
                request.Body.CopyTo(buffer);
                buffer.Position = 0;
                var streamReader = new StreamReader(buffer);
                requestParameters = streamReader.ReadToEnd();
                buffer.Position = 0;
                request.Body = buffer;
            }
            return ($"{request.Host.ToString() + request.Path.ToString()}", httpMethod.ToUpper(), requestParameters);
        }
    }
}
