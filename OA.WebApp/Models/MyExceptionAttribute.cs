using OA.Common;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OA.WebApp.Models
{
    public class MyExceptionAttribute : HandleErrorAttribute
    {
        public static Queue<Exception> ExcptionQueue = new Queue<Exception>();
      
        /// <summary>
        /// 可以捕获异常数据
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
    
            base.OnException(filterContext);
            Exception ex = filterContext.Exception;
            //写到日志中.多个线程同时操作一个文件，造成文件的并发。
            RedisHelper.ListLeftPush(CacheFolderEnum.Folder2, "errorQueue", ex);//写入队列
            //ExcptionQueue.Enqueue(ex);//写入队列
            //跳到错误页
            filterContext.HttpContext.Response.Redirect("/Error.html");
        }
    }
}