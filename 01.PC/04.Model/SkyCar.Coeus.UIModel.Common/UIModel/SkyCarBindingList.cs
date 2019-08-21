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
    /// <para>�ɼ�����ݱ仯��List</para>
    /// </summary>
    public class SkyCarBindingList<NotificationModel, TBModel> : BindingList<NotificationModel> where NotificationModel : NotificationObject
    {
        #region ��������

        /// <summary>
        /// ��Ҫ[����]��UIModel�б�
        /// </summary>
        public IList<NotificationModel> InsertList { get; set; }
        /// <summary>
        /// ��Ҫ[����]��UIModel�б�
        /// </summary>
        public IList<NotificationModel> UpdateList { get; set; }
        /// <summary>
        /// ��Ҫ[ɾ��]��UIModel�б�
        /// </summary>
        public IList<NotificationModel> DeleteList { get; set; }

        /// <summary>
        /// �Ƿ����ڼ��List�仯
        /// <para>ֻ��</para>
        /// </summary>
        public bool IsMonitChanges { get; private set; }

        #endregion

        #region ˽������

        /// <summary>
        /// ��ʼ�����б�
        /// </summary>
        private IList<NotificationModel> _initialList { get; set; }
        /// <summary>
        /// ɾ������Ŀ
        /// </summary>
        private NotificationModel _deletedItem { get; set; }

        #endregion

        #region ���췽��

        public SkyCarBindingList()
        {
            InsertList = new BindingList<NotificationModel>();
            UpdateList = new BindingList<NotificationModel>();
            DeleteList = new BindingList<NotificationModel>();
            _initialList = new List<NotificationModel>();
        }

        #endregion

        #region ˽�з���

        protected override void RemoveItem(int index)
        {
            _deletedItem = this[index];
            base.RemoveItem(index);
        }


        /// <summary>
        /// List�仯�¼�
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
        /// List���Ӻ�Ĵ���
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
        /// List�ڶ�������ֵ�仯��Ĵ���
        /// </summary>
        /// <param name="e"></param>
        private void ItemChanged(ListChangedEventArgs e)
        {
            if ("IsChecked".Equals(e.PropertyDescriptor.Name))
            {
                return;
            }

            //***************************
            //�Ƿ�Ҫ����TBModel���������
            //***************************

            if (!InsertList.Contains(this[e.NewIndex]) && !DeleteList.Contains(this[e.NewIndex]) &&
                !UpdateList.Contains(this[e.NewIndex]))
            {
                UpdateList.Add(this[e.NewIndex]);
            }
        }

        /// <summary>
        /// Listɾ����Ĵ���
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
        /// List��պ�Ĵ���
        /// </summary>
        private void ItemReset()
        {
            InsertList.Clear();
            UpdateList.Clear();
            DeleteList.Clear();
        }

        /// <summary>
        /// ���Ǹ��ƣ����������������ͣ���[ԴModelLis]��ֵ���Ƶ�[���Model����]��IList[D]�ڣ�
        /// </summary>
        /// <typeparam name="S">ԴModel����</typeparam>
        /// <typeparam name="D">���Model����</typeparam>
        /// <param name="paramModelList">ԴModelList</param>
        /// <param name="paramResultList">���ModelList��List,BindingList,ObservableCollection��</param>
        private void CopyModelList<S, D>(IList<S> paramModelList, IList<D> paramResultList)
        {
            //��ս��List
            paramResultList.Clear();
            foreach (S obj in paramModelList)
            {
                D objT = System.Activator.CreateInstance<D>();
                objT = CopyModel<D>(obj);
                paramResultList.Add(objT);
            }
        }
        /// <summary>
        /// ���Ǹ��ƣ����������������ͣ���[ԴModelLis]��ֵ���Ƶ�[���Model����]��IList[D]�ڣ�
        /// </summary>
        /// <typeparam name="S">ԴModel����</typeparam>
        /// <typeparam name="TBModel">���Model����</typeparam>
        /// <param name="paramModelList">ԴModelList</param>
        /// <param name="paramResultList">���ModelList��List,BindingList,ObservableCollection��</param>
        private void CopyModelListForInsert<S, TBModel>(IList<S> paramModelList, IList<TBModel> paramResultList)
        {
            //��ս��List
            paramResultList.Clear();
            foreach (var objT in paramModelList.Select(obj => CopyModelForInsert<TBModel>(obj)))
            {
                paramResultList.Add(objT);
            }
        }

        /// <summary>
        /// ���Ǹ��ƣ����������������ͣ���[ԴModelLis]��ֵ���Ƶ�[���Model����]��IList[D]�ڣ�
        /// </summary>
        /// <typeparam name="S">ԴModel����</typeparam>
        /// <typeparam name="D">���Model����</typeparam>
        /// <param name="paramModelList">ԴModelList</param>
        /// <param name="paramResultList">���ModelList��List,BindingList,ObservableCollection��</param>
        private void CopyModelListForUpdateAndDelete<S, D>(IList<S> paramModelList, IList<D> paramResultList)
        {
            //��ս��List
            paramResultList.Clear();
            foreach (S obj in paramModelList)
            {
                D objT = System.Activator.CreateInstance<D>();
                objT = CopyModelForUpdateAndDelete<D>(obj);
                paramResultList.Add(objT);
            }
        }

        /// <summary>
        /// ���Ǹ��ƣ����������������ͣ���[ԴModel]��ֵ���Ƶ�[���TBModel����]��Model�ڣ�
        /// <para>�����ᱻ���¸�ֵ</para>
        /// </summary>
        /// <typeparam name="TBModel">���TBModel����</typeparam>
        /// <param name="paramModel">ԴModel</param>
        /// <returns>���Model</returns>
        private TBModel CopyModelForInsert<TBModel>(object paramModel)
        {
            var resultObj = Activator.CreateInstance<TBModel>();
            try
            {
                #region �ҵ�����
                var pk = string.Empty;
                foreach (var piT in resultObj.GetType().GetProperties())
                {
                    //����������
                    if ("Item".Equals(piT.Name))
                    {
                        continue;
                    }
                    string[] tmp = piT.Name.Split(new char[] { Convert.ToChar("_") });
                    //�������ĳ���
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
                    //����������
                    if ("Item".Equals(pi.Name))
                    {
                        continue;
                    }

                    if (pi.GetValue(paramModel, null) != null || pk.Equals(pi.Name))
                    {
                        if (pk.Equals(pi.Name))
                        {
                            //��ID
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
                            //����������
                            if ("Item".Equals(piT.Name))
                            {
                                continue;
                            }

                            //�ж������������������Ƿ���ȣ��ҿ�д
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
        /// ���Ǹ��ƣ����������������ͣ���[ԴModel]��ֵ���Ƶ�[���Model����]��Model�ڣ�
        /// </summary>
        /// <typeparam name="D">���Model����</typeparam>
        /// <param name="paramModel">ԴModel</param>
        /// <returns>���Model</returns>
        private D CopyModel<D>(object paramModel)
        {
            D resultObj = System.Activator.CreateInstance<D>();
            try
            {
                Type tp = paramModel.GetType();
                foreach (PropertyInfo pi in tp.GetProperties())
                {
                    //����������
                    if ("Item".Equals(pi.Name)) continue;

                    if (pi.GetValue(paramModel, null) != null)
                    {
                        foreach (PropertyInfo piT in resultObj.GetType().GetProperties())
                        {
                            //����������
                            if ("Item".Equals(piT.Name)) continue;

                            //�ж������������������Ƿ���ȣ��ҿ�д
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
        /// ���Ǹ��ƣ����������������ͣ���[ԴModel]��ֵ���Ƶ�[���Model����]��Model�ڣ�
        /// </summary>
        /// <typeparam name="D">���Model����</typeparam>
        /// <param name="paramModel">ԴModel</param>
        /// <returns>���Model</returns>
        private D CopyModelForUpdateAndDelete<D>(object paramModel)
        {
            D resultObj = System.Activator.CreateInstance<D>();
            try
            {
                Type tp = paramModel.GetType();
                foreach (PropertyInfo pi in tp.GetProperties())
                {
                    //����������
                    if ("Item".Equals(pi.Name))
                    {
                        continue;
                    }
                    //���˿�ֵ
                    if (pi.GetValue(paramModel, null) == null)
                    {
                        continue;
                    }

                    //�Ƿ���[����]
                    var isPk = false;
                    //�Ƿ���[�汾��]
                    var isVersion = false;

                    string[] tmp = pi.Name.Split(new char[] { Convert.ToChar("_") });
                    //�������ĳ���
                    if (tmp.Length == 2 && "ID".Equals(tmp[1]))
                    {
                        isPk = true;
                    }
                    //�ǰ汾�ŵĳ���
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
                            //����������
                            if ("Item".Equals(piT.Name))
                            {
                                continue;
                            }

                            if (isPk)
                            {
                                //�ж������������������Ƿ���ȣ��ҿ�д
                                if (piT.Name == "WHERE_" + pi.Name && piT.PropertyType.Name == pi.PropertyType.Name &&
                                    piT.CanWrite)
                                {
                                    piT.SetValue(resultObj, pi.GetValue(paramModel, null), null);
                                    break;
                                }
                            }
                            else if (isVersion)
                            {
                                //�ж������������������Ƿ���ȣ��ҿ�д
                                if (piT.Name == "WHERE_" + pi.Name && piT.PropertyType.Name == pi.PropertyType.Name &&
                                    piT.CanWrite)
                                {
                                    piT.SetValue(resultObj, pi.GetValue(paramModel, null), null);
                                    break;
                                }
                            }
                            else
                            {
                                //�ж������������������Ƿ���ȣ��ҿ�д
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

        #region ��������

        /// <summary>
        /// ��ʼ���List�ĸı䣬������ǰ���ݷ��
        /// <para>ͬʱ���InsertList,UpdateList,DeleteList</para>
        /// </summary>
        public void StartMonitChanges()
        {
            IsMonitChanges = true;
            //���浱ǰ������[��ʼ�����б�]
            CopyModelList<NotificationModel, NotificationModel>(this, _initialList);
            InsertList.Clear();
            UpdateList.Clear();
            DeleteList.Clear();
        }
        /// <summary>
        /// ��ͣ���List�ĸı�
        /// </summary>
        public void SuspendMonitChanges()
        {
            IsMonitChanges = false;
        }
        /// <summary>
        /// �������List�ĸı�
        /// </summary>
        public void ContinueMonitChanges()
        {
            IsMonitChanges = true;
        }
        /// <summary>
        /// �������List�ĸı䣬������ǰ���ݷ��
        /// <para>ͬʱ���InsertList,UpdateList,DeleteList</para>
        /// </summary>
        public void EndMoinitChanges()
        {
            IsMonitChanges = false;
            //���浱ǰ������[��ʼ�����б�]
            CopyModelList<NotificationModel, NotificationModel>(this, _initialList);
            InsertList.Clear();
            UpdateList.Clear();
            DeleteList.Clear();
        }
        /// <summary>
        /// ���������ݻָ�����ǰList������ֹͣ���List�ĸı�
        /// <para>ͬʱ���InsertList,UpdateList,DeleteList</para>
        /// </summary>
        public void RestoreList()
        {
            IsMonitChanges = false;
            //���浱ǰ������[��ʼ�����б�]
            CopyModelList<NotificationModel, NotificationModel>(_initialList, this);
            InsertList.Clear();
            UpdateList.Clear();
            DeleteList.Clear();
        }
        /// <summary>
        /// ��ȡ��Ҫ[����]��TBModel�б�
        /// </summary>
        /// <returns></returns>
        public IList<TBModel> GetInsertList()
        {
            var resultList = new BindingList<TBModel>();
            CopyModelListForInsert<NotificationModel, TBModel>(InsertList, resultList);
            return resultList;
        }
        /// <summary>
        /// ��ȡ��Ҫ[����]��TBModel�б�
        /// </summary>
        /// <returns></returns>
        public IList<TBModel> GetUpdateList()
        {
            var resultList = new BindingList<TBModel>();

            CopyModelListForUpdateAndDelete<NotificationModel, TBModel>(UpdateList, resultList);
            return resultList;
        }
        /// <summary>
        /// ��ȡ��Ҫ[ɾ��]��TBModel�б�
        /// </summary>
        /// <returns></returns>
        public IList<TBModel> GetDeleteList()
        {
            var resultList = new BindingList<TBModel>();
            CopyModelListForUpdateAndDelete<NotificationModel, TBModel>(DeleteList, resultList);
            return resultList;
        }

        /// <summary>
        /// ����ǰListת��Ϊָ��TModel���͵�List
        /// </summary>
        /// <typeparam name="TModel">���Model����</typeparam>
        /// <param name="paramResultList">���List</param>
        public void ToModelList<TModel>(IList<TModel> paramResultList)
        {
            //��ս��List
            paramResultList.Clear();
            foreach (var obj in this)
            {
                var objT = CopyModel<TModel>(obj);
                paramResultList.Add(objT);
            }
        }

        /// <summary>
        /// ����ǰListת��Ϊָ�����͵�TBModelList
        /// </summary>
        /// <typeparam name="TBModel">TBModel����</typeparam>
        /// <param name="paramResultList">���List</param>
        public void ToTBModelListForUpdateAndDelete<TBModel>(IList<TBModel> paramResultList)
        {
            if (!typeof(TBModel).FullName.Contains("TBModel"))
            {
                throw new Exception("����[TBModel]������TBModel��");
            }
            //��ս��List
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
