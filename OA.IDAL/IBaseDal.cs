using OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OA.IDAL
{
    public interface IBaseDal<T>where T:class,new()
    {
        /// <summary>
        /// 根据lambda表达式查询数据
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        IQueryable<T> LoadEntities
        (Expression<Func<T, bool>> whereLambda);
        /// <summary>
        /// 根据lambda表达式分页查询数据
        /// </summary>
        /// <typeparam name="s"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderbyLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        IQueryable<T> LoadPageEntities<s>
            (int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda,
            Expression<Func<T, s>> orderbyLambda, bool isAsc);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool DeleteEntity(T entity);
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool EditEntity(T entity);
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T AddEntity(T entity);
    }

}
