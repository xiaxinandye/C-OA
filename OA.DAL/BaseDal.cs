using OA.IDAL;
using OA.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OA.DAL
{
    public class BaseDal<T>:IBaseDal<T> where T:class,new()
    {
        //  DbContext db = DBContextFactory.CreateDbContext();
        public DbContext Db
        {
            get
            {
                return DBContextFactory.CreateDbContext();
            }
        }
        public T AddEntity(T entity)
        {
            Db.Set<T>().Add(entity);
           // db.SaveChanges();
            return entity;
        }

        public bool DeleteEntity(T entity)
        {
            Db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
            return true;
        }

        public bool EditEntity(T entity)
        {
            Db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            return true;
        }

        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return Db.Set<T>().Where(whereLambda);
        }

        public IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, s>> orderbyLambda, bool isAsc)
        {
            var temp = Db.Set<T>().Where(whereLambda);
            totalCount = temp.Count();
            temp = isAsc == true ?
                temp.OrderBy(orderbyLambda).Skip(pageSize * (pageIndex - 1)).Take(pageSize) :
                 temp.OrderByDescending(orderbyLambda).Skip(pageSize * (pageIndex - 1)).Take(pageSize)
                ;
            return temp;
        }
    }
}
