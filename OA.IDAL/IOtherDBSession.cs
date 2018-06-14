using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.IDAL
{
    public partial interface IDBSession
    {
        IActionInfoDal ActionInfoDal { get; set; }

        IDepartmentDal DepartmentDal { get; set; }

        IR_UserInfo_ActionInfoDal R_UserInfo_ActionInfoDal { get; set; }

        IRoleInfoDal RoleInfoDal { get; set; }

        IUserInfoDal UserInfoDal { get; set; }
        IT_SearchLogsDal T_SearchLogsDal { get; set; }
        IT_SearchLogStasticsDal T_SearchLogStasticsDal { get; set; }
        IBooksDal BooksDal { get; set; }
    }
}
