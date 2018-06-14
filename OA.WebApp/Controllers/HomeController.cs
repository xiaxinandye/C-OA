using OA.IBLL;
using OA.Model;
using OA.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OA.WebApp.Controllers
{
    public class HomeController : BaseCheckSessionController
    {
        IUserInfoService UserInfoService { get; set; }
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.userName = LoginUser.UName;//便于前台展示登录用户
            return View();
        }
    
        public ActionResult HomePage()
        {
            return View();
        }
        #region 过滤登录者具有的菜单权限
        /// <summary>
        /// 1: 可以按照用户---角色---权限这条线找出登录用户的权限，放在一个集合中。
        /// 2：可以按照用户---权限这条线找出用户的权限，放在一个集合中。
        /// 3：将这两个集合合并成一个集合。
        /// 4：把禁止的权限从总的集合中清除。
        /// 5：将总的集合中的重复权限清除。
        /// 6：把过滤好的菜单权限生成JSON返回。
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMenus()
        {
            //1: 可以按照用户-- - 角色-- - 权限这条线找出登录用户的权限，放在一个集合中
            UserInfo userInfo = UserInfoService.LoadEntities(u => u.ID == LoginUser.ID).FirstOrDefault();//查找出登录用户的信息
            var userRole = userInfo.RoleInfo;//查找出用户的所有角色
            short actionMenu = (short)ActionTypeEnum.Menu;
            var userRoleAction = (from a in userRole
                             from aa in a.ActionInfo
                             where aa.ActionTypeEnum == actionMenu
                             select aa).ToList();
            //2：可以按照用户-- - 权限这条线找出用户的权限，放在一个集合中。
            var userAction = (from a in userInfo.R_UserInfo_ActionInfo
                             where a.ActionInfo.ActionTypeEnum == actionMenu
                             select a.ActionInfo).ToList();
            //3：将这两个集合合并成一个集合。
           userRoleAction.AddRange(userAction);
            // 4：把禁止的权限从总的集合中清除。
            var forbidActionID = (from a in userInfo.R_UserInfo_ActionInfo
                                 where a.IsPass == false
                                 select a.ActionInfoID).ToList();//1,3,4,5
          
            var userAllowMenuAction = userRoleAction.Where(a => !forbidActionID.Contains(a.ID)); //IEnumerable的扩展方法
                                                                                                 //5：将总的集合中的重复权限清除
            var lastLoginUserActions = userAllowMenuAction.Distinct(new EqualityComparer()).ToList();
            //6：把过滤好的菜单权限生成JSON返回。
            var temp = from a in lastLoginUserActions
                       select new { icon = a.MenuIcon, title = a.ActionInfoName, url = a.Url };
            return Json(temp, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}