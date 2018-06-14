using OA.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.DALFactory
{
    public partial class DBSession : IDBSession
    {

        private IActionInfoDal _ActionInfoDal;
        public IActionInfoDal ActionInfoDal
        {
            get
            {
                if (_ActionInfoDal == null)
                {
                    _ActionInfoDal = AbstractFactory.CreateActionInfoDal();
                }
                return _ActionInfoDal;
            }
            set { _ActionInfoDal = value; }
        }
        private IT_SearchLogsDal _T_SearchLogsDal;
        public IT_SearchLogsDal T_SearchLogsDal
        {
            get
            {
                if (_T_SearchLogsDal == null)
                {
                    _T_SearchLogsDal = AbstractFactory.CreateT_SearchLogsDal();
                }
                return _T_SearchLogsDal;
            }
            set { _T_SearchLogsDal = value; }
        }
        private IT_SearchLogStasticsDal _T_SearchLogStasticsDal;
        public IT_SearchLogStasticsDal T_SearchLogStasticsDal
        {
            get
            {
                if (_T_SearchLogStasticsDal == null)
                {
                    _T_SearchLogStasticsDal = AbstractFactory.CreateT_SearchLogStasticsDal();
                }
                return _T_SearchLogStasticsDal;
            }
            set { _T_SearchLogStasticsDal = value; }
        }

        private IBooksDal _BooksDal;
        public IBooksDal BooksDal
        {
            get
            {
                if (_BooksDal == null)
                {
                    _BooksDal = AbstractFactory.CreateBooksDal();
                }
                return _BooksDal;
            }
            set { _BooksDal = value; }
        }
        private IDepartmentDal _DepartmentDal;
        public IDepartmentDal DepartmentDal
        {
            get
            {
                if (_DepartmentDal == null)
                {
                    _DepartmentDal = AbstractFactory.CreateDepartmentDal();
                }
                return _DepartmentDal;
            }
            set { _DepartmentDal = value; }
        }

        private IR_UserInfo_ActionInfoDal _R_UserInfo_ActionInfoDal;
        public IR_UserInfo_ActionInfoDal R_UserInfo_ActionInfoDal
        {
            get
            {
                if (_R_UserInfo_ActionInfoDal == null)
                {
                    _R_UserInfo_ActionInfoDal = AbstractFactory.CreateR_UserInfo_ActionInfoDal();
                }
                return _R_UserInfo_ActionInfoDal;
            }
            set { _R_UserInfo_ActionInfoDal = value; }
        }

        private IRoleInfoDal _RoleInfoDal;
        public IRoleInfoDal RoleInfoDal
        {
            get
            {
                if (_RoleInfoDal == null)
                {
                    _RoleInfoDal = AbstractFactory.CreateRoleInfoDal();
                }
                return _RoleInfoDal;
            }
            set { _RoleInfoDal = value; }
        }

        private IUserInfoDal _UserInfoDal;
        public IUserInfoDal UserInfoDal
        {
            get
            {
                if (_UserInfoDal == null)
                {
                    _UserInfoDal = AbstractFactory.CreateUserInfoDal();
                }
                return _UserInfoDal;
            }
            set { _UserInfoDal = value; }
        }
    }
}
