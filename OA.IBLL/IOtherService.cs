using OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.IBLL
{

    public partial interface IActionInfoService : IBaseService<ActionInfo>
    {
        bool DeleteEntities(List<int> list);//删除多条数据
    }

    public partial interface IDepartmentService : IBaseService<Department>
    {

    }

    public partial interface IT_SearchLogsService : IBaseService<T_SearchLogs>
    {

    }
    public partial interface IT_SearchLogStasticsService : IBaseService<T_SearchLogStastics>
    {
        bool DeleteAllStastics();//删除总统计所有的数据
        bool InsertAllStastics();//重新将明细表数据统计到总汇总统计表中
        List<string> GetMatchWords(string str);//将汇总表中匹配的词语返回
    }
    public partial interface IBooksService : IBaseService<Books>
    {

    }
    public partial interface IR_UserInfo_ActionInfoService : IBaseService<R_UserInfo_ActionInfo>
    {

    }

    public partial interface IRoleInfoService : IBaseService<RoleInfo>
    {
        bool DeleteEntities(List<int> list);//删除多条数据
        bool SetRoleActionInfo(int actionid, int roleid, bool isallow);
    }

    public partial interface IUserInfoService : IBaseService<UserInfo>
    {

    }
}
