using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Common.Utility;

namespace SkyCar.Coeus.BLL.Common
{
    /// <summary>
    /// 码表帮助类
    /// </summary>
    public class CodeTableHelp
    {
        #region 变量定义
        /// <summary>
        /// 业务模块
        /// </summary>
        public const string BussID = Trans.COM;

        /// <summary>
        /// 码表
        /// </summary>
        //private static Dictionary<CodeType, ObservableCollection<MDLSCON_CodeTable>> _codeTableDic = new Dictionary<CodeType, ObservableCollection<MDLSCON_CodeTable>>();

        private static Dictionary<CodeType, ObservableCollection<CodeTableValueTextModel>> _codeTableValueTextDic = new Dictionary<CodeType, ObservableCollection<CodeTableValueTextModel>>();
        #endregion

        #region 自定义方法
        /// <summary>
        /// 初始化系统码表
        /// </summary>
        public static void InitializeCode()
        {
            //记录日志
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), String.Empty, String.Empty, null);

            //定义码表对象列表
            ObservableCollection<MDLSM_CodeTable> resultList = new ObservableCollection<MDLSM_CodeTable>();

            string tableName = LocalConfigFileConst.TableName.SM_CodeTable;
            string localXmlFilePath = LocalConfigFileConst.ConfigFilePath.OtherTablePath.Replace(SysConst.PH_TABLENAME, tableName);

            SystemConfigInfo.DBTimeStampList = new List<DBTableTimeStampUIModel>();
            LocalConfigInfo.DBTimeStampList = new List<DBTableTimeStampUIModel>();

            DBTableTimeStampUIModel serverTableTimeStamp = SystemConfigInfo.DBTimeStampList.Find(table => table.TableName.Equals(tableName));
            DBTableTimeStampUIModel localTableTimeStamp = LocalConfigInfo.DBTimeStampList.Find(table => table.TableName.Equals(tableName));
            if (!File.Exists(localXmlFilePath)
                || localTableTimeStamp == null
                || serverTableTimeStamp == null
                || localTableTimeStamp.LastUpdatedTime < serverTableTimeStamp.LastUpdatedTime
                )
            {
                //创建数据CRUD对象
                CRUD crud = new CRUD(DBCONFIG.Coeus);
                //查询系统所有码表数据
                MDLSM_CodeTable argsCondition = new MDLSM_CodeTable { CT_IsValid = true };
                crud.QueryForList<MDLSM_CodeTable>(argsCondition, resultList);

                XmlFileUtility.SerializeToXmlFile(localXmlFilePath, typeof(ObservableCollection<MDLSM_CodeTable>), resultList);
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
                resultList = XmlFileUtility.DeserializeXmlFileToObj(localXmlFilePath, typeof(ObservableCollection<MDLSM_CodeTable>)) as ObservableCollection<MDLSM_CodeTable>;
            }

            //初始化字典
            //_codeTableDic = new Dictionary<CodeType, ObservableCollection<MDLSCON_CodeTable>>();
            _codeTableValueTextDic = new Dictionary<CodeType, ObservableCollection<CodeTableValueTextModel>>();

            //将查询结果数据保存到字典中
            foreach (MDLSM_CodeTable ctb in resultList)
            {
                try
                {
                    //将字符串转换成对应的码表类型
                    CodeType cType = (CodeType)Enum.Parse(typeof(CodeType), ctb.CT_Type, true);
                    ////码表Key存在的场合：追加数据到码表值列表
                    //if (_codeTableDic.ContainsKey(cType))
                    //{
                    //    //获取已有码表值列表
                    //    var valueList = _codeTableDic[cType];
                    //    valueList.Add(ctb);
                    //}
                    ////码表Key不存在的场合：新增码表值
                    //else
                    //{
                    //    //创建码表值对象
                    //    var valueList = new ObservableCollection<MDLSCON_CodeTable> { ctb };
                    //    if (!_codeTableDic.ContainsKey(cType))
                    //    {
                    //        _codeTableDic.Add(cType, valueList);
                    //    }
                    //}

                    if (_codeTableValueTextDic.ContainsKey(cType))
                    {
                        var valueList = _codeTableValueTextDic[cType];
                        valueList.Add(new CodeTableValueTextModel
                        {
                            Value = ctb.CT_Value,
                            Text = ctb.CT_Desc
                        });
                    }
                    else
                    {
                        var valueList = new ObservableCollection<CodeTableValueTextModel> { new CodeTableValueTextModel {
                             Value = ctb.CT_Value,
                             Text = ctb.CT_Desc
                        } };
                        _codeTableValueTextDic.Add(cType, valueList);
                    }
                }
                catch (ArgumentException)
                {
                    continue;
                }
                catch (Exception)
                {
                    throw;
                }

            }
            //记录日志
            LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), String.Empty, String.Empty, null);
        }
        /// <summary>
        /// 根据码表编码，获取对应【码表信息】的列表
        /// </summary>
        /// <param name="paramCodeType"></param>
        /// <returns></returns>
        public static ObservableCollection<CodeTableValueTextModel> GetEnum(CodeType paramCodeType)
        {
            if (_codeTableValueTextDic.ContainsKey(paramCodeType))
            {
                return _codeTableValueTextDic[paramCodeType];
            }
            else
            {
                return new ObservableCollection<CodeTableValueTextModel>();
            }
        }
        /// <summary>
        /// 根据码表编码，获取对应【码值】和【描述】的列表
        /// </summary>
        /// <param name="paramCodeType"></param>
        /// <returns></returns>
        public static ObservableCollection<CodeTableValueTextModel> GetEnumForComboBoxWithValueText(CodeType paramCodeType)
        {
            if (_codeTableValueTextDic.ContainsKey(paramCodeType))
            {
                return _codeTableValueTextDic[paramCodeType];
            }
            else
            {
                return new ObservableCollection<CodeTableValueTextModel>();
            }
            //List<CodeTableValueTextModel> resultList = new List<CodeTableValueTextModel>();
            //foreach (MDLSCON_CodeTable ctb in _codeTableDic[paramCodeType])
            //{
            //    CodeTableValueTextModel ds = new CodeTableValueTextModel
            //    {
            //        Value = ctb.CT_Value,
            //        Text = ctb.CT_Desc
            //    };
            //    resultList.Add(ds);
            //}
        }

        /// <summary>
        /// 同步码表
        /// </summary>
        /// <param name="paramDataRowID"></param>
        /// <param name="paramSyncAction"></param>
        public static void SyncCodeTable(string paramDataRowID, SyncConfigDataAction paramSyncAction)
        {
            //本地列表
            ObservableCollection<MDLSM_CodeTable> localResultList = new ObservableCollection<MDLSM_CodeTable>();

            string tableName = LocalConfigFileConst.TableName.SM_CodeTable;
            string localXmlFilePath = LocalConfigFileConst.ConfigFilePath.OtherTablePath.Replace(SysConst.PH_TABLENAME, tableName);

            DBTableTimeStampUIModel localTableTimeStamp = LocalConfigInfo.DBTimeStampList.Find(table => table.TableName.Equals(tableName));

            if (File.Exists(localXmlFilePath))
            {
                localResultList = XmlFileUtility.DeserializeXmlFileToObj(localXmlFilePath, typeof(ObservableCollection<MDLSM_CodeTable>)) as ObservableCollection<MDLSM_CodeTable>;
            }

            if (localResultList != null)
            {
                try
                {
                    MDLSM_CodeTable existDataRow = localResultList.FirstOrDefault(localDataRow => localDataRow.CT_ID == paramDataRowID);
                    if (existDataRow != null)
                    {
                        localResultList.Remove(existDataRow);
                        var cType = (CodeType)Enum.Parse(typeof(CodeType), existDataRow.CT_Type, true);
                        if (_codeTableValueTextDic.ContainsKey(cType))
                        {
                            var tempList = _codeTableValueTextDic[cType];
                            var tempModel = tempList.FirstOrDefault(x => x.Value == existDataRow.CT_Value && x.Text == existDataRow.CT_Desc);
                            if (tempModel != null)
                            {
                                tempList.Remove(tempModel);
                            }
                        }
                    }

                    //创建数据CRUD对象
                    CRUD crud = new CRUD(DBCONFIG.Coeus);
                    //查询新的数据
                    MDLSM_CodeTable newDataRow = new MDLSM_CodeTable();
                    newDataRow = (MDLSM_CodeTable)crud.QueryForObject<MDLSM_CodeTable>(new MDLSM_CodeTable { CT_ID = paramDataRowID, CT_IsValid = true });

                    if (newDataRow != null && paramSyncAction == SyncConfigDataAction.Update)
                    {
                        localResultList.Add(newDataRow);
                        var cType = (CodeType)Enum.Parse(typeof(CodeType), newDataRow.CT_Type, true);
                        if (_codeTableValueTextDic.ContainsKey(cType))
                        {
                            var tempList = _codeTableValueTextDic[cType];
                            var tempModel = tempList.FirstOrDefault(x => x.Value == newDataRow.CT_Value && x.Text == newDataRow.CT_Desc);
                            if (tempModel == null)
                            {
                                tempList.Add(new CodeTableValueTextModel { Value = newDataRow.CT_Value, Text = newDataRow.CT_Desc });
                            }
                        }
                        else
                        {
                            _codeTableValueTextDic[cType] = new ObservableCollection<CodeTableValueTextModel>
                            {
                                new CodeTableValueTextModel { Value= newDataRow.CT_Value,Text=newDataRow.CT_Desc}
                            };
                        }
                        if (localTableTimeStamp != null && newDataRow.CT_UpdatedTime != null)
                        {
                            localTableTimeStamp.LastUpdatedTime = newDataRow.CT_UpdatedTime.Value;
                        }
                    }
                    XmlFileUtility.SerializeToXmlFile(localXmlFilePath, typeof(ObservableCollection<MDLSM_CodeTable>), localResultList);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        #endregion
    }
}
