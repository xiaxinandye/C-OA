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
    public class RoleInfoController : BaseCheckSessionController
    {
        // GET: RoleInfo
        IRoleInfoService RoleInfoService { get; set; }
        IActionInfoService ActionInfoService { get; set; }
        public ActionResult Index()
        {
            return View();
        }
        #region 加载角色数据
        public ActionResult GetRoleInfo()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 5;
            short delFlag = (short)DeleteEnumType.Normal;
            var roleInfoList = RoleInfoService.LoadPageEntities<int>(
                pageIndex, pageSize, out int totalCount, r => r.DelFlag == delFlag, r => r.ID, true);
            var temp = from r in roleInfoList
                       select new
                       {
                           ID = r.ID,
                           RName = r.RoleName,
                           Remark = r.Remark,
                           SubTime = r.SubTime
                       };
            return Json(new { rows = temp, total = totalCount });
        }
        #endregion

        #region 删除角色数据
        public ActionResult DeleteRoleInfo()
        {
            string strId = Request["strId"];
            string[] strIds = strId.Split(',');
            List<int> list = new List<int>();
            foreach (string id in strIds)
            {
                list.Add(Convert.ToInt32(id));
            }
            //将list集合中存储的要删除的数据编号传递到业务层
            if (RoleInfoService.DeleteEntities(list))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion

        #region 添加角色表单
        public ActionResult ShowAddInfo()
        {
            return View();
        }

        #endregion

        #region 添加角色数据
        public ActionResult AddRoleInfo(RoleInfo roleInfo)
        {
            roleInfo.ModifiedOn = DateTime.Now;
            roleInfo.SubTime = DateTime.Now;
            roleInfo.DelFlag = 0;
            RoleInfo newRoleInfo = RoleInfoService.AddEntity(roleInfo);
            if(newRoleInfo != null)
            {
                return Content("ok");
            }
           else
            {
                return Content("no");
            }
        }
        #endregion
      
        #region 展示要修改的角色数据
        public ActionResult ShowEditInfo()
        {
            int id = Request["Id"] != null ? Convert.ToInt32(Request["Id"]) : 0;
            var roleInfo = RoleInfoService.LoadEntities(r => r.ID == id).FirstOrDefault();
            Response.ContentType = "application/json";
            if (roleInfo==null)
            {
                return Content("no");
            }
            return Content(Common.SerializeHelper.SerializeToString(roleInfo));
        }
        #endregion EditRoleInfo

        #region 修改角色数据
        public ActionResult EditRoleInfo(RoleInfo roleInfo)
        {
            roleInfo.ModifiedOn = DateTime.Now;
            if(RoleInfoService.EditEntity(roleInfo))
            {
                return Content("ok");
            }
            else
            {
                return Content("error");
            }
        }
        #endregion

        #region 展示要为角色分配的权限
        public ActionResult ShowPermissions()
        {
            int id = Request["roleid"] == null ? 0 : int.Parse(Request["roleid"]);//获取从roleinfo/index传来的id值
            var role = RoleInfoService.LoadEntities(r => r.ID == id).FirstOrDefault();
            ViewBag.roleInfo = role;
            short deFlag = (short)DeleteEnumType.Normal;
            var permissions = ActionInfoService.LoadEntities(a => a.DelFlag == deFlag).ToList();//获取所有的权限,不采用延迟加载，避免嵌套循环时，出错（未关闭sqldatareader）
            ViewBag.permissions = permissions;
            var roleActionId = (from a in role.ActionInfo
                              select a.ID).ToList();//获取该角色所具有的权限Id编号
            ViewBag.roleActionId = roleActionId;
            return View();
        }
        #endregion

        #region 改变角色的权限
        public ActionResult ChangeAction()
        {
            int actionid = Request["actioid"] != null ? int.Parse(Request["actioid"]) : 0;
            int roleid = Request["roleid"] != null ? int.Parse(Request["roleid"]) : 0;
            bool isAllow = Request["isAllow"].ToString() == "true" ? true : false;
            if (isAllow)
            {
                if (RoleInfoService.SetRoleActionInfo(actionid, roleid, isAllow))
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
                if (RoleInfoService.SetRoleActionInfo(actionid, roleid, isAllow))
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