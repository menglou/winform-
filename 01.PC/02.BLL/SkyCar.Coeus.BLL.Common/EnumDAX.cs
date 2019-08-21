using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.DAL;
using System.ComponentModel;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.ComModel;

namespace SkyCar.Coeus.BLL.Common
{
    /// <summary>
    /// 枚举
    /// </summary>
    public class EnumDAX
    {
        #region 变量定义
        /// <summary>
        /// 业务模块
        /// </summary>
        public const string BussID = "DAXEnum";

        /// <summary>
        /// 系统枚举
        /// </summary>
        private static Dictionary<EnumKey, List<MDLSM_Enum>> dicEnum = new Dictionary<EnumKey, List<MDLSM_Enum>>();
        #endregion


        #region 自定义方法
        /// <summary>
        /// 初始化系统枚举
        /// </summary>
        public static void InitializeEnum()
        {
            //记录日志
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, System.Reflection.MethodBase.GetCurrentMethod().ToString(), "", "", null);

            //定义枚举对象列表
            BindingList<MDLSM_Enum> resultList = new BindingList<MDLSM_Enum>();

            //创建数据CRUD对象
            var bll = new BLLBase(Trans.COM);
            //查询系统所有枚举数据
            bll.QueryForList<MDLSM_Enum>(new MDLSM_Enum(), resultList);
            //初始化字典
            dicEnum = new Dictionary<EnumKey, List<MDLSM_Enum>>();

            //将查询结果数据保存到字典中
            foreach (MDLSM_Enum enu in resultList.OrderBy(x => x.Enum_ID))
            {
                //将字符串转换成对应的枚举类型
                EnumKey eKey = (EnumKey)Enum.Parse(typeof(EnumKey), enu.Enum_Key, true);

                //枚举Key存在的场合：追加数据到枚举值列表
                if (dicEnum.ContainsKey(eKey))
                {
                    //创建枚举值对象
                    List<MDLSM_Enum> valueList = new List<MDLSM_Enum>();
                    //获取已有枚举值列表
                    valueList = dicEnum[eKey];
                    valueList.Add(enu);
                }
                //枚举Key不存在的场合：新增枚举值
                else
                {
                    //创建枚举值对象
                    List<MDLSM_Enum> valueList = new List<MDLSM_Enum>();
                    valueList.Add(enu);
                    dicEnum.Add(eKey, valueList);
                }
            }
            //记录日志
            LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, System.Reflection.MethodBase.GetCurrentMethod().ToString(), "", "", null);
        }
        /// <summary>
        /// 根据枚举编码，获取对应【枚举信息】的列表
        /// </summary>
        /// <param name="paramKey"></param>
        /// <returns></returns>
        public static List<MDLSM_Enum> GetEnum(EnumKey paramKey)
        {
            return dicEnum[paramKey];
        }
        /// <summary>
        /// 根据枚举编码，获取对应【枚举值】和【枚举名称】的列表
        /// </summary>
        /// <param name="paramKey"></param>
        /// <returns></returns>
        public static List<ComComboBoxDataSource> GetEnumForComboBoxWithValueText(EnumKey paramKey)
        {
            List<MDLSM_Enum> cacheEnums = new List<MDLSM_Enum>();
            List<ComComboBoxDataSource> resultList = new List<ComComboBoxDataSource>();
            if (!dicEnum.ContainsKey(paramKey))
            {
                return resultList;
            }
            else
            {
                cacheEnums = dicEnum[paramKey];
            }

            foreach (MDLSM_Enum enu in cacheEnums.OrderBy(x => x.Enum_ID))
            {
                ComComboBoxDataSource ds = new ComComboBoxDataSource();
                ds.Value = enu.Enum_Value == null ? -1 : (int)enu.Enum_Value;
                ds.Text = enu.Enum_DisplayName;
                resultList.Add(ds);
            }
            return resultList;
        }
        /// <summary>
        /// 根据枚举编码，获取对应【枚举值编码】和【枚举名称】的列表
        /// </summary>
        /// <param name="paramKey"></param>
        /// <returns></returns>
        public static List<ComComboBoxDataSourceTC> GetEnumForComboBoxWithCodeText(EnumKey paramKey)
        {
            List<MDLSM_Enum> cacheEnums = new List<MDLSM_Enum>();
            List<ComComboBoxDataSourceTC> resultList = new List<ComComboBoxDataSourceTC>();
            if (!dicEnum.ContainsKey(paramKey))
            {
                return resultList;
            }
            else
            {
                cacheEnums = dicEnum[paramKey];
            }
            foreach (MDLSM_Enum enu in cacheEnums.OrderBy(x => x.Enum_ID))
            {
                ComComboBoxDataSourceTC ds = new ComComboBoxDataSourceTC();
                ds.Code = enu.Enum_ValueCode;
                ds.Text = enu.Enum_DisplayName;
                resultList.Add(ds);
            }
            return resultList;
        }
        #endregion
    }

}
