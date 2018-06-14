using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Model.Enum
{
   public enum DeleteEnumType
    {
        Normal=0,
        IsDeleted=1
    }
    /// <summary>
    /// 用户权限的分类
    /// </summary>
    public enum ActionTypeEnum
    {
        Normal = 0,// 非菜单权限
        Menu =1//菜单权限
    }

}
