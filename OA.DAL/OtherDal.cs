using OA.IDAL;
using OA.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.DAL
{
    public partial class T_SearchLogsDal : BaseDal<T_SearchLogs>, IT_SearchLogsDal
    {

    }
    public partial class T_SearchLogStasticsDal : BaseDal<T_SearchLogStastics>, IT_SearchLogStasticsDal
    {
        /// <summary>
        /// 删除总统计所有的数据
        /// </summary>
        /// <returns></returns>
        public int DeleteAllStastics()
        {
            string sql = "truncate table T_SearchLogStastics";
            return Db.Database.ExecuteSqlCommand(sql);
        }
        /// <summary>
        /// 将汇总表中匹配的词语返回
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public List<string> GetMatchWords(string str)
        {
            string sql = "select top 4 word from T_SearchLogStastics where word like @item order by searchcount desc";
            return Db.Database.SqlQuery<string>(sql, new SqlParameter("@item", str + "%")).ToList();
        }

        /// <summary>
        /// 重新将明细表数据统计到总汇总统计表中
        /// </summary>
        /// <returns></returns>
        public int InsertAllStastics()
        {
            string sql = "insert into T_SearchLogStastics(Word,SearchCount) select Word,count(*)  from T_SearchLogs where DateDiff(day,T_SearchLogs.SearchDate,getdate())<=7 group by T_SearchLogs.Word";
            return Db.Database.ExecuteSqlCommand(sql);
        }

    }
    public partial class ActionInfoDal : BaseDal<ActionInfo>, IActionInfoDal
    {

    }
    public partial class BooksDal : BaseDal<Books>, IBooksDal
    {

    }
    public partial class DepartmentDal : BaseDal<Department>, IDepartmentDal
    {

    }

    public partial class R_UserInfo_ActionInfoDal : BaseDal<R_UserInfo_ActionInfo>, IR_UserInfo_ActionInfoDal
    {

    }

    public partial class RoleInfoDal : BaseDal<RoleInfo>, IRoleInfoDal
    {

    }

    public partial class UserInfoDal : BaseDal<UserInfo>, IUserInfoDal
    {

    }
}
