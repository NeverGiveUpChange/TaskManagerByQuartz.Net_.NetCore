using JobManager_Model;
using JobManager_RepositoryInterface;
using Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace JobManager_RepositoryImplements
{
    internal class CustomerJobInfoRepository : ICustomerJobInfoRepository
    {
        readonly DbContext _dbContext;
        public CustomerJobInfoRepository(DbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public int AddCustomerJobInfo(string jobName, string jobGroupName, string triggerName, string triggerGroupName, string cron, string jobDescription, string requestUrl, int? cycle, int? repeatCount, string triggerType)
        {
            //TODO:任务具体类的信息 需配置到配置文件中
            return _dbContext.customer_quartzjobinfoDb.InsertReturnIdentity(new customer_quartzjobinfo { CreateTime = DateTime.Now, Cron = cron, Cycle = cycle, Deleted = 0, Description = jobDescription, DLLName = "", FullJobName = "", JobGroupName = jobGroupName, JobName = jobName, RepeatCount = repeatCount, RequestUrl = requestUrl, TriggerGroupName = triggerGroupName, TriggerName = triggerName, TriggerState = -1, TriggerType = triggerType });
        }

        public customer_quartzjobinfo LoadCustomerInfo(int id)
        {
            return _dbContext.customer_quartzjobinfoDb.GetById(id);
        }

        public (List<customer_quartzjobinfo>, int) LoadCustomerInfoes(Expression<Func<customer_quartzjobinfo, bool>> whereExpression, Expression<Func<customer_quartzjobinfo, object>> orderByExpression, bool isAsc, int pageIndex, int pageSize)
        {
            var pageModel = new PageModel() { PageIndex = pageIndex, PageSize = pageSize };
            var result = _dbContext.customer_quartzjobinfoDb.GetPageList(whereExpression, pageModel, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc);
            return (result, pageModel.PageCount);

        }

        public bool UpdateCustomerJobInfo(Expression<Func<customer_quartzjobinfo,customer_quartzjobinfo>>cloums,Expression<Func<customer_quartzjobinfo,bool>> whereExpression)
        {
            return _dbContext.customer_quartzjobinfoDb.Update(cloums, whereExpression);
        }
    }
}
