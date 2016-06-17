using System.Data.Entity.Core.Objects;
using Peacock.PEP.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Peacock.PEP.Repository
{
    public abstract class Repository<TEntity, TSing> where TEntity : class
    {
        private static readonly Lazy<TSing> _instance = new Lazy<TSing>(() =>
        {
            if (!typeof(TSing).IsSealed)
                throw new Exception(String.Format("仓储{0}必须声明为sealed！", typeof(TSing)));
            var ctors = typeof(TSing).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (ctors.Count() != 1)
                throw new Exception(String.Format("类型{0}必须有构造函数！", typeof(TSing)));
            var ctor = ctors.SingleOrDefault(c => c.GetParameters().Count() == 0 && c.IsPrivate);
            if (ctor == null)
                throw new Exception(String.Format("{0}必须有私有且无参的构造函数", typeof(TSing)));
            return (TSing)ctor.Invoke(null);
        });
        public static TSing Instance
        {
            get { return _instance.Value; }
        }

        private static readonly string _readKey = "_PepDbContextKey";
        private static Collection CoreContext
        {
            get
            {
                Collection collection = CallContext.GetData(_readKey) as Collection;

                //SQL语句拦截器
                //System.Data.Entity.Infrastructure.Interception.DbInterception.Add(new EFSqlInterceptor());

                if (collection == null)
                {
                    collection = new Collection(string.Format("{0}", ConfigurationManager.ConnectionStrings["PepReadConnection"]));
                    CallContext.SetData(_readKey, collection);
                }
                return collection;
            }
        }

        public virtual IQueryable<TEntity> Source
        {
            get { return CoreContext.Set<TEntity>(); }
        }

        public virtual IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            return CoreContext.Set<TEntity>().Where(expression);
        }

        public virtual int Count(Expression<Func<TEntity, bool>> expression)
        {
            return CoreContext.Set<TEntity>().Count(expression);
        }

        public virtual IQueryable<TEntity> FindForPaging(int size, int index, Expression<Func<TEntity, bool>> expression, out int total)
        {
            return FindForPaging(size, index, this.Find(expression), out total);
        }

        public virtual IQueryable<TEntity> FindForPaging(int size, int index, IQueryable<TEntity> source, out int total)
        {
            if (index <= 0)
                index = 1;
            var temp = source.Skip((index - 1) * size).Take(size);
            total = source.Count();
            return temp;
        }

        public virtual bool Insert(TEntity entity)
        {
            if (entity == null)
                throw new Exception("新增对象不能为空");
            CoreContext.Set<TEntity>().Add(entity);
            return CoreContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 插入并返回最新对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity InsertReturnEntity(TEntity entity)
        {
            if (entity == null)
                throw new Exception("新增对象不能为空");
            CoreContext.Set<TEntity>().Add(entity);
            CoreContext.SaveChanges();
            return entity;
        }

        public virtual bool Insert(IEnumerable<TEntity> batch)
        {
            CoreContext.Set<TEntity>().AddRange(batch);
            return CoreContext.SaveChanges() > 0;
        }

        public virtual bool Save(TEntity entity)
        {
            if (entity == null)
                throw new Exception("保存对象不能为空");
            CoreContext.Entry<TEntity>(entity).State = EntityState.Modified;
            return CoreContext.SaveChanges() > 0;
        }

        public virtual bool Delete(TEntity entity)
        {
            if (entity == null)
                throw new Exception("删除对象不能为空");
            CoreContext.Set<TEntity>().Remove(entity);
            return CoreContext.SaveChanges() > 0;
        }

        public virtual bool Delete(IEnumerable<TEntity> batch)
        {
            CoreContext.Set<TEntity>().RemoveRange(batch);
            return CoreContext.SaveChanges() > 0;
        }

        public virtual bool Delete(Expression<Func<TEntity, bool>> expression)
        {
            return this.Delete(this.Find(expression));
        }

        /// <summary>
        /// 执行查询类SQL语句
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="paramers"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> ExecuteSql<TEntity>(string sql, params object[] paramers)
        {
            return CoreContext.Database.SqlQuery<TEntity>(sql, paramers).AsQueryable();
        }

        /// <summary>
        /// 执行查询类存储过程
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="funcName"></param>
        /// <param name="paramers"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> ExecuteFunction<TEntity>(string funcName, params  ObjectParameter[] paramers)
        {
            return ((IObjectContextAdapter)CoreContext).ObjectContext
                .ExecuteFunction<TEntity>(funcName, paramers).AsQueryable();
        }

        /// <summary>
        /// 执行查询类存储过程
        /// </summary>
        /// <param name="funcName"></param>
        /// <param name="paramers"></param>
        /// <returns></returns>
        public virtual DataTable ExecuteFunction(string funcName, params DbParameter[] paramers)
        {
            try
            {
                var conn = (MySql.Data.MySqlClient.MySqlConnection)CoreContext.Database.Connection;
                MySql.Data.MySqlClient.MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = funcName;
                cmd.Parameters.Clear();
                foreach (var paramer in paramers)
                {
                    cmd.Parameters.Add(paramer as MySql.Data.MySqlClient.MySqlParameter);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                
                DataTable dt = new DataTable(); 
                var da = new MySql.Data.MySqlClient.MySqlDataAdapter(selectCommand: cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void Transaction(Action action, int timeout = 5)
        {
            TransactionOptions transactionOption = new TransactionOptions();
            // 设置事务超时时间为一小时
            transactionOption.Timeout = new TimeSpan(0, timeout, 0);
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionOption))
            {
                try
                {
                    action();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    scope.Dispose();
                }
            };
        }
    }
}