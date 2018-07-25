using System;
using System.Linq;
using System.Text;

namespace Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class customer_quartzjobinfo
    {
           public customer_quartzjobinfo(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int Id {get;set;}

           /// <summary>
           /// Desc:任务名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string JobName {get;set;}

           /// <summary>
           /// Desc:任务组名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string JobGroupName {get;set;}

           /// <summary>
           /// Desc:触发器名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string TriggerName {get;set;}

           /// <summary>
           /// Desc:触发器组名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string TriggerGroupName {get;set;}

           /// <summary>
           /// Desc:运行周期表达式
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Cron {get;set;}

           /// <summary>
           /// Desc:任务状态
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int TriggerState {get;set;}

           /// <summary>
           /// Desc:任务所在程序集
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string DLLName {get;set;}

           /// <summary>
           /// Desc:任务类完全名
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string FullJobName {get;set;}

           /// <summary>
           /// Desc:任务异常信息
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Exception {get;set;}

           /// <summary>
           /// Desc:任务请求得业务地址
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string RequestUrl {get;set;}

           /// <summary>
           /// Desc:任务执行时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? JobStartTime {get;set;}

           /// <summary>
           /// Desc:上次执行时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? PreTime {get;set;}

           /// <summary>
           /// Desc:结束时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? EndTime {get;set;}

           /// <summary>
           /// Desc:下次执行时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? NextTime {get;set;}

           /// <summary>
           /// Desc:任务描述信息
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Description {get;set;}

           /// <summary>
           /// Desc:是否删除
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public byte Deleted {get;set;}

           /// <summary>
           /// Desc:创建时间
           /// Default:
           /// Nullable:False
           /// </summary>           
           public DateTime CreateTime {get;set;}

           /// <summary>
           /// Desc:重复时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? Cycle {get;set;}

           /// <summary>
           /// Desc:重复次数
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? RepeatCount {get;set;}

           /// <summary>
           /// Desc:触发器类型
           /// Default:JobCronTrigger
           /// Nullable:False
           /// </summary>           
           public string TriggerType {get;set;}

    }
}
