using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.IS.UIModel;
using SkyCar.Coeus.Common.Enums;
using System.Data.SqlClient;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.DAL;
using SkyCar.Common.Utility;

namespace SkyCar.Coeus.BLL.IS
{
    /// <summary>
    /// 库存共享管理BLL
    /// </summary>
    public class AutoPartsShareInventoryManagerBLL : BLLBase
    {
        #region 全局变量
        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll = new BLLBase(Trans.IS);
        #endregion

        #region 公共属性

        #endregion

        #region 构造方法

        /// <summary>
        /// 库存共享管理BLL
        /// </summary>
        public AutoPartsShareInventoryManagerBLL() : base(Trans.IS)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramModelList">UIModel</param>
        /// <returns></returns>
        public bool SaveDetailDS(SkyCarBindingList<AutoPartsShareInventoryManagerUIModel, MDLPIS_ShareInventory> paramModelList)
        {
            //服务端检查
            if (!ServerCheck(paramModelList))
            {
                return false;
            }
            var funcName = "SaveShareInventory";
            LogHelper.WriteBussLogStart(Trans.IS, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 准备数据

            //同步到平台的数据
            List<AutoPartsShareInventoryManagerUIModel> syncShareInventoryList = new List<AutoPartsShareInventoryManagerUIModel>();

            #region 新增的数据
            ShareInventoryDataSet.InventoryDataTable insertShareInventoryDataTable = new ShareInventoryDataSet.InventoryDataTable();
            foreach (var loopInsert in paramModelList.InsertList)
            {
                loopInsert.OperateType = "Save";
                ShareInventoryDataSet.InventoryRow newInventoryRow =
                    insertShareInventoryDataTable.NewInventoryRow();
                newInventoryRow.SI_Org_ID = loopInsert.SI_Org_ID;
                newInventoryRow.SI_WH_ID = loopInsert.SI_WH_ID;
                newInventoryRow.SI_WHB_ID = loopInsert.SI_WHB_ID;
                newInventoryRow.SI_ThirdNo = loopInsert.SI_ThirdNo;
                newInventoryRow.SI_OEMNo = loopInsert.SI_OEMNo;
                newInventoryRow.SI_Barcode = loopInsert.SI_Barcode;
                newInventoryRow.SI_BatchNo = loopInsert.SI_BatchNo;
                newInventoryRow.SI_Name = loopInsert.SI_Name;
                newInventoryRow.SI_Specification = loopInsert.SI_Specification;
                newInventoryRow.SI_SUPP_ID = loopInsert.SI_SUPP_ID;
                if (loopInsert.SI_Qty != null)
                {
                    newInventoryRow.SI_Qty = (decimal)loopInsert.SI_Qty;
                }
                newInventoryRow.SI_PurchasePriceIsVisible = loopInsert.SI_PurchasePriceIsVisible == null ? false : loopInsert.SI_PurchasePriceIsVisible.Value;
                if (loopInsert.SI_PurchaseUnitPrice != null)
                {
                    newInventoryRow.SI_PurchaseUnitPrice = (decimal)loopInsert.SI_PurchaseUnitPrice;
                }
                if (loopInsert.SI_PriceOfGeneralCustomer != null)
                {
                    newInventoryRow.SI_PriceOfGeneralCustomer = (decimal)loopInsert.SI_PriceOfGeneralCustomer;
                }
                if (loopInsert.SI_PriceOfCommonAutoFactory != null)
                {
                    newInventoryRow.SI_PriceOfCommonAutoFactory = (decimal)loopInsert.SI_PriceOfCommonAutoFactory;
                }
                if (loopInsert.SI_PriceOfPlatformAutoFactory != null)
                {
                    newInventoryRow.SI_PriceOfPlatformAutoFactory = (decimal)loopInsert.SI_PriceOfPlatformAutoFactory;
                }
                newInventoryRow.SI_IsValid = loopInsert.SI_IsValid == null ? false : loopInsert.SI_IsValid.Value;
                newInventoryRow.SI_CreatedBy = loopInsert.SI_CreatedBy;
                newInventoryRow.SI_CreatedTime = BLLCom.GetCurStdDatetime();
                newInventoryRow.SI_UpdatedBy = loopInsert.SI_UpdatedBy;
                newInventoryRow.SI_UpdatedTime = BLLCom.GetCurStdDatetime();
                newInventoryRow.SI_VersionNo = "1";
                insertShareInventoryDataTable.AddInventoryRow(newInventoryRow);
            }
            syncShareInventoryList.AddRange(paramModelList.InsertList);
            #endregion

            #region 修改的数据
            ShareInventoryDataSet.InventoryDataTable updateShareInventoryDataTable = new ShareInventoryDataSet.InventoryDataTable();
            //待更新的共享库存的ID组合字符串(用于本地保存失败时，还原平台上的数据)
            string updateIdStr = string.Empty;
            foreach (var loopUpdate in paramModelList.UpdateList)
            {
                loopUpdate.OperateType = "Save";
                updateIdStr += loopUpdate.SI_ID + SysConst.Semicolon_DBC;

                ShareInventoryDataSet.InventoryRow newInventoryRow =
                    updateShareInventoryDataTable.NewInventoryRow();
                newInventoryRow.SI_ID = loopUpdate.SI_ID;
                newInventoryRow.SI_Org_ID = loopUpdate.SI_Org_ID;
                newInventoryRow.SI_WH_ID = loopUpdate.SI_WH_ID;
                newInventoryRow.SI_WHB_ID = loopUpdate.SI_WHB_ID;
                newInventoryRow.SI_ThirdNo = loopUpdate.SI_ThirdNo;
                newInventoryRow.SI_OEMNo = loopUpdate.SI_OEMNo;
                newInventoryRow.SI_Barcode = loopUpdate.SI_Barcode;
                newInventoryRow.SI_BatchNo = loopUpdate.SI_BatchNo;
                newInventoryRow.SI_Name = loopUpdate.SI_Name;
                newInventoryRow.SI_Specification = loopUpdate.SI_Specification;
                newInventoryRow.SI_SUPP_ID = loopUpdate.SI_SUPP_ID;
                if (loopUpdate.SI_Qty != null)
                {
                    newInventoryRow.SI_Qty = (decimal)loopUpdate.SI_Qty;
                }
                newInventoryRow.SI_PurchasePriceIsVisible = loopUpdate.SI_PurchasePriceIsVisible == null ? false : loopUpdate.SI_PurchasePriceIsVisible.Value;
                if (loopUpdate.SI_PurchaseUnitPrice != null)
                {
                    newInventoryRow.SI_PurchaseUnitPrice = (decimal)loopUpdate.SI_PurchaseUnitPrice;
                }
                if (loopUpdate.SI_PriceOfGeneralCustomer != null)
                {
                    newInventoryRow.SI_PriceOfGeneralCustomer = (decimal)loopUpdate.SI_PriceOfGeneralCustomer;
                }
                if (loopUpdate.SI_PriceOfCommonAutoFactory != null)
                {
                    newInventoryRow.SI_PriceOfCommonAutoFactory = (decimal)loopUpdate.SI_PriceOfCommonAutoFactory;
                }
                if (loopUpdate.SI_PriceOfPlatformAutoFactory != null)
                {
                    newInventoryRow.SI_PriceOfPlatformAutoFactory = (decimal)loopUpdate.SI_PriceOfPlatformAutoFactory;
                }
                newInventoryRow.SI_IsValid = loopUpdate.SI_IsValid == null ? false : loopUpdate.SI_IsValid.Value;
                newInventoryRow.SI_CreatedBy = loopUpdate.SI_CreatedBy;
                newInventoryRow.SI_CreatedTime = loopUpdate.SI_CreatedTime == null
                    ? BLLCom.GetCurStdDatetime()
                    : loopUpdate.SI_CreatedTime.Value;
                newInventoryRow.SI_UpdatedBy = loopUpdate.SI_UpdatedBy;
                newInventoryRow.SI_UpdatedTime = BLLCom.GetCurStdDatetime();
                newInventoryRow.SI_VersionNo = Convert.ToString(loopUpdate.SI_VersionNo + 1);
                updateShareInventoryDataTable.AddInventoryRow(newInventoryRow);
            }
            syncShareInventoryList.AddRange(paramModelList.UpdateList);
            #endregion

            #region 删除的数据
            ShareInventoryDataSet.InventoryDataTable deleteShareInventoryDataTable = new ShareInventoryDataSet.InventoryDataTable();
            foreach (var loopDelete in paramModelList.DeleteList)
            {
                loopDelete.OperateType = "Delete";
                ShareInventoryDataSet.InventoryRow newInventoryRow =
                    deleteShareInventoryDataTable.NewInventoryRow();
                newInventoryRow.SI_ID = loopDelete.SI_ID;
                newInventoryRow.SI_Org_ID = loopDelete.SI_Org_ID;
                newInventoryRow.SI_WH_ID = loopDelete.SI_WH_ID;
                newInventoryRow.SI_WHB_ID = loopDelete.SI_WHB_ID;
                newInventoryRow.SI_ThirdNo = loopDelete.SI_ThirdNo;
                newInventoryRow.SI_OEMNo = loopDelete.SI_OEMNo;
                newInventoryRow.SI_Barcode = loopDelete.SI_Barcode;
                newInventoryRow.SI_BatchNo = loopDelete.SI_BatchNo;
                newInventoryRow.SI_Name = loopDelete.SI_Name;
                newInventoryRow.SI_Specification = loopDelete.SI_Specification;
                newInventoryRow.SI_SUPP_ID = loopDelete.SI_SUPP_ID;
                if (loopDelete.SI_Qty != null)
                {
                    newInventoryRow.SI_Qty = (decimal)loopDelete.SI_Qty;
                }
                newInventoryRow.SI_PurchasePriceIsVisible = loopDelete.SI_PurchasePriceIsVisible == null ? false : loopDelete.SI_PurchasePriceIsVisible.Value;
                if (loopDelete.SI_PurchaseUnitPrice != null)
                {
                    newInventoryRow.SI_PurchaseUnitPrice = (decimal)loopDelete.SI_PurchaseUnitPrice;
                }
                if (loopDelete.SI_PriceOfGeneralCustomer != null)
                {
                    newInventoryRow.SI_PriceOfGeneralCustomer = (decimal)loopDelete.SI_PriceOfGeneralCustomer;
                }
                if (loopDelete.SI_PriceOfCommonAutoFactory != null)
                {
                    newInventoryRow.SI_PriceOfCommonAutoFactory = (decimal)loopDelete.SI_PriceOfCommonAutoFactory;
                }
                if (loopDelete.SI_PriceOfPlatformAutoFactory != null)
                {
                    newInventoryRow.SI_PriceOfPlatformAutoFactory = (decimal)loopDelete.SI_PriceOfPlatformAutoFactory;
                }
                newInventoryRow.SI_IsValid = loopDelete.SI_IsValid == null ? false : loopDelete.SI_IsValid.Value;
                newInventoryRow.SI_CreatedBy = loopDelete.SI_CreatedBy;
                newInventoryRow.SI_CreatedTime = loopDelete.SI_CreatedTime == null
                    ? BLLCom.GetCurStdDatetime()
                    : loopDelete.SI_CreatedTime.Value;
                newInventoryRow.SI_UpdatedBy = loopDelete.SI_UpdatedBy;
                newInventoryRow.SI_UpdatedTime = BLLCom.GetCurStdDatetime();
                newInventoryRow.SI_VersionNo = Convert.ToString(loopDelete.SI_VersionNo);
                deleteShareInventoryDataTable.AddInventoryRow(newInventoryRow);
            }
            syncShareInventoryList.AddRange(paramModelList.DeleteList);
            #endregion

            #endregion

            #region 保存数据

            #region 同步到平台

            if (!SynchronizeShareInventory(syncShareInventoryList))
            {
                //同步到平台失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_ShareInventory, "同步平台失败" });
                return false;
            }
            #endregion

            try
            {
                //打开数据库并连接
                using (SqlConnection mySqlConnection = new SqlConnection
                {
                    ConnectionString = DBManager.GetConnectionString(DBCONFIG.Coeus)
                })
                {
                    SqlCommand mySqlCommand = new SqlCommand();
                    mySqlCommand.Connection = mySqlConnection;
                    mySqlCommand.CommandText = "P_PIS_ShareInventory";
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.Add("@UpdateShareInventory", SqlDbType.Structured);
                    mySqlCommand.Parameters[0].Value = updateShareInventoryDataTable;
                    mySqlCommand.Parameters.Add("@InsertShareInventory", SqlDbType.Structured);
                    mySqlCommand.Parameters[1].Value = insertShareInventoryDataTable;
                    mySqlCommand.Parameters.Add("@DeleteShareInventory", SqlDbType.Structured);
                    mySqlCommand.Parameters[2].Value = deleteShareInventoryDataTable;
                    mySqlConnection.Open();
                    mySqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                #region 保存本地失败，还原同步到平台上已新增、已更新、已删除的共享库存

                List<AutoPartsShareInventoryManagerUIModel> restoreSyncShareInventoryList = new List<AutoPartsShareInventoryManagerUIModel>();
                foreach (var loopInsert in paramModelList.InsertList)
                {
                    loopInsert.OperateType = "Delete";
                }
                restoreSyncShareInventoryList.AddRange(paramModelList.InsertList);
                foreach (var loopDelete in paramModelList.DeleteList)
                {
                    loopDelete.OperateType = "Save";
                }
                restoreSyncShareInventoryList.AddRange(paramModelList.DeleteList);
                //查询待更新数据原保存数据内容
                List<MDLPIS_ShareInventory> updateShareInventoryList = new List<MDLPIS_ShareInventory>();
                _bll.QueryForList(SQLID.IS_AutoPartsShareInventoryManager_SQL02, new MDLPIS_ShareInventory
                {
                    WHERE_SI_ID = updateIdStr,
                }, updateShareInventoryList);
                foreach (var loopUpdate in paramModelList.UpdateList)
                {
                    var curUpdateShareInventory =
                        updateShareInventoryList.FirstOrDefault(x => x.SI_ID == loopUpdate.SI_ID);
                    if (curUpdateShareInventory != null && !string.IsNullOrEmpty(curUpdateShareInventory.SI_ID))
                    {
                        _bll.CopyModel(curUpdateShareInventory, loopUpdate);
                    }
                    loopUpdate.OperateType = "Save";
                }
                restoreSyncShareInventoryList.AddRange(paramModelList.UpdateList);
                SynchronizeShareInventory(restoreSyncShareInventoryList);

                #endregion

                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE, ex.Message });
                LogHelper.WriteBussLogEndOK(Trans.IS, LoginInfoDAX.UserName, funcName,
                            MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE, ex.Message }), "", null);

                return false;
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="paramModelList">UIModel</param>
        /// <returns></returns>
        public bool DeleteDetailDS(List<AutoPartsShareInventoryManagerUIModel> paramModelList)
        {
            var funcName = "DeleteDetailDS";
            LogHelper.WriteBussLogStart(Trans.IS, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 准备数据

            List<MDLPIS_ShareInventory> deleteShareInventoryList = new List<MDLPIS_ShareInventory>();
            _bll.CopyModelList(paramModelList, deleteShareInventoryList);
            foreach (var loopDeleteInventory in deleteShareInventoryList)
            {
                loopDeleteInventory.WHERE_SI_ID = loopDeleteInventory.SI_ID;
            }

            deleteShareInventoryList = deleteShareInventoryList.Where(x => !string.IsNullOrEmpty(x.WHERE_SI_ID)).ToList();
            #endregion

            #region 同步到平台

            if (!SynchronizeShareInventory(paramModelList))
            {
                //同步到平台失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.PIS_ShareInventory, "同步平台失败" });
                return false;
            }
            #endregion

            #region 删除数据
            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 删除共享库存
                var deleteShareInventoryResult = _bll.DeleteByList<MDLPIS_ShareInventory, MDLPIS_ShareInventory>(deleteShareInventoryList);
                if (!deleteShareInventoryResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);

                    #region 保存本地失败，还原同步到平台上已删除的共享库存

                    List<AutoPartsShareInventoryManagerUIModel> restoreSyncShareInventoryList = new List<AutoPartsShareInventoryManagerUIModel>();
                    foreach (var loopInsert in paramModelList)
                    {
                        loopInsert.OperateType = "Save";
                    }
                    restoreSyncShareInventoryList.AddRange(paramModelList);
                    SynchronizeShareInventory(restoreSyncShareInventoryList);

                    #endregion

                    //删除[共享库存]信息失败
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.PIS_ShareInventory });
                    return false;
                }
                #endregion

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);

                #region 保存本地失败，还原同步到平台上已删除的共享库存

                List<AutoPartsShareInventoryManagerUIModel> restoreSyncShareInventoryList = new List<AutoPartsShareInventoryManagerUIModel>();
                foreach (var loopInsert in paramModelList)
                {
                    loopInsert.OperateType = "Save";
                }
                restoreSyncShareInventoryList.AddRange(paramModelList);
                SynchronizeShareInventory(restoreSyncShareInventoryList);

                #endregion

                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.DELETE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                    ex.Message, "", null);

                return false;
            }

            #endregion

            return true;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 服务端检查
        /// </summary>
        /// <param name="paramModelList">UIModel</param>
        /// <returns></returns>
        private bool ServerCheck(SkyCarBindingList<AutoPartsShareInventoryManagerUIModel, MDLPIS_ShareInventory> paramModelList)
        {
            return true;
        }

        /// <summary>
        /// 同步共享库存到平台
        /// </summary>
        /// <returns></returns>
        private bool SynchronizeShareInventory(List<AutoPartsShareInventoryManagerUIModel> paramShareInventoryList)
        {
            var funcName = "SynchronizeShareInventory";
            LogHelper.WriteBussLogStart(Trans.IS, LoginInfoDAX.UserName, funcName, "", "", null);

            if (paramShareInventoryList.Count > 0)
            {
                var jsonShareInventoryList = (JValue)JsonConvert.SerializeObject(paramShareInventoryList);
                string argsPostData = string.Format(ApiParameter.BF0040,
                        ConfigurationManager.AppSettings[AppSettingKey.MCT_CODE],
                        LoginInfoDAX.OrgCode,
                        jsonShareInventoryList);
                string strApiData = APIDataHelper.GetAPIData(ApiUrl.BF0040Url, argsPostData);
                //本地调试：
                //string strApiData = APIDataHelper.GetAPIData("http://localhost:61860//API/BF0040", argsPostData);
                var jsonResult = (JObject)JsonConvert.DeserializeObject(strApiData);
                if (jsonResult == null || jsonResult[SysConst.EN_RESULTCODE].ToString() != SysConst.EN_I0001)
                {
                    var strErrorMessage = jsonResult == null ? "" : jsonResult[SysConst.EN_RESULTCODE].ToString();
                    LogHelper.WriteBussLogEndOK(Trans.IS, LoginInfoDAX.UserName, funcName, strErrorMessage, "", null);
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
