using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Peacock.PEP.Data.Entities;
using System.Collections;

namespace Peacock.PEP.Service.Extensions
{
    public static class ListPowerExtension
    {
        private static UserService _userService = UserService.Instance;


        /// <summary>
        /// 根据权限查询数据
        /// </summary>
        public static IQueryable<TResult> FindByPower<TResult>(this IQueryable<TResult> query)
        {          
            BinaryExpression body = null;
            ParameterExpression p = Expression.Parameter(typeof(TResult), "p");
            var user = _userService.GetUser();
            //有绑定公司才查询公司的数据
            var departmentList = _userService.GetDepartmentsByCompany(null, user.UserAccount);
            PropertyInfo[] propertys = typeof(TResult).GetProperties();
            if (departmentList.Any())
            {
                //当公司为非个人用户组时才按公司数据权限查询
                var company = ConfigurationManager.AppSettings["PersonalUserCompany"];
                if (company == null)
                {
                    throw new Exception("请在AppSettings配置个人用户公司名称节点");
                }
                //当列表为线上业务列表时只查询公司的数据
                if (departmentList.FirstOrDefault().CompanyName != company)
                {
                    var departmentIdList = departmentList.Select(x => x.Id).ToList();
                    for (int i = 0, len = departmentIdList.Count; i < len; i++)
                    {
                        body = body.CreateOrExpression<TResult>("DepartmentId", departmentIdList[i], p);
                    }
                    //query = query.CreateQuery("DepartmentId", departmentIdList, ExpressionCallType.In);
                }
            }
            //查询自己创建的数据
            if (typeof(TResult).Name != typeof(OnLineBusiness).Name)
            {
                var creatorId = propertys.FirstOrDefault(x => x.Name.Contains("CreatorId")).Name;
                body = body.CreateOrExpression<TResult>(creatorId, user.Id, p);
                //query = query.Union(creatorId, _userService.GetUser().Id, ExpressionType.Equal);
            }
            if (body != null)
            {
                var lambda = Expression.Lambda<Func<TResult, bool>>(body, p);
                query = query.Where(lambda);
            }

            //project查询非取消的数据
            if (typeof(TResult).Name == typeof(Project).Name)
            {
                var isCancle = propertys.FirstOrDefault(x => x.Name.Contains("IsCancle")).Name;
                query = query.CreateQuery(isCancle, false, ExpressionType.Equal);
            }

            return query;
        }
    }

    /// <summary>
    /// 查询逻辑枚举
    /// </summary>
    public enum ExpressionCallType
    {
        In
    }

    public static class LambdaCreator
    {
        /// <summary>
        /// 储存对应方法的容器
        /// </summary>
        private static Dictionary<ExpressionCallType, MethodInfo> _MethodCondition = new Dictionary<ExpressionCallType, MethodInfo>()
        {
            { ExpressionCallType.In, typeof(string).GetMethods().FirstOrDefault(x => x.Name == "Contains") }
        };

        /// <summary>
        /// 动态创建查询条件
        /// </summary>
        public static IQueryable<TEntity> CreateQuery<TEntity>(this IQueryable<TEntity> query, string searchProperty, object value, ExpressionType expressType)
        {
            var p = Expression.Parameter(typeof(TEntity), "p");
            var expressionProperty = Expression.Property(p, searchProperty);
            var expressionConstant = Expression.Constant(value);
            var expressionBinary = BinaryExpression.MakeBinary(expressType, expressionProperty, expressionConstant);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(expressionBinary, p);
            return query.Where(lambda);
        }


        /// <summary>
        /// 动态创建查询条件
        /// </summary>
        public static IQueryable<TEntity> CreateQuery<TEntity, TListEntity>(this IQueryable<TEntity> query, string searchProperty, IEnumerable<TListEntity> objs, ExpressionCallType expressType)
        {
            var len = objs.Count();
            var p = Expression.Parameter(typeof(TEntity), "p");
            var expressionProperty = Expression.Property(p, searchProperty);
            var expressionConstant = Expression.Constant(objs.First());
            var body = BinaryExpression.MakeBinary(ExpressionType.Equal, expressionProperty, expressionConstant);

            for (int i = 1; i < len; i++)
            {
                var pcode = objs.ElementAt(i);
                body = Expression.Or(body, BinaryExpression.MakeBinary(ExpressionType.Equal, expressionProperty, Expression.Constant(pcode)));
            }

            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, p);
            return query.Where(lambda);
        }


        public static BinaryExpression CreateOrExpression<TEntity>(this BinaryExpression body, string searchProperty, object value,ParameterExpression p)
        {
            //var p = Expression.Parameter(typeof(TEntity), "p");
            var expressionProperty = Expression.Property(p, searchProperty);
            var expressionConstant = Expression.Constant(value);
            if(body ==null)
            {
                body = BinaryExpression.MakeBinary(ExpressionType.Equal, expressionProperty, expressionConstant);
            }
            else
                body = Expression.Or(body, BinaryExpression.MakeBinary(ExpressionType.Equal, expressionProperty, expressionConstant)); 
            return body;
        }
     

    }
}
