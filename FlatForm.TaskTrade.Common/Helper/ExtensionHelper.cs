using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Peacock.Common.Helper
{
    public static class ExtensionHelper
    {
        /// <summary>
        /// 检查 string 是否不为 null 或 empty
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 比较字符串是否一致，不区分大小写
        /// </summary>
        /// <param name="str"></param>
        /// <param name="str1"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string str, string str1)
        {
            return string.Equals(str, str1, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 转换为价格的字符串格式 
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public static string ToMoneyString(this decimal money)
        {
            return money.ToString("F2");
        }

        public static string ToIntString(this decimal money)
        {
            return money.ToString("D");
        }

        /// <summary>
        /// 转换为日期字符串
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDateString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }

    /// <summary>
    /// json对象扩展
    /// </summary>
    public static class ObjectExtension
    {
        public static string ToJson(this object o, params JsonConverter[] converters)
        {
            if (o is string)
            {
                return o + "";
            }
            var d = o as IXSerializable;
            if (d == null) return JsonConvert.SerializeObject(o, converters);
            var s = d.Serialize();
            return !s.NotValid() ? s : JsonConvert.SerializeObject(o, converters);
        }

        public static object FromJson(this string json, Type type = null, params JsonConverter[] converters)
        {
            if (type == typeof(string))
            {
                return json;
            }
            return type == null
                ? JsonConvert.DeserializeObject(json)
                : JsonConvert.DeserializeObject(json, type, converters);
        }

        public static T FromJson<T>(this string json, params JsonConverter[] converters)
        {
            if (typeof(T) != typeof(string))
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, converters);
            object o = json;
            return (T)o;
        }
    }

    public static class ValidExtensions
    {
        public static bool NotValid(this object obj)
        {
            return obj == null || (obj + "").Length == 0;
        }
        public static bool NotValid(this ICollection collection)
        {
            return collection == null || collection.Count == 0;
        }
        public static bool NotValid(this Guid obj)
        {
            return obj != Guid.Empty;
        }
        public static bool NotValid(this IEnumerable enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }
            var en = enumerable.GetEnumerator();
            bool r = en.MoveNext();

            en = null;
            return !r;
        }
    }

    public interface IXSerializable
    {
        /// <summary>
        /// 序列化自身
        /// </summary>
        /// <returns></returns>
        string Serialize();

        /// <summary>
        /// 反序列化自身
        /// </summary>
        /// <param name="serialStr"></param>
        /// <param name="refresh"></param>
        /// <returns>this</returns>
        IXSerializable DeSerialize(string serialStr, bool refresh = false);
    }

    public static class StringHelper
    {
        /// <summary>
        /// 获取随机验证码
        /// </summary>
        /// <returns></returns>
        public static string GetSecurityCode()
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 6; i++)
            {
                builder.Append(random.Next(10));
            }
            return builder.ToString();
        }
    }                
}
