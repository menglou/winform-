using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.UIModel.Common.QCModel;
using SkyCar.Coeus.UIModel.Common.UIModel;
using SkyCar.Common.Utility;

namespace SkyCar.Coeus.BLL.Common
{
    /// <summary>
    /// 系统缓存
    /// </summary>
    public class CacheDAX
    {
        #region 变量定义
        /// <summary>
        /// 业务模块
        /// </summary>
        public const string BussID = "DAXCache";
        private static System.Web.Caching.Cache _cache = HttpRuntime.Cache;
        private static BLLBase _baseDax = new BLLBase(Trans.COM);
        #endregion

        #region 缓存数据Key【自定义添加】
        public static class ConfigDataKey
        {
            /// <summary>
            /// 商户编码
            /// </summary>
            public static string MerchantCode = "EEC950DC-3E88-4ED6-A896-B320932F1BED";
            /// <summary>
            /// FTP地址
            /// </summary>
            public static string FtpAddress = "BAA2F6B1-83B6-479A-9E16-60C205E6D7DF";
            /// <summary>
            /// FTP用户名
            /// </summary>
            public static string FtpUserName = "6AB9A614-BB6A-4C81-ADC4-E639A0C1F29B";
            /// <summary>
            /// FTP用户密码
            /// </summary>
            public static string FtpPassword = "64ECEC7C-2905-41F0-85B3-CCD26BEB5DF9";
            /// <summary>
            /// 电子钱包初始密码
            /// </summary>
            public static string WalletDefaultPassword = "A1E6C45B-DF5E-4A99-8290-A0D28A62DD8F";
            /// <summary>
            /// 车辆品牌
            /// </summary>
            public static string VehicleBrand = "0EAC9CD1-5250-4D83-9BCD-184C70F14E03";
            /// <summary>
            /// 车辆品牌车系
            /// </summary>
            public static string VehicleBrandInspire = "EC77776A-6FCE-43AF-9F99-F201675D2CD6";
            /// <summary>
            /// 车辆车型描述
            /// </summary>
            public static string VehicleModelDesc = "B78AEB3D-C045-4C98-9FBE-AF129D5CACE8";
            /// <summary>
            /// 用户业务角色
            /// </summary>
            public static string UserBusinessRole = "ED1604F2-F22E-43F9-AFC6-D21C62A6D9A4";
            /// <summary>
            /// 业务消息模板
            /// </summary>
            public static string BusinessMsgTemplate = "B3FEAC04-AA77-4B25-97E1-90A068C46FB8";
            /// <summary>
            /// 配件名称
            /// </summary>
            public static string AutoPartsName = "FEDA80E3-8A8E-45A8-996E-AB07318B216A";
            /// <summary>
            /// 配件档案
            /// </summary>
            public static string AutoPartsArchive = "92B7DDCF-1770-4444-A983-0ABFC0482997";
            /// <summary>
            /// 配件类别
            /// </summary>
            public static string AutoPartsType = "DB142705-A579-42EB-A3A6-05B901DD14DE";
            /// <summary>
            /// 配件品牌
            /// </summary>
            public static string AutoPartsBrand = "7F5E8A3C-FB3D-4BF8-AA23-892A54AA51AC";
            /// <summary>
            /// 配件规格型号
            /// </summary>
            public static string AutoPartsSpecification = "3CE515C0-9C83-47EC-AAB4-91FA4D177A47";
            /// <summary>
            /// 配件计量单位
            /// </summary>
            public static string AutoPartsUOM = "37D16634-F130-4526-8598-55236452AB96";
            /// <summary>
            /// 配件供货商
            /// </summary>
            public static string AutoPartsSupplier = "8A3D5461-F462-4387-8A86-4E9607C75CBB";
            /// <summary>
            /// 仓库
            /// </summary>
            public static string Warehouse = "E32CC325-6A14-4FB6-8651-30E86654C7E0";
            /// <summary>
            /// 仓位
            /// </summary>
            public static string WarehouseBin = "625B99D1-7530-48C5-B288-D753C3CD7E91";
            /// <summary>
            /// 系统用户权限
            /// </summary>
            public static string SystemUserAuthority = "94F61E0D-BF6A-48E0-A4AB-C29FB1B385A1";
            /// <summary>
            /// 汽修商户
            /// </summary>
            public static string ARMerchant = "26A3EEA2-3A7E-4130-AC9A-A675E884B27E";
            /// <summary>
            /// 汽修商户组织
            /// </summary>
            public static string ARMerchantOrg = "22857707-8474-49AC-8BF8-3C09E7788CAC";
            /// <summary>
            /// 系统用户
            /// </summary>
            public static string SystemUser = "4ED4E54C-6E03-4ABC-BA6C-B4F7617ABC70";
            /// <summary>
            /// 客户
            /// </summary>
            public static string Customer = "CB63DFB6-652A-404A-B6ED-15B842EC136F";
        }
        #endregion

        #region 缓存枚举Key【自定义添加】
        public enum CacheKey
        {
            /// <summary>
            /// 系统邮箱地址
            /// </summary>
            S0000,
            /// <summary>
            /// 系统邮箱密码
            /// </summary>
            S0001,
            /// <summary>
            /// 客户端版权描述
            /// </summary>
            S0003,
            /// <summary>
            /// 是否启用进销存模块
            /// </summary>
            S1001,
            /// <summary>
            /// 电子钱包初始密码
            /// </summary>
            S1006,
            /// <summary>
            /// 当前用户菜单
            /// </summary>
            U1001,

        }
        #endregion

        #region 缓存枚举Type【自定义添加】
        public enum CacheType
        {
            /// <summary>
            /// 界面列表状态
            /// </summary>
            ET1001
        }
        #endregion

        #region 缓存操作

        /// <summary>
        /// 初始化系统参数
        /// </summary>
        public static void InitSystemParameter()
        {
            //记录日志
            LogHelper.WriteBussLog(BussID, LoginInfoDAX.UserName, "CacheDAX.InitializeCache", "【Start】", string.Empty, null);

            string tableName = LocalConfigFileConst.TableName.SM_Parameter;
            string localXmlFilePath = LocalConfigFileConst.ConfigFilePath.OtherTablePath.Replace(SysConst.PH_TABLENAME, tableName);
            var serverTableTimeStamp = SystemConfigInfo.DBTimeStampList.Find(table => table.TableName.Equals(tableName));
            var localTableTimeStamp = LocalConfigInfo.DBTimeStampList.Find(table => table.TableName.Equals(tableName));

            var resultList = new ObservableCollection<MDLSM_Parameter>();

            if (!File.Exists(localXmlFilePath)
                || localTableTimeStamp == null
                || serverTableTimeStamp == null
                || localTableTimeStamp.LastUpdatedTime < serverTableTimeStamp.LastUpdatedTime
                )
            {
                //创建数据CRUD对象
                CRUD crud = new CRUD(DBCONFIG.Coeus);
                //查询系统所有枚举数据
                MDLSM_Parameter argsCondition = new MDLSM_Parameter { Para_IsValid = true };
                crud.QueryForList(argsCondition, resultList);

                XmlFileUtility.SerializeToXmlFile(localXmlFilePath, typeof(ObservableCollection<MDLSM_Parameter>), resultList);
                if (localTableTimeStamp == null)
                {
                    if (serverTableTimeStamp != null)
                    {
                        localTableTimeStamp = new DBTableTimeStampUIModel { TableName = tableName, LastUpdatedTime = serverTableTimeStamp.LastUpdatedTime };
                        LocalConfigInfo.DBTimeStampList.Add(localTableTimeStamp);
                    }
                }
                else
                {
                    if (serverTableTimeStamp != null)
                    {
                        localTableTimeStamp.LastUpdatedTime = serverTableTimeStamp.LastUpdatedTime;
                    }
                }
            }
            else if (File.Exists(localXmlFilePath))
            {
                resultList = XmlFileUtility.DeserializeXmlFileToObj(localXmlFilePath, typeof(ObservableCollection<MDLSM_Parameter>)) as ObservableCollection<MDLSM_Parameter>;
            }

            //将查询结果数据保存到字典中
            if (resultList != null)
            {
                foreach (MDLSM_Parameter para in resultList)
                {
                    StringBuilder strKey = new StringBuilder();
                    if (!string.IsNullOrEmpty(para.Para_Code1))
                    {
                        strKey.Append("S" + para.Para_Code1);
                    }
                    if (!string.IsNullOrEmpty(para.Para_Code2))
                    {
                        strKey.Append("-" + para.Para_Code2);
                    }
                    if (!string.IsNullOrEmpty(para.Para_Code3))
                    {
                        strKey.Append("-" + para.Para_Code3);
                    }
                    if (!string.IsNullOrEmpty(para.Para_Code4))
                    {
                        strKey.Append("-" + para.Para_Code4);
                    }
                    if (!string.IsNullOrEmpty(para.Para_Code5))
                    {
                        strKey.Append("-" + para.Para_Code5);
                    }

                    //枚举Key存在的场合：追加数据到枚举值列表
                    if (HttpRuntime.Cache.Get(strKey.ToString()) != null)
                    {
                        //获取已有枚举值列表
                        var valueList = HttpRuntime.Cache.Get(strKey.ToString()) as List<MDLSM_Parameter>;
                        if (valueList != null) valueList.Add(para);
                    }
                    //枚举Key不存在的场合：新增枚举值
                    else
                    {
                        //创建枚举值对象
                        List<MDLSM_Parameter> valueList = new List<MDLSM_Parameter> { para };
                        HttpRuntime.Cache.Insert(strKey.ToString(), valueList);
                    }
                }
            }

            #region 存储配置参数

            #endregion

            //记录日志
            LogHelper.WriteBussLog(BussID, LoginInfoDAX.UserName, "CacheDAX.InitializeCache", "【End】", string.Empty, null);
        }

        ///// <summary>
        ///// 初始化用户操作习惯
        ///// </summary>
        //public static void InitUserPractice(string paramUserID)
        //{
        //    //记录日志
        //    LogHelper.WriteBussLog(BussID, LoginInfoDAX.UserName, "CacheDAX.InitializeUserPractice", "【Start】", string.Empty, null);

        //    string tableName = LocalConfigFileConst.TableName.SM_ListStyle;
        //    string localXmlFilePath = LocalConfigFileConst.ConfigFilePath.SM_ListStyle.Replace(SysConst.PH_USERID, paramUserID);
        //    var serverTableTimeStamp = SystemConfigInfo.DBTimeStampList.Find(table => table.TableName.Equals(tableName));
        //    var localTableTimeStamp = LocalConfigInfo.DBTimeStampList.Find(table => table.TableName.Equals(tableName));

        //    var resultList = new ObservableCollection<MDLSM_ListStyle>();

        //    if (!File.Exists(localXmlFilePath)
        //        || localTableTimeStamp == null
        //        || serverTableTimeStamp == null
        //        || localTableTimeStamp.LastUpdatedTime < serverTableTimeStamp.LastUpdatedTime
        //        )
        //    {
        //        //创建数据CRUD对象
        //        CRUD crud = new CRUD(DBCONFIG.Coeus);
        //        MDLSM_ListStyle argsCondition = new MDLSM_ListStyle
        //        {
        //            LS_UserID = LoginInfoDAX.UserID,
        //            LS_IsValid = true
        //        };
        //        //查询系统所有枚举数据
        //        crud.QueryForList(argsCondition, resultList);

        //        XmlFileUtility.SerializeToXmlFile(localXmlFilePath, typeof(ObservableCollection<MDLSM_ListStyle>), resultList);
        //        if (localTableTimeStamp == null)
        //        {
        //            if (serverTableTimeStamp != null)
        //            {
        //                localTableTimeStamp = new DBTableTimeStampUIModel { TableName = tableName, LastUpdatedTime = serverTableTimeStamp.LastUpdatedTime };
        //                LocalConfigInfo.DBTimeStampList.Add(localTableTimeStamp);
        //            }
        //        }
        //        else
        //        {
        //            if (serverTableTimeStamp != null)
        //            {
        //                localTableTimeStamp.LastUpdatedTime = serverTableTimeStamp.LastUpdatedTime;
        //            }
        //        }
        //    }
        //    else if (File.Exists(localXmlFilePath))
        //    {
        //        resultList = XmlFileUtility.DeserializeXmlFileToObj(localXmlFilePath, typeof(ObservableCollection<MDLSM_ListStyle>)) as ObservableCollection<MDLSM_ListStyle>;
        //    }

        //    //将查询结果数据保存到缓存中
        //    if (resultList != null)
        //    {
        //        foreach (MDLSM_ListStyle listStyle in resultList)
        //        {
        //            StringBuilder strKey = new StringBuilder();
        //            if (!string.IsNullOrEmpty(listStyle.LS_ClassFullName))
        //            {
        //                strKey.Append(CacheType.ET1001 + SysConst.ULINE + listStyle.LS_ClassFullName);
        //            }
        //            if (!string.IsNullOrEmpty(listStyle.LS_ListControlName))
        //            {
        //                strKey.Append(SysConst.ULINE + listStyle.LS_ListControlName);
        //            }
        //            Add(strKey.ToString(), listStyle.LS_State, true);
        //        }
        //    }
        //    //记录日志
        //    LogHelper.WriteBussLog(BussID, LoginInfoDAX.UserName, "CacheDAX.InitializeUserPractice", "【End】", string.Empty, null);
        //}

        /// <summary>
        /// 初始化汽车品牌车系信息
        /// </summary>
        public static void InitVehicleBrandInspireList()
        {
            //记录日志
            LogHelper.WriteBussLog(BussID, LoginInfoDAX.UserName, "CacheDAX.InitializeCarBrandInfo", "【Start】", string.Empty, null);

            string tableName = LocalConfigFileConst.TableName.SCON_VehicleBrandInspireSumma;
            string localXmlFilePath = LocalConfigFileConst.ConfigFilePath.OtherTablePath.Replace(SysConst.PH_TABLENAME, tableName);
            var serverTableTimeStamp = SystemConfigInfo.DBTimeStampList.Find(table => table.TableName.Equals(tableName));
            var localTableTimeStamp = LocalConfigInfo.DBTimeStampList.Find(table => table.TableName.Equals(tableName));

            var resultList = new ObservableCollection<MDLBS_VehicleBrandInspireSumma>();

            if (!File.Exists(localXmlFilePath)
                || localTableTimeStamp == null
                || serverTableTimeStamp == null
                || localTableTimeStamp.LastUpdatedTime < serverTableTimeStamp.LastUpdatedTime
                )
            {
                //创建数据CRUD对象
                CRUD crud = new CRUD(DBCONFIG.Coeus);
                var tempResultList = new List<MDLBS_VehicleBrandInspireSumma>();
                var argsCondition = new MDLBS_VehicleBrandInspireSumma { VBIS_IsValid = true };
                crud.QueryForList(argsCondition, tempResultList);

                foreach (var sortedItem in tempResultList.OrderBy(x => x.VBIS_Brand).ThenBy(x => x.VBIS_Inspire).ToList())
                {
                    var newItem = new MDLBS_VehicleBrandInspireSumma();
                    _baseDax.CopyModel(sortedItem, newItem);
                    resultList.Add(newItem);
                }

                XmlFileUtility.SerializeToXmlFile(localXmlFilePath, typeof(ObservableCollection<MDLBS_VehicleBrandInspireSumma>), resultList);
                if (localTableTimeStamp == null)
                {
                    if (serverTableTimeStamp != null)
                    {
                        localTableTimeStamp = new DBTableTimeStampUIModel { TableName = tableName, LastUpdatedTime = serverTableTimeStamp.LastUpdatedTime };
                        LocalConfigInfo.DBTimeStampList.Add(localTableTimeStamp);
                    }
                }
                else
                {
                    if (serverTableTimeStamp != null)
                    {
                        localTableTimeStamp.LastUpdatedTime = serverTableTimeStamp.LastUpdatedTime;
                    }
                }
            }
            else if (File.Exists(localXmlFilePath))
            {
                resultList =
                    XmlFileUtility.DeserializeXmlFileToObj(localXmlFilePath,
                        typeof(ObservableCollection<MDLBS_VehicleBrandInspireSumma>)) as
                        ObservableCollection<MDLBS_VehicleBrandInspireSumma>;
            }

            if (resultList != null)
            {
                var brandList = resultList.GroupBy(p => new { p.VBIS_Brand, p.VBIS_BrandSpellCode })
                    .Select(g => g.First()).OrderBy(x => x.VBIS_Brand).ToList();

                var brandObsCol = new ObservableCollection<MDLBS_VehicleBrandInspireSumma>();
                foreach (MDLBS_VehicleBrandInspireSumma brandInspire in brandList)
                {
                    MDLBS_VehicleBrandInspireSumma newBrand = new MDLBS_VehicleBrandInspireSumma
                    {
                        VBIS_Brand = brandInspire.VBIS_Brand,
                        VBIS_BrandSpellCode = brandInspire.VBIS_BrandSpellCode
                    };
                    brandObsCol.Add(newBrand);
                }
                HttpRuntime.Cache.Insert(ConfigDataKey.VehicleBrand, brandObsCol);

                var inspireList = resultList.GroupBy(p => new { p.VBIS_Brand, p.VBIS_Inspire, p.VBIS_InspireSpellCode })
                    .Select(g => g.First()).OrderBy(x => x.VBIS_Brand).ThenBy(x => x.VBIS_Inspire).ToList();
                var inspireObsCol = new ObservableCollection<MDLBS_VehicleBrandInspireSumma>();
                foreach (MDLBS_VehicleBrandInspireSumma brandInspire in inspireList)
                {
                    MDLBS_VehicleBrandInspireSumma newInspire = new MDLBS_VehicleBrandInspireSumma
                    {
                        VBIS_ID = brandInspire.VBIS_ID,
                        VBIS_Brand = brandInspire.VBIS_Brand,
                        VBIS_Inspire = brandInspire.VBIS_Inspire,
                        VBIS_InspireSpellCode = brandInspire.VBIS_InspireSpellCode,
                        VBIS_Model = brandInspire.VBIS_Model
                    };
                    inspireObsCol.Add(newInspire);
                }
                HttpRuntime.Cache.Insert(ConfigDataKey.VehicleBrandInspire, inspireObsCol);
            }

            //记录日志
            LogHelper.WriteBussLog(BussID, LoginInfoDAX.UserName, "CacheDAX.InitializeCarBrandInfo", "【End】", string.Empty, null);
        }

        /// <summary>
        /// 初始化系统缓存
        /// </summary>
        public static void InitializeCache()
        {
            //记录日志
            LogHelper.WriteBussLog(BussID, LoginInfoDAX.UserName, "DAXCache.InitializeCache", "【Start】", "", null);

            //定义枚举对象列表
            BindingList<MDLSM_Parameter> resultList = new BindingList<MDLSM_Parameter>();

            //创建数据CRUD对象
            var crud = new CRUD(DBCONFIG.Coeus);
            //查询系统所有枚举数据
            crud.QueryForList<MDLSM_Parameter>(new MDLSM_Parameter(), resultList);
            //初始化系统缓存
            _cache = System.Web.HttpRuntime.Cache;

            //将查询结果数据保存到字典中
            foreach (MDLSM_Parameter para in resultList)
            {
                StringBuilder strKey = new StringBuilder();
                if (!string.IsNullOrEmpty(para.Para_Code1))
                {
                    strKey.Append("S" + para.Para_Code1);
                }
                if (!string.IsNullOrEmpty(para.Para_Code2))
                {
                    strKey.Append("-" + para.Para_Code2);
                }
                if (!string.IsNullOrEmpty(para.Para_Code3))
                {
                    strKey.Append("-" + para.Para_Code3);
                }
                if (!string.IsNullOrEmpty(para.Para_Code4))
                {
                    strKey.Append("-" + para.Para_Code4);
                }
                if (!string.IsNullOrEmpty(para.Para_Code5))
                {
                    strKey.Append("-" + para.Para_Code5);
                }

                //枚举Key存在的场合：追加数据到枚举值列表
                if (_cache.Get(strKey.ToString()) != null)
                {
                    //获取已有枚举值列表
                    var valueList = _cache.Get(strKey.ToString()) as List<MDLSM_Parameter>;
                    if (valueList != null) valueList.Add(para);
                }
                //枚举Key不存在的场合：新增枚举值
                else
                {
                    //创建枚举值对象
                    var valueList = new List<MDLSM_Parameter> { para };
                    _cache.Insert(strKey.ToString(), valueList);
                }
            }
            //记录日志
            LogHelper.WriteBussLog(BussID, LoginInfoDAX.UserName, "DAXCache.InitializeCache", "【End】", "", null);
        }

        /// <summary>
        /// 添加指定项到系统缓存中【公共缓存】
        /// </summary>
        /// <param name="paramKey">用于标识该项的缓存键(Key已在EnumKey中定义)</param>
        /// <param name="paramValue">要插入缓存中的对象</param>
        /// <param name="paramIsOverWrite">若对应记录已存在，是否覆盖原记录<para>True:覆盖</para><para>False:不覆盖，返回False</para></param>
        /// <returns>ture:添加成功；false:添加失败；</returns>
        public static bool Add(CacheKey paramKey, object paramValue, bool paramIsOverWrite)
        {
            if (paramValue == null) return false;
            //若系统缓存中已经存在此标识符则退出
            if (_cache.Get(paramKey.ToString()) != null)
            {
                if (paramIsOverWrite)
                {
                    object result = _cache.Remove(paramKey.ToString());
                    if (result == null) return false;
                }
                else
                {
                    return false;
                }
            }
            _cache.Insert(paramKey.ToString(), paramValue);
            return true;
        }
        /// <summary>
        /// 添加指定项到系统缓存中【非公共缓存】
        /// </summary>
        /// <param name="paramKey">用于标识该项的缓存键(Key未在EnumKey中定义)</param>
        /// <param name="paramValue">要插入缓存中的对象</param>
        /// <param name="paramIsOverWrite">若对应记录已存在，是否覆盖原记录<para>True:覆盖</para><para>False:不覆盖，返回False</para></param>
        /// <returns>ture:添加成功；false:添加失败；</returns>
        public static bool Add(string paramKey, object paramValue, bool paramIsOverWrite)
        {
            if (paramValue == null) return false;
            //若系统缓存中已经存在此标识符则退出
            if (_cache.Get(paramKey) != null)
            {
                if (paramIsOverWrite)
                {
                    object result = _cache.Remove(paramKey);
                    if (result == null) return false;
                }
                else
                {
                    return false;
                }
            }
            _cache.Insert(paramKey, paramValue);
            return true;
        }


        /// <summary>
        /// 从系统缓存中移除指定项【公共缓存】
        /// </summary>
        /// <param name="paramKey">要移除的缓存项的标识符(Key已在EnumKey中定义)</param>
        /// <returns>ture:移除成功；false:移除失败；</returns>
        public static bool Remove(CacheKey paramKey)
        {
            object result = _cache.Remove(paramKey.ToString());
            if (result == null) return false;
            return true;
        }
        /// <summary>
        /// 从系统缓存中移除指定项【非公共缓存】
        /// </summary>
        /// <param name="paramKey">要移除的缓存项的标识符(Key未在EnumKey中定义)</param>
        /// <returns>ture:移除成功；false:移除失败；</returns>
        public static bool Remove(string paramKey)
        {
            object result = _cache.Remove(paramKey);
            if (result == null) return false;
            return true;
        }
        /// <summary>
        /// 从系统缓存获取指定项【公共缓存】
        /// <para>注意：系统参数缓存返回结果类型是【List[MDLSM_Parameter]】</para>
        /// </summary>
        /// <param name="paramKey">要获取的缓存项的标识符(Key已在EnumKey中定义)</param>
        /// <returns>null:获取失败；null以外:获取成功；</returns>
        public static object Get(CacheKey paramKey)
        {
            object result = _cache.Get(paramKey.ToString());
            return result;
        }
        /// <summary>
        /// 从系统缓存获取指定项【非公共缓存】
        /// </summary>
        /// <param name="paramKey">要获取的缓存项的标识符(Key未在EnumKey中定义)</param>
        /// <returns>null:获取失败；null以外:获取成功；</returns>
        public static object Get(string paramKey)
        {
            object result = _cache.Get(paramKey);
            return result;
        }
        /// <summary>
        /// 清除所有的缓存数据
        /// </summary>
        public static void Clear()
        {
            System.Collections.IDictionaryEnumerator idenum = _cache.GetEnumerator();
            while (idenum.MoveNext())
            {
                _cache.Remove(idenum.Key.ToString());
            }
        }

        public static string ShowAllKeyValue()
        {
            StringBuilder str = new StringBuilder();
            System.Collections.IDictionaryEnumerator idenum = _cache.GetEnumerator();
            while (idenum.MoveNext())
            {
                str.AppendFormat("[key={0},value={1}],", idenum.Key.ToString(), _cache.Get(idenum.Key.ToString()));
            }
            return str.ToString();
        }

        /// <summary>
        /// 从系统缓存获取指定项本身【非公共缓存】
        /// </summary>
        /// <param name="paramKey">要获取的缓存项的标识符(Key未在EnumKey中定义)</param>
        /// <returns>null:获取失败；null以外:获取成功；</returns>
        public static object GetItself(string paramKey)
        {
            object result = HttpRuntime.Cache.Get(paramKey);
            return result;
        }

        #endregion

        #region 初始化常用列表

        /// <summary>
        /// 初始化系统用户
        /// </summary>
        public static void InitSystemUser()
        {
            var funcName = "InitSystemUser";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            //查询所有有效的系统用户
            List<MDLSM_User> resultUserList = new List<MDLSM_User>();
            _baseDax.QueryForList(new MDLSM_User()
            {
                WHERE_User_IsValid = true,
            }, resultUserList);
            if (resultUserList.Count > 0)
            {
                resultUserList = resultUserList.OrderBy(x => x.User_Name).ThenBy(x => x.User_EMPNO).ToList();
                //将系统用户保存到缓存
                CacheDAX.Add(CacheDAX.ConfigDataKey.SystemUser, resultUserList, true);
            }

            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 初始化配件名称
        /// </summary>
        public static void InitAutoPartsName()
        {
            var funcName = "InitAutoPartsName";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            //查询所有有效的配件名称
            List<MDLBS_AutoPartsName> resultAutoPartsNameList = new List<MDLBS_AutoPartsName>();
            _baseDax.QueryForList(new MDLBS_AutoPartsName()
            {
                WHERE_APN_IsValid = true,
            }, resultAutoPartsNameList);
            if (resultAutoPartsNameList.Count > 0)
            {
                resultAutoPartsNameList = resultAutoPartsNameList.OrderBy(x => x.APN_Name).ToList();
                //将配件名称保存到缓存
                CacheDAX.Add(CacheDAX.ConfigDataKey.AutoPartsName, resultAutoPartsNameList, true);
            }

            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 初始化配件类别
        /// </summary>
        public static void InitAutoPartsType()
        {
            var funcName = "InitAutoPartsType";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            //查询所有有效的配件类别
            List<MDLBS_AutoPartsType> resultAutoPartsTypeList = new List<MDLBS_AutoPartsType>();
            _baseDax.QueryForList(new MDLBS_AutoPartsType()
            {
                WHERE_APT_IsValid = true,
            }, resultAutoPartsTypeList);
            if (resultAutoPartsTypeList.Count > 0)
            {
                resultAutoPartsTypeList = resultAutoPartsTypeList.OrderBy(x => x.APT_Name).ToList();
                //将配件类别保存到缓存
                CacheDAX.Add(CacheDAX.ConfigDataKey.AutoPartsType, resultAutoPartsTypeList, true);
            }

            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 初始化配件供应商
        /// </summary>
        public static void InitAutoPartsSupplier()
        {
            var funcName = "InitAutoPartsSupplier";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            //查询所有有效的供应商
            List<MDLPIS_Supplier> resultSupplierList = new List<MDLPIS_Supplier>();
            _baseDax.QueryForList(new MDLPIS_Supplier()
            {
                WHERE_SUPP_IsValid = true,
            }, resultSupplierList);
            if (resultSupplierList.Count > 0)
            {
                resultSupplierList = resultSupplierList.OrderBy(x => x.SUPP_Name).ToList();
                //将供应商保存到缓存
                CacheDAX.Add(CacheDAX.ConfigDataKey.AutoPartsSupplier, resultSupplierList, true);
            }

            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 初始化仓库
        /// </summary>
        public static void InitWarehouse()
        {
            var funcName = "InitWarehouse";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            //查询所有有效的仓库
            List<MDLPIS_Warehouse> resultWarehouseList = new List<MDLPIS_Warehouse>();
            _baseDax.QueryForList(new MDLPIS_Warehouse()
            {
                WHERE_WH_IsValid = true,
            }, resultWarehouseList);
            if (resultWarehouseList.Count > 0)
            {
                resultWarehouseList = resultWarehouseList.OrderBy(x => x.WH_Name).ToList();
                //将仓库保存到缓存
                CacheDAX.Add(CacheDAX.ConfigDataKey.Warehouse, resultWarehouseList, true);
            }

            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 初始化仓位
        /// </summary>
        public static void InitWarehouseBin()
        {
            var funcName = "InitWarehouseBin";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            //查询所有有效的仓位
            List<MDLPIS_WarehouseBin> resultWarehouseBinList = new List<MDLPIS_WarehouseBin>();
            _baseDax.QueryForList(new MDLPIS_WarehouseBin()
            {
                WHERE_WHB_IsValid = true,
            }, resultWarehouseBinList);
            if (resultWarehouseBinList.Count > 0)
            {
                resultWarehouseBinList = resultWarehouseBinList.OrderBy(x => x.WHB_Name).ToList();
                //将仓位保存到缓存
                CacheDAX.Add(CacheDAX.ConfigDataKey.WarehouseBin, resultWarehouseBinList, true);
            }

            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 初始化车辆品牌、车系、车型描述
        /// </summary>
        public static void InitVehicleBrand()
        {
            var funcName = "InitVehicleBrand";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            //查询所有有效的车辆品牌车系
            List<MDLBS_VehicleBrandInspireSumma> resultVehicleBrandAndInspireList = new List<MDLBS_VehicleBrandInspireSumma>();
            _baseDax.QueryForList(new MDLBS_VehicleBrandInspireSumma()
            {
                WHERE_VBIS_IsValid = true,
            }, resultVehicleBrandAndInspireList);
            //品牌
            List<MDLBS_VehicleBrandInspireSumma> resultVehicleBrandList = new List<MDLBS_VehicleBrandInspireSumma>();
            _baseDax.CopyModelList(resultVehicleBrandAndInspireList, resultVehicleBrandList);
            //车系
            List<MDLBS_VehicleBrandInspireSumma> resultVehicleInspireList = new List<MDLBS_VehicleBrandInspireSumma>();
            _baseDax.CopyModelList(resultVehicleBrandAndInspireList, resultVehicleInspireList);
            //车型描述
            List<MDLBS_VehicleBrandInspireSumma> resultVehicleModelDescList = new List<MDLBS_VehicleBrandInspireSumma>();
            _baseDax.CopyModelList(resultVehicleBrandAndInspireList, resultVehicleModelDescList);

            if (resultVehicleBrandList.Count > 0)
            {
                resultVehicleBrandList = resultVehicleBrandList.GroupBy(p => new { p.VBIS_Brand, p.VBIS_BrandSpellCode })
                    .Select(g => g.First()).OrderBy(x => x.VBIS_Brand).ToList();
                //将车辆品牌保存到缓存
                CacheDAX.Add(CacheDAX.ConfigDataKey.VehicleBrand, resultVehicleBrandList, true);
            }
            if (resultVehicleInspireList.Count > 0)
            {
                resultVehicleInspireList = resultVehicleInspireList.GroupBy(p => new { p.VBIS_Brand, p.VBIS_Inspire, p.VBIS_InspireSpellCode })
                    .Select(g => g.First()).OrderBy(x => x.VBIS_Brand).ThenBy(x => x.VBIS_Inspire).ToList();
                //将车辆车系保存到缓存
                CacheDAX.Add(CacheDAX.ConfigDataKey.VehicleBrandInspire, resultVehicleInspireList, true);
            }
            if (resultVehicleModelDescList.Count > 0)
            {
                resultVehicleModelDescList = resultVehicleModelDescList.GroupBy(p => new { p.VBIS_Brand, p.VBIS_Inspire, p.VBIS_ModelDesc })
                    .Select(g => g.First()).OrderBy(x => x.VBIS_Brand).ThenBy(x => x.VBIS_Inspire).ThenBy(x => x.VBIS_ModelDesc).ToList();
                //将车型描述保存到缓存
                CacheDAX.Add(CacheDAX.ConfigDataKey.VehicleModelDesc, resultVehicleModelDescList, true);
            }

            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 初始化车辆车系
        /// </summary>
        public static void InitVehicleInspire()
        {
            var funcName = "InitVehicleInspire";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            ////查询所有有效的车辆车系
            //List<MDLBS_VehicleBrandInspireSumma> resultVehicleInspireList = new List<MDLBS_VehicleBrandInspireSumma>();
            //_baseDax.QueryForList(new MDLBS_VehicleBrandInspireSumma()
            //{
            //    WHERE_VBIS_IsValid = true,
            //}, resultVehicleInspireList);
            //if (resultVehicleInspireList.Count > 0)
            //{
            //    resultVehicleInspireList = resultVehicleInspireList.GroupBy(p => new { p.VBIS_Brand, p.VBIS_Inspire, p.VBIS_InspireSpellCode })
            //        .Select(g => g.First()).OrderBy(x => x.VBIS_Brand).ThenBy(x => x.VBIS_Inspire).ToList();
            //    //将车辆车系保存到缓存
            //    CacheDAX.Add(CacheDAX.ConfigDataKey.VehicleBrandInspire, resultVehicleInspireList, true);
            //}

            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 初始化客户
        /// </summary>
        public static void InitCustomer()
        {
            var funcName = "InitCustomer";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            //查询所有有效的客户
            List<CustomerQueryUIModel> resultCustomerList = new List<CustomerQueryUIModel>();
            _baseDax.QueryForList(SQLID.COMM_SQL11, new CustomerQueryQCModel
            {
                //组织ID
                WHERE_OrgID = LoginInfoDAX.UserID == SysConst.SUPER_ADMIN ? null : LoginInfoDAX.OrgID,
            }, resultCustomerList);
            if (resultCustomerList.Count > 0)
            {
                List<MDLSM_Organization> allVenusOrgList = new List<MDLSM_Organization>();
                List<string> autoFactoryCodeList = resultCustomerList.Where(x => x.CustomerType == CustomerTypeEnum.Name.PTNQXSH && !string.IsNullOrEmpty(x.AutoFactoryCode)).Select(x => x.AutoFactoryCode).Distinct().ToList();
                foreach (var loopAutoFactoryCode in autoFactoryCodeList)
                {
                    //根据指定的汽修商户数据库信息获取Venus组织列表
                    List<MDLSM_Organization> tempVenusOrgList = new List<MDLSM_Organization>();
                    BLLCom.QueryAutoFactoryCustomerOrgList(loopAutoFactoryCode, tempVenusOrgList);

                    allVenusOrgList.AddRange(tempVenusOrgList);
                }

                foreach (var loopCustomer in resultCustomerList)
                {
                    if (loopCustomer.CustomerType != CustomerTypeEnum.Name.PTNQXSH)
                    {
                        continue;
                    }
                    foreach (var loopVenusOrg in allVenusOrgList)
                    {
                        if (loopCustomer.AutoFactoryOrgCode == loopVenusOrg.Org_Code
                            && !string.IsNullOrEmpty(loopVenusOrg.Org_ShortName)
                            && !string.IsNullOrEmpty(loopVenusOrg.Org_ID))
                        {
                            loopCustomer.AutoFactoryOrgInfo = loopCustomer.CustomerID + SysConst.Semicolon_DBC +
                                                              loopVenusOrg.Org_ID + SysConst.Semicolon_DBC +
                                                              loopCustomer.AutoFactoryOrgCode;
                        }
                    }
                }
                //将客户保存到缓存
                CacheDAX.Add(CacheDAX.ConfigDataKey.Customer, resultCustomerList, true);
            }

            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        #endregion
    }
}
