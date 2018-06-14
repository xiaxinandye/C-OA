using OA.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.DALFactory
{
    public partial class AbstractFactory
    {
        public static IT_SearchLogsDal CreateT_SearchLogsDal()
        {
            string fullClassName = NameSpace + ".T_SearchLogsDal";
            return CreateInstance(fullClassName) as IT_SearchLogsDal;

        }
        public static IT_SearchLogStasticsDal CreateT_SearchLogStasticsDal()
        {
            string fullClassName = NameSpace + ".T_SearchLogStasticsDal";
            return CreateInstance(fullClassName) as IT_SearchLogStasticsDal;

        }
        public static IActionInfoDal CreateActionInfoDal()
        {
            string fullClassName = NameSpace + ".ActionInfoDal";
            return CreateInstance(fullClassName) as IActionInfoDal;

        }
        public static IBooksDal CreateBooksDal()
        {
            string fullClassName = NameSpace + ".BooksDal";
            return CreateInstance(fullClassName) as IBooksDal;

        }
        public static IDepartmentDal CreateDepartmentDal()
        {
            string fullClassName = NameSpace + ".DepartmentDal";
            return CreateInstance(fullClassName) as IDepartmentDal;

        }

        public static IR_UserInfo_ActionInfoDal CreateR_UserInfo_ActionInfoDal()
        {
            string fullClassName = NameSpace + ".R_UserInfo_ActionInfoDal";
            return CreateInstance(fullClassName) as IR_UserInfo_ActionInfoDal;

        }

        public static IRoleInfoDal CreateRoleInfoDal()
        {
            string fullClassName = NameSpace + ".RoleInfoDal";
            return CreateInstance(fullClassName) as IRoleInfoDal;

        }

        public static IUserInfoDal CreateUserInfoDal()
        {
            string fullClassName = NameSpace + ".UserInfoDal";
            return CreateInstance(fullClassName) as IUserInfoDal;

        }
    }
}
