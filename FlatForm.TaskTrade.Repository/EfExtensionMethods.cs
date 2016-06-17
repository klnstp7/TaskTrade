using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Peacock.PEP.Repository
{
    public static class EfExtensionMethods
    {
        public static IQueryable<TInput> Include<TInput>(this IQueryable<TInput> query, params Expression<Func<TInput, object>>[] IncludeParas)
        {
            List<string> IncludeList = new List<string>();
            foreach (var item in IncludeParas)
            {
                var property = item.Body as MemberExpression;
                if (property == null)
                    throw new Exception(string.Format("无效的表达式({0})!", item.Body.ToString()));
                if (!property.Type.IsClass)
                    throw new Exception(string.Format("类型{0}必须为引用类型", property.Type.Name));
                IncludeList.Add(property.Member.Name);
            }
            query = QueryableExtensions.Include(query, string.Join(",", IncludeList));
            return query;
        }
    }
}
