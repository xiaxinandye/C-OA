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
    public class ActionInfoController : BaseCheckSessionController
    {
        // GET: ActionInfo
        IActionInfoService ActionInfoService { get; set; }
        public ActionResult Index()
        {
            return View();
        }
        #region 查询出指定页码的权限数据
        public ActionResult GetActionInfo()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 5;
            short delFlag = (short)DeleteEnumType.Normal;
            var actionInfoList = ActionInfoService.LoadPageEntities<int>(
                pageIndex, pageSize, out int totalCount, a => a.DelFlag == delFlag, a => a.ID, true);
            var temp = from a in actionInfoList
                       select new
                       {
                           ID=a.ID,
                           ActionInfoName=a.ActionInfoName,
                           Url=a.Url,
                           ControllerName=a.ControllerName,
                           ActionInfoMethodName=a.ActionMethodName,
                           ActionTypeEnum=a.ActionTypeEnum==0?"非菜单权限": "菜单权限",
                           HttpMethod=a.HttpMethod,
                           MenuIcon=a.MenuIcon,
                           Sort=a.Sort,
                           Remark=a.Remark,
                           SubTime=a.SubTime
                       };
            return Json(new { rows = temp, total = totalCount });
        }
        #endregion

        #region 展示要修改的权限数据
        public ActionResult ShowEditInfo()
        {
            int id = Request["Id"] != null ? Convert.ToInt32(Request["Id"]) : 0;
            var actionInfo = ActionInfoService.LoadEntities(a => a.ID == id).FirstOrDefault();
            Response.ContentType = "application/json";
            if (actionInfo == null)
            {
                return Content("no");
            }
            return Content(Common.SerializeHelper.SerializeToString(actionInfo));
        }
        #endregion

        #region 修改权限
        public ActionResult EditActionInfo(ActionInfo actionInfo)
        {
            actionInfo.ModifiedOn = DateTime.Now;
            if (ActionInfoService.EditEntity(actionInfo))
            {
                return Content("ok");
            }
            else
            {
                return Content("failed");
            }
        }
        #endregion

        #region 删除用户数据
        public ActionResult DeleteActionInfo()
        {
            string strId = Request["strId"];
            string[] strIds = strId.Split(',');
            List<int> list = new List<int>();
            foreach (string id in strIds)
            {
                list.Add(Convert.ToInt32(id));
            }
            //将list集合中存储的要删除的数据编号传递到业务层
            if (ActionInfoService.DeleteEntities(list))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion

        #region 添加权限表单
        public ActionResult ShowAddInfo()
        {
            return View();
        }
        #endregion
        #region 添加权限数据
        public ActionResult AddActionInfo(ActionInfo actionInfo)
        {
            actionInfo.ModifiedOn = DateTime.Now;
            actionInfo.SubTime = DateTime.Now;
            actionInfo.DelFlag = 0;
            actionInfo.IconHeight = 0;
            actionInfo.IconWidth = 0;
            ActionInfo newActionInfo = ActionInfoService.AddEntity(actionInfo);
            if (newActionInfo != null)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        

    }
}