using OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.IDAL
{
    public partial interface IT_SearchLogsDal : IBaseDal<T_SearchLogs>
    {

    }
    public partial interface IT_SearchLogStasticsDal : IBaseDal<T_SearchLogStastics>
    {
        int DeleteAllStastics();/// 删除总统计所有的数据
        int InsertAllStastics();// 重新将明细表数据统计到总汇总统计表中
        List<string> GetMatchWords(string str);//将汇总表中匹配的词语返回
    }
    public partial interface IBooksDal : IBaseDal<Books>
    {

    }
    public partial interface IBooksDal : IBaseDal<Books>
    {

    }
    public partial interface IActionInfoDal : IBaseDal<ActionInfo>
    {

    }

    public partial interface IDepartmentDal : IBaseDal<Department>
    {

    }

    public partial interface IR_UserInfo_ActionInfoDal : IBaseDal<R_UserInfo_ActionInfo>
    {

    }

    public partial interface IRoleInfoDal : IBaseDal<RoleInfo>
    {

    }

    public partial interface IUserInfoDal : IBaseDal<UserInfo>
    {

    }
}
