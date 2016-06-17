using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Data
{
    public class EntityConfig<T> : EntityTypeConfiguration<T> where T : class
    {
        /// <summary>
        /// （构造函数）默认将所有string类型长度设为500
        /// </summary>
        internal EntityConfig()
            : this(500)
        {

        }

        /// <summary>
        /// （构造函数）设置string类型长度
        /// </summary>
        /// <param name="DefaultStringLength">限制所有字符串长度</param>
        internal EntityConfig(int DefaultStringLength)
        {
            this.DefaultStringLength = DefaultStringLength;
            Init();
        }

        private void Init()
        {
            Type type = typeof(T);
            var stringProperties = type.GetProperties().Where(x => x.PropertyType == typeof(string));
            foreach (var item in stringProperties)
            {
                var parameter = Expression.Parameter(type, "x");
                var property = Expression.MakeMemberAccess(parameter, item);
                Expression<Func<T, string>> lambda = Expression.Lambda<Func<T, string>>(property, parameter);
                base.Property(lambda).HasMaxLength(this.DefaultStringLength);
            }
            base.ToTable(type.Name);

        }
        private int DefaultStringLength { get; set; }

    }
}
