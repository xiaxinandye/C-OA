using OA.DALFactory;
using OA.IBLL;
using OA.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OA.BLL
{
    public abstract class BaseService<T> :IBaseService<T> where T : class, new()
    {
        public IDBSession CurrentDBSession
        {
            get
            {
                return  DBSessionFactory.CreateDBSession();
            }
        }
        public IBaseDal<T> CurrentDal { get; set; }
        public abstract void SetCurrentDal();
        public BaseService()
        {
            SetCurrentDal();
        }
        public IQueryable<T> LoadEntities
            (Expression<Func<T,bool>> whereLambda)
        {
            return CurrentDal.LoadEntities(whereLambda);
        }
        public IQueryable<T> LoadPageEntities<s>
          (int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda,
          Expression<Func<T, s>> orderbyLambda, bool isAsc)
        {
            return CurrentDal.LoadPageEntities<s>(pageIndex, pageSize, out totalCount, whereLambda, orderbyLambda, isAsc);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
       public bool DeleteEntity(T entity)
        {
            CurrentDal.DeleteEntity(entity);
            return CurrentDBSession.SaveChanges();
        }
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool EditEntity(T entity)
        {
            CurrentDal.EditEntity(entity);
            return CurrentDBSession.SaveChanges();
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T AddEntity(T entity)
        {
            CurrentDal.AddEntity(entity);
            CurrentDBSession.SaveChanges();
            return entity;
        }
    }
}
