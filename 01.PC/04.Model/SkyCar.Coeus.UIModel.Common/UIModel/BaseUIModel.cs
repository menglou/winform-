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
        /// �ܼ�¼��������ҳ�ã�
        /// </summary>
        public Int32? RecordCount { get; set; }

        /// <summary>
        /// ����ǰ����ת��Ϊָ�����͵Ķ���
        /// <para>����������������ͬ�����Խ��и�ֵ</para>
        /// </summary>
        /// <typeparam name="TModel">���Model����</typeparam>
        /// <returns>���Model</returns>
        private TModel ToModel<TModel>()
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
        public TBModel ToTBModelForSaveAndDelete<TBModel>()
        {
            var resultObj = System.Activator.CreateInstance<TBModel>();
            if (!typeof(TBModel).FullName.Contains("TBModel"))
            {
                throw new Exception("����[TBModel]������TBModel��");
            }
            try
            {
                Type tp = this.GetType();
                foreach (var pi in tp.GetProperties())
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
    }
}
