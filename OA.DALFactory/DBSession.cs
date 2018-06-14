using OA.DAL;
using OA.IDAL;
using OA.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.DALFactory
{
    /// <summary>
    /// 数据会话层：就是一个工厂类，负责完成所有数据操作类实例的创建，然后
    /// 业务层通过数据会话层来获取操作类的实例，所以数据会话层将业务层与数据层解耦
    /// 目的：解决当操作多张表逻辑的时候，仅仅操作一次数据库，也就是实现工作单元模式
    /// </summary>
    public partial class DBSession:IDBSession
    {
       // OAEntities Db = new OAEntities();
       public DbContext Db
        {
            get
            {
                return DBContextFactory.CreateDbContext();
            }
        }
        //private IUserInfoDal _userInfoDal;
        //public IUserInfoDal UserInfoDal
        //{
        //    get
        //    {
        //        if (_userInfoDal == null)
        //        {
        //           /* _userInfoDal = new UserInfoDal()*/;
        //            _userInfoDal = AbstractFactory.CreateUserInfoDal();//通过抽象工厂封装了类的实例的创建
        //        }
        //        return _userInfoDal;
        //    }
        //    set
        //    {
        //        _userInfoDal = value;
        //    }
        //}
        /// <summary>
        /// 一个业务中经常涉及到对多张表的操作，我们希望链接一次数据库来完成
        /// 多张表的操作，为了提高性能。工作单元模式
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            return Db.SaveChanges() > 0;
        }
    }
}
