using OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.IBLL
{
    public partial interface IUserInfoService:IBaseService<UserInfo>
    {
        bool DeleteEntities(List<int> list);
        bool SetUserRoleInfo(int userid, int roleid, bool isallow);
    }

    
}
