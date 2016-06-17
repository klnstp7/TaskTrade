using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.ApiModel
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class CheckFieldValidateAttribute : Attribute
    {
        bool m_isCheckNull;
        bool m_isCheckNum;
        object m_DefaultValue = string.Empty;
        string m_Desc = string.Empty;
        int m_maxLenth = 50;
        string m_enumStr = string.Empty;
        public CheckFieldValidateAttribute(bool isCheckNull, string desc, object DefaultValue = null, int maxLength = 50, string enumStr = null, bool isCheckNum = false)
        {
            m_isCheckNull = isCheckNull;
            m_isCheckNum = isCheckNum;
            m_DefaultValue = DefaultValue;
            m_Desc = desc;
            m_maxLenth = maxLength;
            //m_deserializeObject =deserializeObject;
        }
        //public Type DeserializeObject
        //{
        //    get
        //    {
        //        return m_deserializeObject;
        //    }
        //    set
        //    {
        //        m_deserializeObject = value;
        //    }
        //}
        public string EnumStr
        {
            get
            {
                return m_enumStr;
            }
            set
            {
                m_enumStr = value;
            }
        }
        public int MaxLength
        {
            get
            {
                return m_maxLenth;
            }
            set
            {
                m_maxLenth = value;
            }
        }
        public string Desc
        {
            get
            {
                return m_Desc;
            }
            set
            {
                m_Desc = value;
            }
        }
        // 获取该成员映射的数据库字段名称。
        public bool IsCheckNull
        {
            get
            {
                return m_isCheckNull;
            }
            set
            {
                m_isCheckNull = value;
            }
        }

        public bool isCheckNum
        {
            get
            {
                return m_isCheckNum;
            }
            set
            {
                m_isCheckNum = value;
            }
        }

        // 获取该字段的默认值
        public object DefaultValue
        {
            get
            {
                return m_DefaultValue;
            }
            set
            {
                m_DefaultValue = value;
            }
        }
    }

    public class ValidateModel
    {
        /// <summary>
        /// 验证模型中的数据是否合法
        /// </summary>
        /// <param name="Model"></param>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public static bool ValidateEntityData<T>(T Model, out string SendMessage) where T : class
        {
            SendMessage = string.Empty;
            long trylong = 0;
            foreach (PropertyInfo proInfo in Model.GetType().GetProperties())
            {
                object[] attrs = proInfo.GetCustomAttributes(typeof(CheckFieldValidateAttribute), true);
                if (attrs.Length == 1)
                {
                    CheckFieldValidateAttribute att = (CheckFieldValidateAttribute)attrs[0];
                    var ProptotyVal = proInfo.GetValue(Model, null) == null ? string.Empty : proInfo.GetValue(Model, null).ToString();
                    if (att.IsCheckNull && string.IsNullOrEmpty(ProptotyVal))
                    {
                        if (att.DefaultValue!=null && !string.IsNullOrEmpty(att.DefaultValue.ToString()))
                        {
                            proInfo.SetValue(Model, att.DefaultValue);
                        }
                        else
                        {
                            SendMessage = string.Format("必填字段[{1}:{0}]值为空", proInfo.Name, att.Desc);
                            break;
                        }
                    }//验证必填项
                    else if (!string.IsNullOrEmpty(ProptotyVal) && ProptotyVal.Length > att.MaxLength)
                    {
                        SendMessage = string.Format("字段[{2}:{0}]长度超过指定长度{1}", proInfo.Name, att.MaxLength, att.Desc);
                        break;
                    }//验证字符长度
                    else if (!string.IsNullOrEmpty(ProptotyVal) && !string.IsNullOrEmpty(att.EnumStr) && !att.EnumStr.Split(',').Contains(ProptotyVal))
                    {
                        SendMessage = string.Format("字段[{3}:{0}]提供的枚举值\"{1}\"不在合法范围:{2}", proInfo.Name, ProptotyVal, att.EnumStr, att.Desc);
                        break;
                    }//验证枚举值
                    else if (!string.IsNullOrEmpty(ProptotyVal) && att.isCheckNum && !long.TryParse(ProptotyVal, out trylong))
                    {
                        SendMessage = string.Format("数值字段[{2}:{0}]提供的值\"{1}\"不是数字", proInfo.Name, ProptotyVal, att.Desc);
                        break;
                    }//验证数字
                }
            }
            if (!string.IsNullOrEmpty(SendMessage))
                return false;
            return true;
        }
    }
}
