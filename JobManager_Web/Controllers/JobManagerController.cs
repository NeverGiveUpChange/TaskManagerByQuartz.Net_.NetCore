using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using JobManager_Core.JobTriggerAbstract;
using JobManager_RepositoryInterface;
using JobManager_Web.Factory;
using JobManager_Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Models;
using NLog;

namespace JobManager_Web.Controllers
{
    [Route("jobweb/jobs")]
    public class JobManagerController : Controller
    {
        readonly ICustomerJobInfoRepository _customerJobInfoRepository;
        readonly IEnumerable<JobBaseTrigger> _triggerBases;
        JobBaseTrigger _triggerBase;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public JobManagerController(ICustomerJobInfoRepository _customerJobInfoRepository, IEnumerable<JobBaseTrigger> _triggerBases)
        {

            this._customerJobInfoRepository = _customerJobInfoRepository;
            this._triggerBases = _triggerBases;
        }
        [HttpGet]
        public ActionResult Test(int id) {
            _logger.Info("测试本地请求日志");
            return Json("");
        }
        // GET: JobManager
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost("testmodel")]
        public ActionResult TestModel([FromBody]AA aA) {
            if (!ModelState.IsValid) {
                return new UnprocesableEntityObjectResult(ModelState);
            }
            return Content("");
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="addJobViewModel">添加任务模型</param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult AddJob([FromBody]AddJobViewModel addJobViewModel)
        {
            AjaxResponseData ajaxResponseData = null;
            if (!ModelState.IsValid)
            {
                ajaxResponseData = ResponseDataFactory.CreateAjaxResponseData("-10001", "添加失败", $"输入参数错误：{ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + ($"{item.Errors[0].ErrorMessage} ;  "))}");
            }
            else
            {
                var jobId = _customerJobInfoRepository.AddCustomerJobInfo(addJobViewModel.JobName, addJobViewModel.JobGroupName, addJobViewModel.TriggerName, addJobViewModel.TriggerGroupName, addJobViewModel.CronJob == null ? null : addJobViewModel.CronJob.Cron, addJobViewModel.JobDescription, addJobViewModel.RequestUrl, addJobViewModel.SimpleJob == null ? null : addJobViewModel.SimpleJob.Cycle, addJobViewModel.SimpleJob == null ? null : addJobViewModel.SimpleJob.RepeatCount, addJobViewModel.TriggerType);

                ajaxResponseData = ResponseDataFactory.CreateAjaxResponseData("1", "添加成功", jobId);

            }
            return Json(ajaxResponseData);
        }
        /// <summary>
        /// 更改任务周期
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <param name="cron">任务执行周期表达式</param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult ModifyJobCron(int jobId, string cron)
        {

            var ajaxResponseData = _operateJob(jobId, (jobDetail) => { jobDetail.Cron = cron; _customerJobInfoRepository.UpdateCustomerJobInfo(x => new customer_quartzjobinfo { Cron = cron }, s => s.Id == jobId); return _triggerBase.ModifyJobCron(jobDetail); });
            return Json(ajaxResponseData);
        }
        /// <summary>
        /// 任务运行时更新任务状态信息
        /// </summary>
        /// <param name="updateJobInfoViewModel">更新模型</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateJobInfo(UpdateJobInfoViewModel updateJobInfoViewModel)
        {

            var jobName = _customerJobInfoRepository.UpdateCustomerJobInfo(x => new customer_quartzjobinfo { JobName = updateJobInfoViewModel.JobName, Deleted = updateJobInfoViewModel.Deleted, TriggerState = updateJobInfoViewModel.JobState, Exception = updateJobInfoViewModel.Exception, PreTime = updateJobInfoViewModel.PreTime, NextTime = updateJobInfoViewModel.NextTime }, s => s.Id == updateJobInfoViewModel.Id);
            return Json(ResponseDataFactory.CreateAjaxResponseData("1", "操作成功", jobName));
        }
        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult RunJob(int jobId)
        {
            var ajaxResponseData = _operateJob(jobId, (jobDetail) => { return _triggerBase.RunJob(jobDetail); });
            return Json(ajaxResponseData);
        }
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteJob(int jobId)
        {
            var ajaxResponseData = _operateJob(jobId, (jobDetail) => { return _triggerBase.DeleteJob(jobDetail); });
            return Json(ajaxResponseData);
        }
        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult PauseJob(int jobId)
        {
            var ajaxResponseData = _operateJob(jobId, (jobDetail) => { return _triggerBase.PauseJob(jobDetail); });
            return Json(ajaxResponseData);
        }
       
        /// <summary>
        /// 恢复任务
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ResumeJob(int jobId)
        {
            var ajaxResponseData = _operateJob(jobId, (jobDetail) => { return _triggerBase.ResumeJob(jobDetail); });
            return Json(ajaxResponseData);

        }
        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="jobStatus">任务状态</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页数量</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetJobList(int jobStatus, int pageIndex, int pageSize)
        {

            var jobQueryable = _customerJobInfoRepository.LoadCustomerInfoes(x => x.TriggerState == jobStatus, x => x.Id, false, pageIndex, pageSize);
            var JobList = jobQueryable.Item1.ToList().Select(x => new
            {
                x.Id,
                x.JobName,
                TriggerType = x.TriggerType == "JobCronTrigger" ? "复杂任务" : "简单任务",
                x.Description,
                x.Cron,
                PreTime = x.PreTime.HasValue ? x.PreTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                NextTime = x.NextTime.HasValue ? x.NextTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                JobStartTime = x.JobStartTime.HasValue ? x.JobStartTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                CreateTime = x.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                x.Exception
            }).ToList();
            var ajaxResponseData = ResponseDataFactory.CreateAjaxResponseData("1", "获取成功", new { JobList, TotalCount = jobQueryable.Item2 });
            return Json(ajaxResponseData);
        }

        /// <summary>
        /// 操作任务
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <param name="operateJobFunc">具体操作任务的委托</param>
        /// <returns></returns>
        private AjaxResponseData _operateJob(int jobId, Func<customer_quartzjobinfo, bool> operateJobFunc)
        {
            AjaxResponseData ajaxResponseData = null;
            var jobDetail = _customerJobInfoRepository.LoadCustomerInfo(jobId);
            if (jobDetail == null)
            {
                ajaxResponseData = ResponseDataFactory.CreateAjaxResponseData("0", "无此任务", jobDetail);
            }
            else
            {
                _setSpecificTrigger(jobDetail.TriggerType);
                var isSuccess = operateJobFunc(jobDetail);
                if (isSuccess)
                {
                    ajaxResponseData = ResponseDataFactory.CreateAjaxResponseData("1", "操作成功", jobDetail);
                }
                else
                {
                    ajaxResponseData = ResponseDataFactory.CreateAjaxResponseData("-10001", "操作失败", jobDetail);
                }
            }
            return ajaxResponseData;
        }

        private void _setSpecificTrigger(string triggerType)
        {
            _triggerBase = _triggerBases.FirstOrDefault(x => x.GetType().Name == triggerType);
        }
    }
    public class AA {
        [Display(Name ="主键编号")]
        [Required,Range(1,3,ErrorMessage ="主键必须在1-3之间")]
        public int Id { get; set; }
    }
    public class UnprocesableEntityObjectResult : ObjectResult {
        public UnprocesableEntityObjectResult(ModelStateDictionary modelState) : base(new SerializableError( modelState)) {

            if (modelState == null) {
                throw new ArgumentNullException(nameof(modelState));
            }
            StatusCode = 422;
        }
    }
}