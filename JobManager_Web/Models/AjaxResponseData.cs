using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobManager_Web.Models
{
    public class AjaxResponseData
    {

        /// <summary>
        /// 状态码
        /// </summary>
        public string StausCode { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }
    }
}
