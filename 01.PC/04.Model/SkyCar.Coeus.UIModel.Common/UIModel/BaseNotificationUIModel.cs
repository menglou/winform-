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
    /// <para>�ɼ�����ݱ仯��UIModel</para>
    /// </summary>
    public class BaseNotificationUIModel : NotificationObject
    {
        #region ˽�б���

        /// <summary>
        /// �Ƿ����ڼ�ص�ǰ����ı仯
        /// </summary>
        private bool _isMonitChanges = false;
        /// <summary>
        /// ��ʼ����
        /// </summary>
        private Dictionary<string, object> _initialData = new Dictionary<string, object>();

        #endregion

        #region ˽�з���

        /// <summary>
        /// ����ֵ�仯���¼�
        /// </summary>
        /// <param name="propertyName">������</param>
        protected override void RaisePropertyChanged(string propertyName)
        {
            try
            {
                base.RaisePropertyChanged(propertyName);
                //δ��صĳ��ϣ�����ǰ�䶯��ֵ���浽[��ʼ����]
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
        /// ���浱ǰ����������[��ʼ����]
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
        /// �ָ�[��ʼ����]����ǰ����
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

        #region ��������

        /// <summary>
        /// �ܼ�¼��������ҳ�ã�
        /// </summary>
        public Int32? RecordCount { get; set; }

        #endregion

        #region ��������

        /// <summary>
        /// �Ƿ�������ֵ�ı�
        /// </summary>
        /// <returns>true:������ֵ�����仯��false:������ֵ�仯</returns>
        public bool PropertyValueChanged()
        {
            var tp = this.GetType();
            foreach (var pi in tp.GetProperties().Where(pi => _initialData.ContainsKey(pi.Name)))
            {
                //[��ʼ����]�е�ָ�����Ե�ֵ��Ϊ�յĳ���
                if (_initialData[pi.Name] != null)
                {
                    //[��ʼ����]�е�ָ�����Ե�ֵ�Ͷ�Ӧ���Ե�ǰ��ֵ����ͬ�ĳ���
                    if (!_initialData[pi.Name].Equals(pi.GetValue(this, null)))
                    {
                        //������ֵ�ı�
                        return true;
                    }
                }
                //[��ʼ����]�е�ָ�����Ե�ֵΪ�յĳ���
                else
                {
                    //��Ӧ���Ե�ǰ��ֵ��Ϊ�յĳ���
                    if (pi.GetValue(this, null) != null)
                    {
                        //������ֵ�ı�
                        return true;
                    }
                }
                continue;
            }
            //������ֵ�ı�
            return false;
        }
        /// <summary>
        /// ��ʼ���List�ĸı䣬������ǰ���ݷ��
        /// <para>ͬʱ���InsertList,UpdateList,DeleteList</para>
        /// </summary>
        public void StartMonitChanges()
        {
            _isMonitChanges = true;
            //���浱ǰ������[��ʼ����]
            SaveInitialData();
        }
        /// <summary>
        /// ��ͣ���List�ĸı�
        /// </summary>
        public void SuspendMonitChanges()
        {
            _isMonitChanges = false;
        }
        /// <summary>
        /// �������List�ĸı�
        /// </summary>
        public void ContinueMonitChanges()
        {
            _isMonitChanges = true;
        }
        /// <summary>
        /// �������List�ĸı䣬������ǰ���ݷ��
        /// <para>ͬʱ���InsertList,UpdateList,DeleteList</para>
        /// </summary>
        public void EndMoinitChanges()
        {
            _isMonitChanges = false;
            //���浱ǰ������[��ʼ����]
            SaveInitialData();
        }
        /// <summary>
        /// ���������ݻָ�����ǰ���󣬽���ֹͣ���List�ĸı�
        /// <para>ͬʱ���InsertList,UpdateList,DeleteList</para>
        /// </summary>
        public void Restore()
        {
            _isMonitChanges = false;
            //�ָ�[��ʼ����]����ǰ����
            RestoreInitialData();
        }

        /// <summary>
        /// ����ǰ����ת��Ϊָ�����͵Ķ���
        /// <para>����������������ͬ�����Խ��и�ֵ</para>
        /// </summary>
        /// <typeparam name="TModel">���Model����</typeparam>
        /// <returns>���Model</returns>
        public TModel ToModel<TModel>()
        {
            var resultObj = System.Activator.CreateInstance<TModel>();
            try
            {
                Type tp = this.GetType();
                foreach (PropertyInfo pi in tp.GetProperties())
                {
                    //����������
                    if ("Item".Equals(pi.Name)) continue;

                    if (pi.GetValue(this, null) != null)
                    {
                        foreach (PropertyInfo piT in resultObj.GetType().GetProperties())
                        {
                            //����������
                            if ("Item".Equals(piT.Name)) continue;

                            //�ж������������������Ƿ���ȣ��ҿ�д
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
        /// ����ǰ����ת��Ϊָ�����͵�TBModel����
        /// <para>����������������ͬ�����Խ��и�ֵ������ǰ�����[XXX_ID]��[XXX_VersionNo]�ḳֵ����������[WHERE_XxX_ID]��[WHERE_XXX_VersionNo]</para>
        /// </summary>
        /// <typeparam name="TBModel">���TBModel����</typeparam>
        /// <returns>���TBModel</returns>
        public TBModel ToTBModelForUpdateAndDelete<TBModel>()
        {
            var resultObj = System.Activator.CreateInstance<TBModel>();
            if (!typeof(TBModel).FullName.Contains("TBModel"))
            {
                throw new Exception("����[TBModel]������TBModel��");
            }
            try
            {
                Type tp = this.GetType();
                foreach (PropertyInfo pi in tp.GetProperties())
                {
                    //����������
                    if ("Item".Equals(pi.Name)) continue;
                    //���˿�ֵ
                    if (pi.GetValue(this, null) == null) continue;

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

                    if (pi.GetValue(this, null) != null)
                    {
                        foreach (PropertyInfo piT in resultObj.GetType().GetProperties())
                        {
                            //����������
                            if ("Item".Equals(piT.Name)) continue;

                            if (isPk)
                            {
                                //�ж������������������Ƿ���ȣ��ҿ�д
                                if (piT.Name == "WHERE_" + pi.Name && piT.PropertyType.Name == pi.PropertyType.Name &&
                                    piT.CanWrite)
                                {
                                    piT.SetValue(resultObj, pi.GetValue(this, null), null);
                                    break;
                                }
                            }
                            else if (isVersion)
                            {
                                //�ж������������������Ƿ���ȣ��ҿ�д
                                if (piT.Name == "WHERE_" + pi.Name && piT.PropertyType.Name == pi.PropertyType.Name &&
                                    piT.CanWrite)
                                {
                                    piT.SetValue(resultObj, pi.GetValue(this, null), null);
                                    break;
                                }
                            }
                            else
                            {
                                //�ж������������������Ƿ���ȣ��ҿ�д
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
