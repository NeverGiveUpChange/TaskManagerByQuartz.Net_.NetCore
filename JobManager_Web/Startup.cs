using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Elmah.Io.AspNetCore;
using JobManager_Model;
using JobManager_Web.Filters;
using JobManager_Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using NLog.Extensions.Logging;
using NLog.Web;

namespace JobManager_Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {

                options.Filters.Add(new CustomerLogAttribute());
            }); 
            //services.AddMvc();

            //services.AddElmahIo(options =>
            //{
            //    options.ApiKey = "3d66b03397ab4e23b6910c2e967edb98";
            //    options.LogId = new Guid("9e6c4337-fcd2-438a-ab9e-0945bd11899e");
            //});
            services.AddScoped<DbContext>();
            var result = _getClassInterfaceDic("JobManager_RepositoryImplements");
            foreach (var item in result)
            {
                foreach (var interfaceTypes in item.Value)
                {
                    services.AddScoped(interfaceTypes, item.Key);
                 
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            env.ConfigureNLog("NLog.config");
          
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseElmahIo();
            app.UseStaticFiles();
      
            #region SqlSugar DbFirst
            //var db = app.ApplicationServices.GetRequiredService<DbContext>();
            //db.Db.DbFirst.CreateClassFile(@"E:\BiHuProjects\JobManagerByQuartz_.NetCore_1.0\JobManagerByQuartz\JobManager_Model\Models");
            #endregion

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        /// <summary>
        /// 用于服务注册。获取当前程序集中的类与接口或者抽象类的对应关系字典
        /// </summary>
        /// <param name="assermblyName">程序集名称</param>
        /// <returns></returns>
        private Dictionary<Type, Type[]> _getClassInterfaceDic(string assermblyName)
        {
            var result = new Dictionary<Type, Type[]>();
            if (!string.IsNullOrWhiteSpace(assermblyName))
            {
                var assembly = Assembly.Load(assermblyName);
                var typeList = assembly.GetTypes().ToList();
                var implementTypes = typeList.Where(x => !x.IsInterface && !x.IsAbstract);

                foreach (var item in implementTypes)
                {
                    var interfaceTypes = item.GetInterfaces();
                    var baseType = item.BaseType;
                    if (baseType.Name != "Object")
                    {
                        interfaceTypes.Append(baseType);
                    }
                    result.Add(item, interfaceTypes);
                }
            }
            return result;
        }
    }
}
