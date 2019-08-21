using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using SkyCar.Coeus.ComModel;

namespace SkyCar.Coeus.DAL
{
    /// <summary>
    /// 处理Model的类
    /// </summary>
    public class MDOperate
    {
        #region 共通方法

        /// <summary>
        /// 获取已赋值的属性信息（属性名、属性类型、属性值）
        /// 根据SQL文语法规则格式化属性值
        /// </summary>
        /// <param name="paramModel">Model对象</param>
        /// <returns></returns>
        public List<ComMDAttribute> GetPropertyListFormatValue(object paramModel)
        {
            object param = paramModel;

            //Sting型
            Type str = typeof(System.String);
            //Int32型
            Type int32 = typeof(System.Int32);
            //Int64型
            Type int64 = typeof(System.Int64);
            //Decimal型
            Type dec = typeof(System.Decimal);
            //DateTime型
            Type dateTime = typeof(System.DateTime);
            //Boolean型
            Type boolean = typeof(System.Boolean);
            //字节数组
            Type byteArray = typeof(System.Byte[]);

            //Int32型
            Type int32N = typeof(System.Nullable<Int32>);
            //Int64型
            Type int64N = typeof(System.Nullable<Int64>);
            //Decimal型
            Type decN = typeof(System.Nullable<Decimal>);
            //DateTime型
            Type dateTimeN = typeof(System.Nullable<DateTime>);
            //Boolean型
            Type booleanN = typeof(System.Nullable<Boolean>);

            //定义Model属性List
            List<ComMDAttribute> resultList = new List<ComMDAttribute>();

            //获取Model类型
            Type t = param.GetType();

            foreach (PropertyInfo pi in t.GetProperties())
            {
                //定义Model属性
                ComMDAttribute ComMDAttribute = new ComMDAttribute();
                //判断属性值是否为空（属性值为空不处理）
                if (pi.GetValue(param, null) != null)
                {
                    //Sting型
                    if (str.Equals(pi.PropertyType))
                    {
                        //把属性值用单引号括起来
                        ComMDAttribute.Value = String.Format("'{0}'", pi.GetValue(param, null));
                    }
                    //int32型
                    else if (int32.Equals(pi.PropertyType))
                    {
                        //属性值保持原样
                        ComMDAttribute.Value = pi.GetValue(param, null).ToString();
                    }
                    //int64型
                    else if (int64.Equals(pi.PropertyType) || int64N.Equals(pi.PropertyType))
                    {
                        //属性值保持原样
                        ComMDAttribute.Value = pi.GetValue(param, null).ToString();
                    }
                    //decimal型
                    else if (dec.Equals(pi.PropertyType) || decN.Equals(pi.PropertyType))
                    {
                        //属性值保持原样
                        ComMDAttribute.Value = pi.GetValue(param, null).ToString();
                    }
                    //DateTime型
                    else if (dateTime.Equals(pi.PropertyType) || dateTimeN.Equals(pi.PropertyType))
                    {
                        //把属性值用单引号括起来
                        ComMDAttribute.Value = String.Format("'{0}'", pi.GetValue(param, null));
                    }
                    //Boolean型
                    else if (boolean.Equals(pi.PropertyType) || booleanN.Equals(pi.PropertyType))
                    {
                        //把属性值用单引号括起来
                        ComMDAttribute.Value = String.Format("'{0}'", pi.GetValue(param, null));
                    }
                    //byte[]
                    else if (byteArray.Equals(pi.PropertyType))
                    {
                        ComMDAttribute.Value = System.Convert.ToBase64String((byte[])pi.GetValue(param, null));
                    }
                    //其他类型
                    else
                    {
                        //属性值保持原样
                        ComMDAttribute.Value = pi.GetValue(param, null);
                    }

                    //属性名
                    ComMDAttribute.Name = pi.Name;
                    //属性类型
                    ComMDAttribute.Type = pi.PropertyType.Name;
                    //把Model属性信息追加到属性List内
                    resultList.Add(ComMDAttribute);
                }
            }


            return resultList;
        }
        /// <summary>
        /// 获取已赋值的属性信息（属性名、属性类型、属性值）
        /// 根据SQL文语法规则格式化属性值
        /// <para>过滤掉翻页相关属性（RecordCount、PageIndex、PageSize）</para>
        /// </summary>
        /// <param name="paramModel">Model对象</param>
        /// <returns></returns>
        public List<ComMDAttribute> GetPropertyListFormatValueFOPageProperty(object paramModel)
        {
            object param = paramModel;

            //Sting型
            Type str = typeof(System.String);
            //Int32型
            Type int32 = typeof(System.Int32);
            //Int64型
            Type int64 = typeof(System.Int64);
            //Decimal型
            Type dec = typeof(System.Decimal);
            //DateTime型
            Type dateTime = typeof(System.DateTime);
            //Boolean型
            Type boolean = typeof(System.Boolean);
            //字节数组
            Type byteArray = typeof(System.Byte[]);

            //Int32型
            Type int32N = typeof(System.Nullable<Int32>);
            //Int64型
            Type int64N = typeof(System.Nullable<Int64>);
            //Decimal型
            Type decN = typeof(System.Nullable<Decimal>);
            //DateTime型
            Type dateTimeN = typeof(System.Nullable<DateTime>);
            //Boolean型
            Type booleanN = typeof(System.Nullable<Boolean>);

            //定义Model属性List
            List<ComMDAttribute> resultList = new List<ComMDAttribute>();

            //获取Model类型
            Type t = param.GetType();

            foreach (PropertyInfo pi in t.GetProperties())
            {
                //过滤掉Model内翻页用的相关属性（RecordCount、PageIndex、PageSize）
                if ("RecordCount".Equals(pi.Name) || "PageIndex".Equals(pi.Name) || "PageSize".Equals(pi.Name))
                {
                    continue;
                }

                //定义Model属性
                ComMDAttribute ComMDAttribute = new ComMDAttribute();
                //判断属性值是否为空（属性值为空不处理）
                if (pi.GetValue(param, null) != null)
                {
                    //Sting型
                    if (str.Equals(pi.PropertyType))
                    {
                        //把属性值用单引号括起来
                        ComMDAttribute.Value = String.Format("'{0}'", pi.GetValue(param, null));
                    }
                    //int32型
                    else if (int32.Equals(pi.PropertyType))
                    {
                        //属性值保持原样
                        ComMDAttribute.Value = pi.GetValue(param, null).ToString();
                    }
                    //int64型
                    else if (int64.Equals(pi.PropertyType) || int64N.Equals(pi.PropertyType))
                    {
                        //属性值保持原样
                        ComMDAttribute.Value = pi.GetValue(param, null).ToString();
                    }
                    //decimal型
                    else if (dec.Equals(pi.PropertyType) || decN.Equals(pi.PropertyType))
                    {
                        //属性值保持原样
                        ComMDAttribute.Value = pi.GetValue(param, null).ToString();
                    }
                    //DateTime型
                    else if (dateTime.Equals(pi.PropertyType) || dateTimeN.Equals(pi.PropertyType))
                    {
                        //把属性值用单引号括起来
                        ComMDAttribute.Value = String.Format("'{0}'", pi.GetValue(param, null));
                    }
                    //Boolean型
                    else if (boolean.Equals(pi.PropertyType) || booleanN.Equals(pi.PropertyType))
                    {
                        //把属性值用单引号括起来
                        ComMDAttribute.Value = String.Format("'{0}'", pi.GetValue(param, null));
                    }
                    //byte[]
                    else if (byteArray.Equals(pi.PropertyType))
                    {
                        ComMDAttribute.Value = System.Convert.ToBase64String((byte[])pi.GetValue(param, null));
                    }
                    //其他类型
                    else
                    {
                        //属性值保持原样
                        ComMDAttribute.Value = pi.GetValue(param, null);
                    }

                    //属性名
                    ComMDAttribute.Name = pi.Name;
                    //属性类型
                    ComMDAttribute.Type = pi.PropertyType.Name;
                    //把Model属性信息追加到属性List内
                    resultList.Add(ComMDAttribute);
                }
            }


            return resultList;
        }
        /// <summary>
        /// 获取已赋值的属性信息（属性名、属性类型、属性值）
        /// </summary>
        /// <param name="paramModel">Model对象</param>
        /// <returns></returns>
        public List<ComMDAttribute> GetPropertyListNoFormatValue(object paramModel)
        {
            object param = paramModel;

            List<ComMDAttribute> resultList = new List<ComMDAttribute>();

            Type t = param.GetType();
            foreach (PropertyInfo pi in t.GetProperties())
            {
                ComMDAttribute ComMDAttribute = new ComMDAttribute();
                if (pi.GetValue(param, null) != null)
                {
                    //属性值
                    ComMDAttribute.Value = pi.GetValue(param, null).ToString();
                    //属性名
                    ComMDAttribute.Name = pi.Name;
                    //属性类型
                    ComMDAttribute.Type = pi.PropertyType.Name;
                    resultList.Add(ComMDAttribute);
                }
            }
            return resultList;
        }

        /// <summary>
        /// 根据Model对象获取主键名
        /// </summary>
        /// <param name="paramModel">Model对象</param>
        /// <returns>主键名</returns>
        public string GetPKName(object paramModel)
        {
            object param = paramModel;

            string result = string.Empty;

            Type t = param.GetType();
            foreach (PropertyInfo pi in t.GetProperties())
            {
                string[] tmp = pi.Name.Split(new char[] { '_' });
                if (DALConst.ID.Equals(tmp[1]))
                {
                    return pi.Name;
                }
                //if (SysConst.ID.Equals(pi.Name))
                //{
                //    return pi.Name;
                //}
                //else
                //{
                //    //string[] tmp = pi.Name.Split(new char[] { '_' });
                //    //if (SysConst.ID.Equals(tmp[1]))
                //    //{
                //    //    return pi.Name;
                //    //}
                //    if (pi.Name.Length > 4 && "ID".Equals(pi.Name.Substring(4))) return pi.Name;
                //}
            }
            return result;
        }
       
        /// <summary>
        /// 基础类型转换（将value的类型转为convertsionType）
        /// </summary>
        /// <param name="value">要转型的对象</param>
        /// <param name="convertsionType">要转的类型（基础基元类型或者可空类型）</param>
        /// <returns>转型后的对象</returns>
        public object ChanageType(object value, Type convertsionType)
        {
            //判断convertsionType类型是否为泛型，因为nullable是泛型类,
            if (convertsionType.IsGenericType &&
                //判断convertsionType是否为nullable泛型类
                convertsionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null || value.ToString().Length == 0)
                {
                    return null;
                }

                //如果convertsionType为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                NullableConverter nullableConverter = new NullableConverter(convertsionType);
                //将convertsionType转换为nullable对的基础基元类型
                convertsionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, convertsionType);
        }
        #endregion
    }
}
