using OA.BLL;
using OA.IBLL;
using OA.Model;
using OA.Model.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace OA.WebApp.Controllers
{
    public class UserInfoController : BaseCheckSessionController
    {
        // GET: UserInfo
        IUserInfoService UserInfoService { get; set; }
        IR_UserInfo_ActionInfoService R_UserInfo_ActionInfoService { get; set; }
        IActionInfoService ActionInfoService { get; set; }
        IRoleInfoService RoleInfoService { get; set; }
        public ActionResult Index()
        {
           
            return View();
        }
        #region 加载用户数据
        public ActionResult GetUserInfo()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 5;
            short delFlag = (short)DeleteEnumType.Normal;
            var userInfoList = UserInfoService.LoadPageEntities<int>(
                pageIndex, pageSize, out int totalCount, u => u.DelFlag == delFlag, u => u.ID, true);
            var temp = from u in userInfoList
                       select new
                       {
                           ID = u.ID,
                           UName = u.UName,
                           UPwd = u.UPwd,
                           Remark = u.Remark,
                           SubTime = u.SubTime
                       };
            return Json(new { rows = temp, total = totalCount });
        }
        #endregion

        #region 删除用户数据
        public ActionResult DeleteUserInfo()
        {
            string strId = Request["strId"];
            string[] strIds = strId.Split(',');
            List<int> list = new List<int>();
            foreach (string id in strIds)
            {
                list.Add(Convert.ToInt32(id));
            }
            //将list集合中存储的要删除的数据编号传递到业务层
            if (UserInfoService.DeleteEntities(list))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion

        #region 添加用户数据
        public ActionResult AddUserInfo(UserInfo user)
        {
            user.DelFlag = 0;
            user.ModifiedOn = DateTime.Now;
            user.SubTime = DateTime.Now;
            UserInfoService.AddEntity(user);
            return Content("ok");
        }
        #endregion

        #region 展示要修改的用户数据
        public ActionResult ShowEditInfo()
        {
            int id = int.Parse(Request["Id"]);
            var userInfo = UserInfoService.LoadEntities(u => u.ID == id).ToList().FirstOrDefault();
            Response.ContentType = "application/json";
            return Content(Common.SerializeHelper.SerializeToString(userInfo));
            // return Json(userInfo, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region 修改数据
        public ActionResult EditUserInfo(UserInfo userInfo)
        {

            userInfo.ModifiedOn = DateTime.Now;
            if (UserInfoService.EditEntity(userInfo))
            {
                return Content("ok");
            }
            else
            {
                return Content("failed");
            }
        }
        #endregion

     

        #region 展示要为用户分配的权限
        public ActionResult ShowPermissions()
        {
            int id = Request["id"] == null ? 0 : int.Parse(Request["id"]);//获取从userinfo/index传来的id值
            var user = UserInfoService.LoadEntities(u => u.ID == id).FirstOrDefault();
            ViewBag.userInfo = user;
            short deFlag =(short) DeleteEnumType.Normal;
            var permissions = ActionInfoService.LoadEntities(a => a.DelFlag==deFlag).ToList();//获取所有的权限,不采用延迟加载，避免嵌套循环时，出错（未关闭sqldatareader）
            ViewBag.permissions = permissions;
            var userAction = R_UserInfo_ActionInfoService.LoadEntities(u => u.UserInfoID == id).ToList();//获取该用户对应的权限记录
            ViewBag.userAction = userAction;
            return View();
        }
        #endregion

        #region 改变用户的权限
        public ActionResult ChangeAction()
        {
            int userid = Request["userid"] != null ? int.Parse(Request["userid"]) : 0;
            int actionid = Request["actionid"] != null ? int.Parse(Request["actionid"]) : 0;
            bool isAllow = Request["radiovalue"].ToString() == "true" ? true : false;
            R_UserInfo_ActionInfo useraction = R_UserInfo_ActionInfoService.LoadEntities(u => u.UserInfoID == userid && u.ActionInfoID == actionid).FirstOrDefault();
            if (useraction == null)//R_UserInfo_ActionInfo表里未有该记录
            {
                //添加记录
                R_UserInfo_ActionInfo newuseraction = new R_UserInfo_ActionInfo
                {
                    UserInfoID = userid,
                    ActionInfoID = actionid,
                    IsPass = isAllow
                };
                R_UserInfo_ActionInfoService.AddEntity(newuseraction);
                return Content("成功修改权限");
            }
            else
            {
                //修改记录
                useraction.IsPass = isAllow;
                R_UserInfo_ActionInfoService.EditEntity(useraction);
                return Content("成功修改权限");
            }

        }
        #endregion

        #region 收回用户的权限
        public ActionResult CleanAction()
        {
            int userid = Request["userid"] != null ? int.Parse(Request["userid"]) : 0;
            int actionid = Request["actionid"] != null ? int.Parse(Request["actionid"]) : 0;
            R_UserInfo_ActionInfo useraction = R_UserInfo_ActionInfoService.LoadEntities(u => u.UserInfoID == userid && u.ActionInfoID == actionid).FirstOrDefault();
            if (useraction == null)//R_UserInfo_ActionInfo表里未有该记录
            {
                return Content("noexsit");
            }
            else
            {
                if (R_UserInfo_ActionInfoService.DeleteEntity(useraction))//删除
                {
                    return Content("ok");
                }
                else
                {
                    return Content("fail");
                }
            }
        }
        #endregion

        #region 展示要为用户分配的角色
        public ActionResult ShowRole()
        {
            int id = Request["id"] == null ? 0 : int.Parse(Request["id"]);//获取从userinfo/index传来的id值
            var user = UserInfoService.LoadEntities(u => u.ID == id).FirstOrDefault();
            ViewBag.userInfo = user;
            short deFlag = (short)DeleteEnumType.Normal;
            var roleInfoList = RoleInfoService.LoadEntities(r => r.DelFlag==deFlag).ToList();//获取所有的权限,不采用延迟加载，避免嵌套循环时，出错（未关闭sqldatareader）
            ViewBag.role = roleInfoList;
            var userRoleId = (from r in user.RoleInfo
                              select r.ID).ToList();//获取该用户所具有的角色Id编号
            ViewBag.userRoleId = userRoleId;
            return View();
        }
        #endregion

        #region 改变用户的角色
        public ActionResult ChangeRole()
        {
            int userid = Request["userid"] != null ? int.Parse(Request["userid"]) : 0;
            int roleid = Request["roleid"] != null ? int.Parse(Request["roleid"]) : 0;
            bool isAllow = Request["isAllow"].ToString() == "true" ? true : false;
            if(isAllow)
            {
                if (UserInfoService.SetUserRoleInfo(userid, roleid, isAllow))
                {
                    return Content("add");
                }
                else
                {
                    return Content("error");
                }
            }
        
            else
            {
                if (UserInfoService.SetUserRoleInfo(userid, roleid, isAllow))
                {
                    return Content("remove");
                }
                else
                {
                    return Content("error");
                }
            }
        }
        #endregion
    }
}