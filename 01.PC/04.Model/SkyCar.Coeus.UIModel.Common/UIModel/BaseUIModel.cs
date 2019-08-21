using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using SkyCar.Coeus.Common.ExtendClass.Interface;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// BaseUIModel
    /// </summary>
    public class BaseUIModel : IPropertyChanged
    {
        public bool PropertyValueChanged { get; set; }

        /// <summary>
        /// 总记录条数（翻页用）
        /// </summary>
        public Int32? RecordCount { get; set; }

        /// <summary>
        /// 将当前对象转换为指定类型的对象
        /// <para>将属性名和类型相同的属性进行赋值</para>
        /// </summary>
        /// <typeparam name="TModel">结果Model类型</typeparam>
        /// <returns>结果Model</returns>
        private TModel ToModel<TModel>()
        {
            var resultObj = System.Activator.CreateInstance<TModel>();
            try
            {
                Type tp = this.GetType();
                foreach (PropertyInfo pi in tp.GetProperties())
                {
                    //过滤索引器
                    if ("Item".Equals(pi.Name)) continue;

                    if (pi.GetValue(this, null) != null)
                    {
                        foreach (PropertyInfo piT in resultObj.GetType().GetProperties())
                        {
                            //过滤索引器
                            if ("Item".Equals(piT.Name)) continue;

                            //判断属性名和属性类型是否相等，且可写
                            if (piT.Name == pi.Name && piT.PropertyType.Name == pi.PropertyType.Name && piT.CanWrite)
                            {
                                piT.SetValue(resultObj, pi.GetValue(this, null), null);
                                break;
                            }
                        }
                    }
                }
                return resultObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将当前对象转换为指定类型的TBModel对象
        /// <para>将属性名和类型相同的属性进行赋值，但当前对象的[XXX_ID]和[XXX_VersionNo]会赋值给结果对象的[WHERE_XxX_ID]和[WHERE_XXX_VersionNo]</para>
        /// </summary>
        /// <typeparam name="TBModel">结果TBModel类型</typeparam>
        /// <returns>结果TBModel</returns>
        public TBModel ToTBModelForSaveAndDelete<TBModel>()
        {
            var resultObj = System.Activator.CreateInstance<TBModel>();
            if (!typeof(TBModel).FullName.Contains("TBModel"))
            {
                throw new Exception("类型[TBModel]必须是TBModel！");
            }
            try
            {
                Type tp = this.GetType();
                foreach (var pi in tp.GetProperties())
                {
                    //过滤索引器
                    if ("Item".Equals(pi.Name)) continue;
                    //过滤空值
                    if (pi.GetValue(this, null) == null) continue;

                    //是否是[主键]
                    var isPk = false;
                    //是否是[版本号]
                    var isVersion = false;

                    string[] tmp = pi.Name.Split(new char[] { Convert.ToChar("_") });
                    //是主键的场合
                    if (tmp.Length == 2 && "ID".Equals(tmp[1]))
                    {
                        isPk = true;
                    }
                    //是版本号的场合
                    else if (tmp.Length == 2 && "VersionNo".Equals(tmp[1]))
                    {
                        isVersion = true;
                    }
                    else
                    {
                        isPk = false;
                        isVersion = false;
                    }

                    if (pi.GetValue(this, null) != null)
                    {
                        foreach (PropertyInfo piT in resultObj.GetType().GetProperties())
                        {
                            //过滤索引器
                            if ("Item".Equals(piT.Name)) continue;

                            if (isPk)
                            {
                                //判断属性名和属性类型是否相等，且可写
                                if (piT.Name == "WHERE_" + pi.Name && piT.PropertyType.Name == pi.PropertyType.Name &&
                                    piT.CanWrite)
                                {
                                    piT.SetValue(resultObj, pi.GetValue(this, null), null);
                                    break;
                                }
                            }
                            else if (isVersion)
                            {
                                //判断属性名和属性类型是否相等，且可写
                                if (piT.Name == "WHERE_" + pi.Name && piT.PropertyType.Name == pi.PropertyType.Name &&
                                    piT.CanWrite)
                                {
                                    piT.SetValue(resultObj, pi.GetValue(this, null), null);
                                    break;
                                }
                            }
                            else
                            {
                                //判断属性名和属性类型是否相等，且可写
                                if (piT.Name == pi.Name && piT.PropertyType.Name == pi.PropertyType.Name && piT.CanWrite)
                                {
                                    piT.SetValue(resultObj, pi.GetValue(this, null), null);
                                    break;
                                }
                            }
                        }
                    }
                }
                return resultObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
