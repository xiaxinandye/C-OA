using log4net;
using OA.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OA.WebApp
{
    public class MvcApplication : Spring.Web.Mvc.SpringMvcApplication
    {
        protected void Application_Start()
        {
            //读取配置文件中关于log4net的信息
            log4net.Config.XmlConfigurator.Configure();

            IndexManager.GetInstance().StartThread();//开启线程，扫描luceneNet的队列
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //开启一个线程，扫描异常信息队列
            string filePath = Server.MapPath("/Log/");
            ThreadPool.QueueUserWorkItem((a) =>
            { 
                while(true)
                {
                    //判断队列是否有数据
                    if(Common.RedisHelper.ListLength(Common.CacheFolderEnum.Folder2, "errorQueue")>0)
                    {
                        Exception ex = Common.RedisHelper.ListLeftPop<Exception>(Common.CacheFolderEnum.Folder2 ,"errorQueue");
                        if (ex!=null)
                        {
                            //将异常信息写入到日志中
                            //string fileName = DateTime.Now.ToString("yyyy-MM-dd");
                            //File.AppendAllText(filePath + fileName + ".txt", ex.ToString(), Encoding.UTF8);
                            ILog logger = LogManager.GetLogger("errorMsg");
                            logger.Error(ex.ToString());
                        }
                        else
                        {
                            //如果队列中没有数据，休息
                            Thread.Sleep(3000);
                        }
                    }
                    else
                    {   
                        //休息
                        Thread.Sleep(3000);
                    }
                }
            },filePath);

            //自动迁移
            Database.SetInitializer<OA.Model.OAEntities>(new MigrateDatabaseToLatestVersion<OA.Model.OAEntities, OA.Model.Migrations.Configuration>());
            var dbMigrator = new DbMigrator(new OA.Model.Migrations.Configuration());
            dbMigrator.Update();
        }
    }
}
