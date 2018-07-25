using JobManager_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobManager_Web.Factory
{
    public class ResponseDataFactory
    {
        public static AjaxResponseData CreateAjaxResponseData(string statusCode, string message, object data)
        {

            return new AjaxResponseData() { StausCode = statusCode, Message = message, Data = data };


        }
    }
}
