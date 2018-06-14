using OA.IBLL;
using OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.BLL
{
    public partial class ActionInfoService : BaseService<ActionInfo>, IActionInfoService
    {
        /// <summary>
        /// 批量删除多条数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DeleteEntities(List<int> list)
        {
            var actionInfoList = this.CurrentDBSession.ActionInfoDal.LoadEntities(
                a => list.Contains(a.ID));
            foreach (var actionInfo in actionInfoList)
            {
                this.CurrentDBSession.ActionInfoDal.DeleteEntity(actionInfo);
            }
            return this.CurrentDBSession.SaveChanges();
        }
        public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDBSession.ActionInfoDal;
        }
    }

    public partial class T_SearchLogsService : BaseService<T_SearchLogs>, IT_SearchLogsService
    {

        public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDBSession.T_SearchLogsDal;
        }
    }
    public partial class T_SearchLogStasticsService : BaseService<T_SearchLogStastics>, IT_SearchLogStasticsService
    {
        /// <summary>
        /// 删除总统计所有的数据
        /// </summary>
        /// <returns></returns>
        public bool DeleteAllStastics()
        {
             return this.CurrentDBSession.T_SearchLogStasticsDal.DeleteAllStastics()>0;
        }
        /// <summary>
        /// 将汇总表中匹配的词语返回
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public List<string> GetMatchWords(string str)
        {
            return this.CurrentDBSession.T_SearchLogStasticsDal.GetMatchWords(str);
        }

        /// <summary>
        /// 重新将明细表数据统计到总汇总统计表中
        /// </summary>
        /// <returns></returns>
        public bool InsertAllStastics()
        {
            return this.CurrentDBSession.T_SearchLogStasticsDal.InsertAllStastics() > 0;
        }

        public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDBSession.T_SearchLogStasticsDal;
        }
    }
    public partial class DepartmentService : BaseService<Department>, IDepartmentService
    {

        public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDBSession.DepartmentDal;
        }
    }


    public partial class BooksService : BaseService<Books>, IBooksService
    {


        public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDBSession.BooksDal;
        }
    }

    public partial class R_UserInfo_ActionInfoService : BaseService<R_UserInfo_ActionInfo>, IR_UserInfo_ActionInfoService
    {

        public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDBSession.R_UserInfo_ActionInfoDal;
        }
    }

    public partial class RoleInfoService : BaseService<RoleInfo>, IRoleInfoService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDBSession.RoleInfoDal;
        }
        /// <summary>
        /// 批量删除多条数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DeleteEntities(List<int> list)
        {
            var roleInfoList = this.CurrentDBSession.RoleInfoDal.LoadEntities(
                r => list.Contains(r.ID));
            foreach (var roleInfo in roleInfoList)
            {
                this.CurrentDBSession.RoleInfoDal.DeleteEntity(roleInfo);
            }
            return this.CurrentDBSession.SaveChanges();
        }

        /// <summary>
        /// 为角色分配权限
        /// </summary>
        /// <param name="actionid"></param>
        /// <param name="roleid"></param>
        /// <param name="isallow"></param>
        /// <returns></returns>
        public bool SetRoleActionInfo(int actionid, int roleid, bool isallow)
        {
            var role = LoadEntities(r => r.ID == roleid).FirstOrDefault();//根据id查找出角色的信息
            var action = CurrentDBSession.ActionInfoDal.LoadEntities(a => a.ID == actionid).FirstOrDefault(); //根据id查找出权限的信息
            if (isallow)
            {
                role.ActionInfo.Add(action);
                return CurrentDBSession.SaveChanges();

            }
            else
            {
                role.ActionInfo.Remove(action);
                return CurrentDBSession.SaveChanges();
            }
        }
    }

    public partial class UserInfoService : BaseService<UserInfo>, IUserInfoService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDBSession.UserInfoDal;
        }
    }
}
