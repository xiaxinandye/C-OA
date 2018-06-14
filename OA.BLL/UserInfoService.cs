using OA.IBLL;
using OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.BLL
{
    public partial class UserInfoService : BaseService<UserInfo>,IUserInfoService
    {
        //public override void SetCurrentDal()
        //{
        //    CurrentDal = CurrentDBSession.UserInfoDal;
        //}
        /// <summary>
        /// 批量删除多条数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DeleteEntities(List<int> list)
        {
            var userInfoList = this.CurrentDBSession.UserInfoDal.LoadEntities(
                u => list.Contains(u.ID));
            foreach (var userInfo in userInfoList)
            {
                this.CurrentDBSession.UserInfoDal.DeleteEntity(userInfo);
            }
            return this.CurrentDBSession.SaveChanges();
        }

        public bool SetUserRoleInfo(int userid, int roleid, bool isallow)
        {
            var user = LoadEntities(u => u.ID == userid).FirstOrDefault();//根据id查找出用户的信息
            var role = CurrentDBSession.RoleInfoDal.LoadEntities(r => r.ID == roleid).FirstOrDefault(); //根据id查找出角色的信息
            if(isallow)
            {
                user.RoleInfo.Add(role);
                return CurrentDBSession.SaveChanges();
              
            }
            else
            {
                user.RoleInfo.Remove(role);
                return CurrentDBSession.SaveChanges();
            }
        }
    }
}
