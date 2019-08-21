using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using SkyCar.Coeus.Common.ExtendClass.Interface;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// SkyCarBindingList
    /// <para>可监控内容变化的List</para>
    /// </summary>
    public class SkyCarBindingList<NotificationModel, TBModel> : BindingList<NotificationModel> where NotificationModel : NotificationObject
    {
        #region 公共属性

        /// <summary>
        /// 需要[新增]的UIModel列表
        /// </summary>
        public IList<NotificationModel> InsertList { get; set; }
        /// <summary>
        /// 需要[更新]的UIModel列表
        /// </summary>
        public IList<NotificationModel> UpdateList { get; set; }
        /// <summary>
        /// 需要[删除]的UIModel列表
        /// </summary>
        public IList<NotificationModel> DeleteList { get; set; }

        /// <summary>
        /// 是否正在监控List变化
        /// <para>只读</para>
        /// </summary>
        public bool IsMonitChanges { get; private set; }

        #endregion

        #region 私有属性

        /// <summary>
        /// 初始数据列表
        /// </summary>
        private IList<NotificationModel> _initialList { get; set; }
        /// <summary>
        /// 删除的项目
        /// </summary>
        private NotificationModel _deletedItem { get; set; }

        #endregion

        #region 构造方法

        public SkyCarBindingList()
        {
            InsertList = new BindingList<NotificationModel>();
            UpdateList = new BindingList<NotificationModel>();
            DeleteList = new BindingList<NotificationModel>();
            _initialList = new List<NotificationModel>();
        }

        #endregion

        #region 私有方法

        protected override void RemoveItem(int index)
        {
            _deletedItem = this[index];
            base.RemoveItem(index);
        }


        /// <summary>
        /// List变化事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnListChanged(ListChangedEventArgs e)
        {
            if (!IsMonitChanges)
            {
                return;
            }
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    ItemAdded(e);
                    break;
                case ListChangedType.ItemChanged:
                    ItemChanged(e);
                    break;
                case ListChangedType.ItemDeleted:
                    ItemDeleted(e);
                    break;
                case ListChangedType.ItemMoved:

                    break;
                case ListChangedType.Reset:
                    ItemReset();
                    break;
                default:
                    break;
            }
            base.OnListChanged(e);
        }

        /// <summary>
        /// List增加后的处理
        /// </summary>
        /// <param name="e"></param>
        private void ItemAdded(ListChangedEventArgs e)
        {
            if (!InsertList.Contains(this[e.NewIndex]))
            {
                InsertList.Add(this[e.NewIndex]);
            }
        }

        /// <summary>
        /// List内对象属性值变化后的处理
        /// </summary>
        /// <param name="e"></param>
        private void ItemChanged(ListChangedEventArgs e)
        {
            if ("IsChecked".Equals(e.PropertyDescriptor.Name))
            {
                return;
            }

            //***************************
            //是否要过滤TBModel以外的属性
            //***************************

            if (!InsertList.Contains(this[e.NewIndex]) && !DeleteList.Contains(this[e.NewIndex]) &&
                !UpdateList.Contains(this[e.NewIndex]))
            {
                UpdateList.Add(this[e.NewIndex]);
            }
        }

        /// <summary>
        /// List删除后的处理
        /// </summary>
        /// <param name="e"></param>
        private void ItemDeleted(ListChangedEventArgs e)
        {
            if (InsertList.Contains(_deletedItem))
            {
                InsertList.Remove(_deletedItem);
                return;
            }
            if (UpdateList.Contains(_deletedItem))
            {
                UpdateList.Remove(_deletedItem);
            }

            if (!DeleteList.Contains(_deletedItem))
            {
                DeleteList.Add(_deletedItem);
            }
        }

        /// <summary>
        /// List清空后的处理
        /// </summary>
        private void ItemReset()
        {
            InsertList.Clear();
            UpdateList.Clear();
            DeleteList.Clear();
        }

        /// <summary>
        /// 覆盖复制（根据属性名和类型，将[源ModelLis]的值复制到[结果Model类型]的IList[D]内）
        /// </summary>
        /// <typeparam name="S">源Model类型</typeparam>
        /// <typeparam name="D">结果Model类型</typeparam>
        /// <param name="paramModelList">源ModelList</param>
        /// <param name="paramResultList">结果ModelList（List,BindingList,ObservableCollection）</param>
        private void CopyModelList<S, D>(IList<S> paramModelList, IList<D> paramResultList)
        {
            //清空结果List
            paramResultList.Clear();
            foreach (S obj in paramModelList)
            {
                D objT = System.Activator.CreateInstance<D>();
                objT = CopyModel<D>(obj);
                paramResultList.Add(objT);
            }
        }
        /// <summary>
        /// 覆盖复制（根据属性名和类型，将[源ModelLis]的值复制到[结果Model类型]的IList[D]内）
        /// </summary>
        /// <typeparam name="S">源Model类型</typeparam>
        /// <typeparam name="TBModel">结果Model类型</typeparam>
        /// <param name="paramModelList">源ModelList</param>
        /// <param name="paramResultList">结果ModelList（List,BindingList,ObservableCollection）</param>
        private void CopyModelListForInsert<S, TBModel>(IList<S> paramModelList, IList<TBModel> paramResultList)
        {
            //清空结果List
            paramResultList.Clear();
            foreach (var objT in paramModelList.Select(obj => CopyModelForInsert<TBModel>(obj)))
            {
                paramResultList.Add(objT);
            }
        }

        /// <summary>
        /// 覆盖复制（根据属性名和类型，将[源ModelLis]的值复制到[结果Model类型]的IList[D]内）
        /// </summary>
        /// <typeparam name="S">源Model类型</typeparam>
        /// <typeparam name="D">结果Model类型</typeparam>
        /// <param name="paramModelList">源ModelList</param>
        /// <param name="paramResultList">结果ModelList（List,BindingList,ObservableCollection）</param>
        private void CopyModelListForUpdateAndDelete<S, D>(IList<S> paramModelList, IList<D> paramResultList)
        {
            //清空结果List
            paramResultList.Clear();
            foreach (S obj in paramModelList)
            {
                D objT = System.Activator.CreateInstance<D>();
                objT = CopyModelForUpdateAndDelete<D>(obj);
                paramResultList.Add(objT);
            }
        }

        /// <summary>
        /// 覆盖复制（根据属性名和类型，将[源Model]的值复制到[结果TBModel类型]的Model内）
        /// <para>主键会被重新赋值</para>
        /// </summary>
        /// <typeparam name="TBModel">结果TBModel类型</typeparam>
        /// <param name="paramModel">源Model</param>
        /// <returns>结果Model</returns>
        private TBModel CopyModelForInsert<TBModel>(object paramModel)
        {
            var resultObj = Activator.CreateInstance<TBModel>();
            try
            {
                #region 找到主键
                var pk = string.Empty;
                foreach (var piT in resultObj.GetType().GetProperties())
                {
                    //过滤索引器
                    if ("Item".Equals(piT.Name))
                    {
                        continue;
                    }
                    string[] tmp = piT.Name.Split(new char[] { Convert.ToChar("_") });
                    //是主键的场合
                    if (tmp.Length == 2 && "ID".Equals(tmp[1]))
                    {
                        pk = piT.Name;
                        break;
                    }
                }
                #endregion

                var tp = paramModel.GetType();
                foreach (var pi in tp.GetProperties())
                {
                    //过滤索引器
                    if ("Item".Equals(pi.Name))
                    {
                        continue;
                    }

                    if (pi.GetValue(paramModel, null) != null || pk.Equals(pi.Name))
                    {
                        if (pk.Equals(pi.Name))
                        {
                            //新ID
                            string tmpID;
                            var getId = pi.GetValue(paramModel, null);
                            if (getId != null && !string.IsNullOrEmpty(getId.ToString()))
                            {
                                tmpID = getId.ToString();
                            }
                            else
                            {
                                tmpID = Guid.NewGuid().ToString();
                            }
                            pi.SetValue(paramModel, tmpID, null);
                        }
                        foreach (var piT in resultObj.GetType().GetProperties())
                        {
                            //过滤索引器
                            if ("Item".Equals(piT.Name))
                            {
                                continue;
                            }

                            //判断属性名和属性类型是否相等，且可写
                            if (piT.Name == pi.Name && piT.PropertyType.Name == pi.PropertyType.Name && piT.CanWrite)
                            {
                                piT.SetValue(resultObj, pi.GetValue(paramModel, null), null);
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
        /// 覆盖复制（根据属性名和类型，将[源Model]的值复制到[结果Model类型]的Model内）
        /// </summary>
        /// <typeparam name="D">结果Model类型</typeparam>
        /// <param name="paramModel">源Model</param>
        /// <returns>结果Model</returns>
        private D CopyModel<D>(object paramModel)
        {
            D resultObj = System.Activator.CreateInstance<D>();
            try
            {
                Type tp = paramModel.GetType();
                foreach (PropertyInfo pi in tp.GetProperties())
                {
                    //过滤索引器
                    if ("Item".Equals(pi.Name)) continue;

                    if (pi.GetValue(paramModel, null) != null)
                    {
                        foreach (PropertyInfo piT in resultObj.GetType().GetProperties())
                        {
                            //过滤索引器
                            if ("Item".Equals(piT.Name)) continue;

                            //判断属性名和属性类型是否相等，且可写
                            if (piT.Name == pi.Name && piT.PropertyType.Name == pi.PropertyType.Name && piT.CanWrite)
                            {
                                piT.SetValue(resultObj, pi.GetValue(paramModel, null), null);
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
        /// 覆盖复制（根据属性名和类型，将[源Model]的值复制到[结果Model类型]的Model内）
        /// </summary>
        /// <typeparam name="D">结果Model类型</typeparam>
        /// <param name="paramModel">源Model</param>
        /// <returns>结果Model</returns>
        private D CopyModelForUpdateAndDelete<D>(object paramModel)
        {
            D resultObj = System.Activator.CreateInstance<D>();
            try
            {
                Type tp = paramModel.GetType();
                foreach (PropertyInfo pi in tp.GetProperties())
                {
                    //过滤索引器
                    if ("Item".Equals(pi.Name))
                    {
                        continue;
                    }
                    //过滤空值
                    if (pi.GetValue(paramModel, null) == null)
                    {
                        continue;
                    }

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

                    if (pi.GetValue(paramModel, null) != null)
                    {
                        foreach (PropertyInfo piT in resultObj.GetType().GetProperties())
                        {
                            //过滤索引器
                            if ("Item".Equals(piT.Name))
                            {
                                continue;
                            }

                            if (isPk)
                            {
                                //判断属性名和属性类型是否相等，且可写
                                if (piT.Name == "WHERE_" + pi.Name && piT.PropertyType.Name == pi.PropertyType.Name &&
                                    piT.CanWrite)
                                {
                                    piT.SetValue(resultObj, pi.GetValue(paramModel, null), null);
                                    break;
                                }
                            }
                            else if (isVersion)
                            {
                                //判断属性名和属性类型是否相等，且可写
                                if (piT.Name == "WHERE_" + pi.Name && piT.PropertyType.Name == pi.PropertyType.Name &&
                                    piT.CanWrite)
                                {
                                    piT.SetValue(resultObj, pi.GetValue(paramModel, null), null);
                                    break;
                                }
                            }
                            else
                            {
                                //判断属性名和属性类型是否相等，且可写
                                if (piT.Name == pi.Name && piT.PropertyType.Name == pi.PropertyType.Name && piT.CanWrite)
                                {
                                    piT.SetValue(resultObj, pi.GetValue(paramModel, null), null);
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

        #region 公共方法

        /// <summary>
        /// 开始监控List的改变，并将当前数据封存
        /// <para>同时清空InsertList,UpdateList,DeleteList</para>
        /// </summary>
        public void StartMonitChanges()
        {
            IsMonitChanges = true;
            //保存当前数据至[初始数据列表]
            CopyModelList<NotificationModel, NotificationModel>(this, _initialList);
            InsertList.Clear();
            UpdateList.Clear();
            DeleteList.Clear();
        }
        /// <summary>
        /// 暂停监控List的改变
        /// </summary>
        public void SuspendMonitChanges()
        {
            IsMonitChanges = false;
        }
        /// <summary>
        /// 继续监控List的改变
        /// </summary>
        public void ContinueMonitChanges()
        {
            IsMonitChanges = true;
        }
        /// <summary>
        /// 结束监控List的改变，并将当前数据封存
        /// <para>同时清空InsertList,UpdateList,DeleteList</para>
        /// </summary>
        public void EndMoinitChanges()
        {
            IsMonitChanges = false;
            //保存当前数据至[初始数据列表]
            CopyModelList<NotificationModel, NotificationModel>(this, _initialList);
            InsertList.Clear();
            UpdateList.Clear();
            DeleteList.Clear();
        }
        /// <summary>
        /// 将封存的数据恢复到当前List，结束停止监控List的改变
        /// <para>同时清空InsertList,UpdateList,DeleteList</para>
        /// </summary>
        public void RestoreList()
        {
            IsMonitChanges = false;
            //保存当前数据至[初始数据列表]
            CopyModelList<NotificationModel, NotificationModel>(_initialList, this);
            InsertList.Clear();
            UpdateList.Clear();
            DeleteList.Clear();
        }
        /// <summary>
        /// 获取需要[新增]的TBModel列表
        /// </summary>
        /// <returns></returns>
        public IList<TBModel> GetInsertList()
        {
            var resultList = new BindingList<TBModel>();
            CopyModelListForInsert<NotificationModel, TBModel>(InsertList, resultList);
            return resultList;
        }
        /// <summary>
        /// 获取需要[更新]的TBModel列表
        /// </summary>
        /// <returns></returns>
        public IList<TBModel> GetUpdateList()
        {
            var resultList = new BindingList<TBModel>();

            CopyModelListForUpdateAndDelete<NotificationModel, TBModel>(UpdateList, resultList);
            return resultList;
        }
        /// <summary>
        /// 获取需要[删除]的TBModel列表
        /// </summary>
        /// <returns></returns>
        public IList<TBModel> GetDeleteList()
        {
            var resultList = new BindingList<TBModel>();
            CopyModelListForUpdateAndDelete<NotificationModel, TBModel>(DeleteList, resultList);
            return resultList;
        }

        /// <summary>
        /// 将当前List转换为指定TModel类型的List
        /// </summary>
        /// <typeparam name="TModel">结果Model类型</typeparam>
        /// <param name="paramResultList">结果List</param>
        public void ToModelList<TModel>(IList<TModel> paramResultList)
        {
            //清空结果List
            paramResultList.Clear();
            foreach (var obj in this)
            {
                var objT = CopyModel<TModel>(obj);
                paramResultList.Add(objT);
            }
        }

        /// <summary>
        /// 将当前List转换为指定类型的TBModelList
        /// </summary>
        /// <typeparam name="TBModel">TBModel类型</typeparam>
        /// <param name="paramResultList">结果List</param>
        public void ToTBModelListForUpdateAndDelete<TBModel>(IList<TBModel> paramResultList)
        {
            if (!typeof(TBModel).FullName.Contains("TBModel"))
            {
                throw new Exception("类型[TBModel]必须是TBModel！");
            }
            //清空结果List
            paramResultList.Clear();
            foreach (var obj in this)
            {
                var objT = CopyModelForUpdateAndDelete<TBModel>(obj);
                paramResultList.Add(objT);
            }
        }

        #endregion
    }
}
