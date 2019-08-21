using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// BaseNotificationUIModel
    /// <para>可监控内容变化的UIModel</para>
    /// </summary>
    public class BaseNotificationUIModel : NotificationObject
    {
        #region 私有变量

        /// <summary>
        /// 是否正在监控当前对象的变化
        /// </summary>
        private bool _isMonitChanges = false;
        /// <summary>
        /// 初始数据
        /// </summary>
        private Dictionary<string, object> _initialData = new Dictionary<string, object>();

        #endregion

        #region 私有方法

        /// <summary>
        /// 属性值变化的事件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected override void RaisePropertyChanged(string propertyName)
        {
            try
            {
                base.RaisePropertyChanged(propertyName);
                //未监控的场合，将当前变动的值保存到[初始数据]
                if (!_isMonitChanges)
                {
                    var tp = this.GetType();
                    foreach (var pi in tp.GetProperties().Where(pi => propertyName == pi.Name))
                    {
                        if (_initialData.ContainsKey(pi.Name))
                        {
                            _initialData[pi.Name] = pi.GetValue(this, null);
                        }
                        else
                        {
                            _initialData.Add(pi.Name, pi.GetValue(this, null));
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存当前对象数据至[初始数据]
        /// </summary>
        private void SaveInitialData()
        {
            try
            {
                var tp = this.GetType();
                foreach (var pi in tp.GetProperties().Where(pi => !"Item".Equals(pi.Name)))
                {
                    if (_initialData.ContainsKey(pi.Name))
                    {
                        _initialData[pi.Name] = pi.GetValue(this, null);
                    }
                    else
                    {
                        _initialData.Add(pi.Name, pi.GetValue(this, null));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 恢复[初始数据]至当前对象
        /// </summary>
        private void RestoreInitialData()
        {
            try
            {
                var tp = this.GetType();
                foreach (var pi in tp.GetProperties().Where(pi => !"Item".Equals(pi.Name)).Where(pi => _initialData.ContainsKey(pi.Name)))
                {
                    pi.SetValue(this, _initialData[pi.Name], null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 公共属性

        /// <summary>
        /// 总记录条数（翻页用）
        /// </summary>
        public Int32? RecordCount { get; set; }

        #endregion

        #region 公共方法

        /// <summary>
        /// 是否有属性值改变
        /// </summary>
        /// <returns>true:有属性值发生变化；false:无属性值变化</returns>
        public bool PropertyValueChanged()
        {
            var tp = this.GetType();
            foreach (var pi in tp.GetProperties().Where(pi => _initialData.ContainsKey(pi.Name)))
            {
                //[初始数据]中的指定属性的值不为空的场合
                if (_initialData[pi.Name] != null)
                {
                    //[初始数据]中的指定属性的值和对应属性当前的值不相同的场合
                    if (!_initialData[pi.Name].Equals(pi.GetValue(this, null)))
                    {
                        //有属性值改变
                        return true;
                    }
                }
                //[初始数据]中的指定属性的值为空的场合
                else
                {
                    //对应属性当前的值不为空的场合
                    if (pi.GetValue(this, null) != null)
                    {
                        //有属性值改变
                        return true;
                    }
                }
                continue;
            }
            //无属性值改变
            return false;
        }
        /// <summary>
        /// 开始监控List的改变，并将当前数据封存
        /// <para>同时清空InsertList,UpdateList,DeleteList</para>
        /// </summary>
        public void StartMonitChanges()
        {
            _isMonitChanges = true;
            //保存当前数据至[初始数据]
            SaveInitialData();
        }
        /// <summary>
        /// 暂停监控List的改变
        /// </summary>
        public void SuspendMonitChanges()
        {
            _isMonitChanges = false;
        }
        /// <summary>
        /// 继续监控List的改变
        /// </summary>
        public void ContinueMonitChanges()
        {
            _isMonitChanges = true;
        }
        /// <summary>
        /// 结束监控List的改变，并将当前数据封存
        /// <para>同时清空InsertList,UpdateList,DeleteList</para>
        /// </summary>
        public void EndMoinitChanges()
        {
            _isMonitChanges = false;
            //保存当前数据至[初始数据]
            SaveInitialData();
        }
        /// <summary>
        /// 将封存的数据恢复给当前对象，结束停止监控List的改变
        /// <para>同时清空InsertList,UpdateList,DeleteList</para>
        /// </summary>
        public void Restore()
        {
            _isMonitChanges = false;
            //恢复[初始数据]至当前对象
            RestoreInitialData();
        }

        /// <summary>
        /// 将当前对象转换为指定类型的对象
        /// <para>将属性名和类型相同的属性进行赋值</para>
        /// </summary>
        /// <typeparam name="TModel">结果Model类型</typeparam>
        /// <returns>结果Model</returns>
        public TModel ToModel<TModel>()
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
        public TBModel ToTBModelForUpdateAndDelete<TBModel>()
        {
            var resultObj = System.Activator.CreateInstance<TBModel>();
            if (!typeof(TBModel).FullName.Contains("TBModel"))
            {
                throw new Exception("类型[TBModel]必须是TBModel！");
            }
            try
            {
                Type tp = this.GetType();
                foreach (PropertyInfo pi in tp.GetProperties())
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

        #endregion
    }
}
