using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Model
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
   
    using System.ComponentModel.DataAnnotations;


    public partial class UserInfo
    {
        public UserInfo()
        {
            this.R_UserInfo_ActionInfo = new HashSet<R_UserInfo_ActionInfo>();
            this.Department = new HashSet<Department>();
            this.RoleInfo = new HashSet<RoleInfo>();
        }
        [Key]
        public int ID { get; set; }
        public string UName { get; set; }
        public string UPwd { get; set; }
        public System.DateTime SubTime { get; set; }
        public short DelFlag { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public string Remark { get; set; }
        public string Sort { get; set; }

        [JsonIgnore]
        public virtual ICollection<R_UserInfo_ActionInfo> R_UserInfo_ActionInfo { get; set; }
        [JsonIgnore]
        public virtual ICollection<Department> Department { get; set; }
        [JsonIgnore]
        public virtual ICollection<RoleInfo> RoleInfo { get; set; }
    }
}
