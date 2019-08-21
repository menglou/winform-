using System;
using System.Collections.Generic;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.SD;
using SkyCar.Coeus.UIModel.SD.UIModel;

namespace SkyCar.Coeus.BLL.SD
{
    /// <summary>
    /// 主动销售模板管理BLL
    /// </summary>
    public class SalesTemplateManagerBLL : BLLBase
    {
        #region 全局变量
        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll = new BLLBase(Trans.SD);
        #endregion

        #region 公共属性

        #endregion

        #region 构造方法

        /// <summary>
        /// 主动销售模板管理BLL
        /// </summary>
        public SalesTemplateManagerBLL() : base(Trans.SD)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramHead">单头</param>
        /// <param name="paramDetailList">明细列表</param>
        /// <param name="paramDistributePathList">下发路径</param>
        /// <returns></returns>
        public bool SaveDetailDs(SalesTemplateManagerUIModel paramHead,
            SkyCarBindingList<SalesTemplateDetailUIModel, MDLSD_SalesTemplateDetail> paramDetailList,
            SkyCarBindingList<DistributePathUIModel, MDLSD_DistributePath> paramDistributePathList)
        {
            var funcName = "SaveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            //服务端检查
            if (!ServerCheck(paramHead))
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }

            #region 准备数据

            #region 定义变量

            //待新增的目的组织的[销售模板]列表
            List<MDLSD_SalesTemplate> insertSalesTmpListOfDistributeOrg = new List<MDLSD_SalesTemplate>();
            //待新增的目的组织的[销售模板明细]列表
            List<MDLSD_SalesTemplateDetail> insertSalesTmpDetailListOfDistributeOrg = new List<MDLSD_SalesTemplateDetail>();

            //待删除的目的组织的[销售模板]列表
            List<MDLSD_SalesTemplate> deleteSalesTmpListOfDistributeOrg = new List<MDLSD_SalesTemplate>();
            //待删除的目的组织的[销售模板明细]列表
            List<MDLSD_SalesTemplateDetail> deleteSalesTmpDetailListOfDistributeOrg = new List<MDLSD_SalesTemplateDetail>();

            #endregion

            #region 保存[销售模板]
            //将UIModel转为TBModel
            var argsHead = paramHead.ToTBModelForSaveAndDelete<MDLSD_SalesTemplate>();
            //判断主键是否被赋值
            if (string.IsNullOrEmpty(paramHead.SasT_ID))
            {
                argsHead.SasT_ID = Guid.NewGuid().ToString();
                argsHead.SasT_CreatedBy = LoginInfoDAX.UserName;
                argsHead.SasT_CreatedTime = BLLCom.GetCurStdDatetime();
            }
            argsHead.SasT_UpdatedBy = LoginInfoDAX.UserName;
            argsHead.SasT_UpdatedTime = BLLCom.GetCurStdDatetime();

            #endregion

            #region 保存[销售模板明细]

            //添加的明细
            if (paramDetailList != null && paramDetailList.InsertList != null &&
                paramDetailList.InsertList.Count > 0)
            {
                foreach (var loopSalesOrderDetail in paramDetailList.InsertList)
                {
                    //赋值销售模板ID
                    loopSalesOrderDetail.SasTD_SasT_ID = argsHead.SasT_ID ?? argsHead.WHERE_SasT_ID;
                    loopSalesOrderDetail.SasTD_CreatedBy = LoginInfoDAX.UserName;
                    loopSalesOrderDetail.SasTD_CreatedTime = BLLCom.GetCurStdDatetime();
                    loopSalesOrderDetail.SasTD_UpdatedBy = LoginInfoDAX.UserName;
                    loopSalesOrderDetail.SasTD_UpdatedTime = BLLCom.GetCurStdDatetime();
                }
            }

            #endregion

            #region 下发或取消下发销售模板

            if (paramDistributePathList != null)
            {
                //下发销售模板：新增目的组织下的[销售模板]和[销售模板明细]列表
                if (paramDistributePathList.InsertList != null &&
                    paramDistributePathList.InsertList.Count > 0)
                {
                    foreach (var loopDistributePath in paramDistributePathList.InsertList)
                    {
                        #region 下发路径

                        //赋值销售模板ID
                        loopDistributePath.DP_SendDataID = !string.IsNullOrEmpty(argsHead.SasT_ID) ? argsHead.SasT_ID : argsHead.WHERE_SasT_ID;
                        loopDistributePath.DP_CreatedBy = LoginInfoDAX.UserName;
                        loopDistributePath.DP_CreatedTime = BLLCom.GetCurStdDatetime();
                        loopDistributePath.DP_UpdatedBy = LoginInfoDAX.UserName;
                        loopDistributePath.DP_UpdatedTime = BLLCom.GetCurStdDatetime();
                        #endregion

                        #region 目的组织下的[销售模板]列表

                        MDLSD_SalesTemplate insertSalesTemplate = new MDLSD_SalesTemplate();
                        _bll.CopyModel(argsHead, insertSalesTemplate);

                        insertSalesTemplate.SasT_ID = Guid.NewGuid().ToString();
                        insertSalesTemplate.SasT_Org_ID = loopDistributePath.DP_Org_ID_To;
                        insertSalesTemplate.SasT_CreatedBy = LoginInfoDAX.UserName;
                        insertSalesTemplate.SasT_CreatedTime = BLLCom.GetCurStdDatetime();
                        insertSalesTemplate.SasT_UpdatedBy = LoginInfoDAX.UserName;
                        insertSalesTemplate.SasT_UpdatedTime = BLLCom.GetCurStdDatetime();

                        insertSalesTmpListOfDistributeOrg.Add(insertSalesTemplate);
                        #endregion

                        #region 目的组织下的[销售模板明细]列表

                        if (paramDetailList != null)
                        {
                            foreach (var loopSalesTemplateDetail in paramDetailList)
                            {
                                MDLSD_SalesTemplateDetail inserSalesTemplateDetail = new MDLSD_SalesTemplateDetail();
                                _bll.CopyModel(loopSalesTemplateDetail, inserSalesTemplateDetail);

                                inserSalesTemplateDetail.SasTD_ID = Guid.NewGuid().ToString();
                                inserSalesTemplateDetail.SasTD_SasT_ID = insertSalesTemplate.SasT_ID;
                                inserSalesTemplateDetail.SasTD_CreatedBy = LoginInfoDAX.UserName;
                                inserSalesTemplateDetail.SasTD_CreatedTime = BLLCom.GetCurStdDatetime();
                                inserSalesTemplateDetail.SasTD_UpdatedBy = LoginInfoDAX.UserName;
                                inserSalesTemplateDetail.SasTD_UpdatedTime = BLLCom.GetCurStdDatetime();

                                insertSalesTmpDetailListOfDistributeOrg.Add(inserSalesTemplateDetail);
                            }
                        }

                        #endregion
                    }
                }

                //取消下发销售模板：删除目的组织下的[销售模板]和[销售模板明细]列表
                if (paramDistributePathList.DeleteList != null &&
                    paramDistributePathList.DeleteList.Count > 0)
                {
                    foreach (var loopDistributePath in paramDistributePathList.DeleteList)
                    {
                        if (string.IsNullOrEmpty(loopDistributePath.DP_ID)
                            || string.IsNullOrEmpty(loopDistributePath.DP_Org_ID_To))
                        {
                            continue;
                        }

                        #region 目的组织下的[销售模板]

                        //查询目的组织下的[销售模板]
                        var deleteSalesTemplate = _bll.QueryForObject<MDLSD_SalesTemplate>(SQLID.SD_SalesTemplate_SQL03, new SalesTemplateManagerQCModel
                        {
                            WHERE_SasT_Name = argsHead.SasT_Name,
                            WHERE_DP_SendDataID = !string.IsNullOrEmpty(argsHead.SasT_ID) ? argsHead.SasT_ID : argsHead.WHERE_SasT_ID,
                            WHERE_DP_Org_ID_From = argsHead.SasT_Org_ID,
                            WHERE_DP_Org_ID_To = loopDistributePath.DP_Org_ID_To
                        });
                        if (deleteSalesTemplate == null
                            || string.IsNullOrEmpty(deleteSalesTemplate.SasT_ID))
                        {
                            continue;
                        }
                        deleteSalesTemplate.WHERE_SasT_ID = deleteSalesTemplate.SasT_ID;

                        deleteSalesTmpListOfDistributeOrg.Add(deleteSalesTemplate);
                        #endregion

                        #region 目的组织下的[销售模板明细]列表

                        List<MDLSD_SalesTemplateDetail> tempSalesTemplateDetailList = new List<MDLSD_SalesTemplateDetail>();
                        _bll.QueryForList<MDLSD_SalesTemplateDetail, MDLSD_SalesTemplateDetail>(new MDLSD_SalesTemplateDetail
                        {
                            WHERE_SasTD_SasT_ID = deleteSalesTemplate.SasT_ID,
                            WHERE_SasTD_IsValid = true
                        }, tempSalesTemplateDetailList);

                        tempSalesTemplateDetailList.ForEach(x => x.WHERE_SasTD_ID = x.SasTD_ID);

                        deleteSalesTmpDetailListOfDistributeOrg.AddRange(tempSalesTemplateDetailList);

                        #endregion
                    }
                }
            }
            #endregion

            #endregion

            #region 带事务的保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存[销售模板]

                //执行保存
                if (!_bll.Save(argsHead, argsHead.SasT_ID))
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.SD_SalesTemplate });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                #region 保存[销售模板明细]

                if (paramDetailList != null)
                {
                    //执行保存
                    if (!_bll.UnitySave(paramDetailList))
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.SD_SalesTemplateDetail });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }

                #endregion

                #region 保存[下发路径]

                if (paramDistributePathList != null)
                {
                    //执行保存
                    if (!_bll.UnitySave(paramDistributePathList))
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.SD_DistributePath });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 新增目的组织下的[销售模板]

                if (insertSalesTmpListOfDistributeOrg.Count > 0)
                {
                    var insertSalesTmpListOfDistributeOrgResult = _bll.InsertByList<MDLSD_SalesTemplate, MDLSD_SalesTemplate>(insertSalesTmpListOfDistributeOrg);
                    if (!insertSalesTmpListOfDistributeOrgResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELIVER + SystemTableEnums.Name.SD_SalesTemplate });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }

                #endregion

                #region 新增目的组织下的[销售模板明细]

                if (insertSalesTmpDetailListOfDistributeOrg.Count > 0)
                {
                    var insertSalesTmpListOfDistributeOrgResult = _bll.InsertByList<MDLSD_SalesTemplateDetail, MDLSD_SalesTemplateDetail>(insertSalesTmpDetailListOfDistributeOrg);
                    if (!insertSalesTmpListOfDistributeOrgResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELIVER + SystemTableEnums.Name.SD_SalesTemplate });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }

                #endregion

                #region 删除目的组织下的[销售模板]

                if (deleteSalesTmpListOfDistributeOrg.Count > 0)
                {
                    var deleteSalesTmpListOfDistributeOrgResult = _bll.DeleteByList<MDLSD_SalesTemplate, MDLSD_SalesTemplate>(deleteSalesTmpListOfDistributeOrg);
                    if (!deleteSalesTmpListOfDistributeOrgResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.CANCELDELIVER + SystemTableEnums.Name.SD_SalesTemplate });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }

                #endregion

                #region 删除目的组织下的[销售模板明细]

                if (deleteSalesTmpDetailListOfDistributeOrg.Count > 0)
                {
                    var deleteSalesTmpDetailListOfDistributeOrgResult = _bll.DeleteByList<MDLSD_SalesTemplateDetail, MDLSD_SalesTemplateDetail>(deleteSalesTmpDetailListOfDistributeOrg);
                    if (!deleteSalesTmpDetailListOfDistributeOrgResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.CANCELDELIVER + SystemTableEnums.Name.SD_SalesTemplate });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }

                #endregion

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ex.Message, "", null);
                return false;
            }

            #endregion

            //将最新数据回写给DetailDS
            CopyModel(argsHead, paramHead);

            //更新销售模板明细版本号
            if (paramDetailList != null)
            {
                if (paramDetailList.InsertList != null)
                {
                    foreach (var loopInsertDetail in paramDetailList.InsertList)
                    {
                        //新增时版本号为1
                        loopInsertDetail.SasTD_VersionNo = 1;
                    }
                }

                foreach (var loopUpdateDetail in paramDetailList.UpdateList)
                {
                    //更新时版本号加1
                    loopUpdateDetail.SasTD_VersionNo = loopUpdateDetail.SasTD_VersionNo + 1;
                }
            }

            //更新下发路径版本号
            if (paramDistributePathList != null)
            {
                if (paramDistributePathList.InsertList != null)
                {
                    foreach (var loopInsertDetail in paramDistributePathList.InsertList)
                    {
                        //新增时版本号为1
                        loopInsertDetail.DP_VersionNo = 1;
                    }
                }

                foreach (var loopUpdateDetail in paramDistributePathList.UpdateList)
                {
                    //更新时版本号加1
                    loopUpdateDetail.DP_VersionNo = loopUpdateDetail.DP_VersionNo + 1;
                }
            }

            return true;
        }

        /// <summary>
        /// 删除销售模板信息
        /// </summary>
        /// <param name="paramHead">单头</param>
        /// <param name="paramDetailList">明细列表</param>
        /// <param name="paramDistributePathList">下发路径</param>
        /// <returns></returns>
        public bool DeleteSalesTemplateInfo(MDLSD_SalesTemplate paramHead, List<MDLSD_SalesTemplateDetail> paramDetailList,
           List<MDLSD_DistributePath> paramDistributePathList)
        {
            var funcName = "DeleteSalesTemplateInfo";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 验证

            if (paramHead == null || string.IsNullOrEmpty(paramHead.WHERE_SasT_ID))
            {
                //没有获取到销售模板，删除失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.SD_SalesTemplate, SystemActionEnum.Name.DELETE });
                LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);
                return false;
            }
            #endregion

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 删除销售模板

                var deleteSalesTemplate = _bll.Delete<MDLSD_SalesTemplate>(paramHead);
                if (!deleteSalesTemplate)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.SD_SalesTemplate });
                    LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);
                    return false;
                }
                #endregion

                #region 删除销售模板明细

                if (paramDetailList != null && paramDetailList.Count > 0)
                {
                    var deleteSalesTemplateDetailResult = _bll.DeleteByList<MDLSD_SalesTemplateDetail, MDLSD_SalesTemplateDetail>(paramDetailList);
                    if (!deleteSalesTemplateDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.SD_SalesTemplateDetail });
                        LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);
                        return false;
                    }
                }
                #endregion

                #region 删除下发记录

                if (paramDistributePathList != null && paramDistributePathList.Count > 0)
                {
                    var deleteDistributePathListResult = _bll.DeleteByList<MDLSD_DistributePath, MDLSD_DistributePath>(paramDistributePathList);
                    if (!deleteDistributePathListResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.SD_DistributePath });
                        LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);
                        return false;
                    }
                }
                #endregion

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.DELETE, ex.Message });
                LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="paramHead">单头</param>
        /// <param name="paramDetailList">明细列表</param>
        /// <returns></returns>
        public bool ApproveDetailDS(SalesTemplateManagerUIModel paramHead, SkyCarBindingList<SalesTemplateDetailUIModel, MDLSD_SalesTemplateDetail> paramDetailList)
        {
            var funcName = "ApproveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            if (paramHead == null)
            {
                //没有获取到销售模板，审核失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.SD_SalesTemplate, SystemActionEnum.Name.APPROVE });
                return false;
            }

            #region 准备数据

            #region 更新[主动销售模板]

            //更新主动销售模板[审核状态]为[已审核]
            paramHead.SasT_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            paramHead.SasT_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            paramHead.SasT_UpdatedBy = LoginInfoDAX.UserName;
            paramHead.SasT_UpdatedTime = BLLCom.GetCurStdDatetime();
            //将UIModel转为TBModel
            var updateHead = paramHead.ToTBModelForSaveAndDelete<MDLSD_SalesTemplate>();

            #endregion

            #endregion

            #region 带事务的保存
            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存[主动销售模板]

                bool updateSalesTemplate = _bll.Save(updateHead);
                if (!updateSalesTemplate)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.SD_SalesTemplate });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                #region 保存[主动销售模板明细]

                var saveSalesTemplateDetailResult = _bll.UnitySave(paramDetailList);
                if (!saveSalesTemplateDetailResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.SD_SalesTemplateDetail });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.APPROVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }
            #endregion

            //将最新数据回写给DetailDS
            CopyModel(updateHead, paramHead);

            //更新版本号
            foreach (var loopDetail in paramDetailList)
            {
                if (string.IsNullOrEmpty(loopDetail.SasTD_ID))
                {
                    loopDetail.SasTD_VersionNo = 1;
                }
                else
                {
                    loopDetail.SasTD_VersionNo = loopDetail.SasTD_VersionNo + 1;
                }
            }
            return true;
        }

        /// <summary>
        /// 反审核
        /// </summary>
        /// <param name="paramHead">单头</param>
        /// <returns></returns>
        public bool UnApproveDetailDS(SalesTemplateManagerUIModel paramHead)
        {
            if (paramHead == null)
            {
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.SD_SalesTemplate, SystemActionEnum.Name.UNAPPROVE });
                return false;
            }
            MDLSD_SalesTemplate argsSalesTemplate = new MDLSD_SalesTemplate();
            CopyModel(paramHead, argsSalesTemplate);

            argsSalesTemplate.WHERE_SasT_ID = argsSalesTemplate.SasT_ID;
            argsSalesTemplate.WHERE_SasT_VersionNo = argsSalesTemplate.SasT_VersionNo;
            argsSalesTemplate.SasT_VersionNo++;
            argsSalesTemplate.SasT_ApprovalStatusCode = ApprovalStatusEnum.Code.DSH;
            argsSalesTemplate.SasT_ApprovalStatusName = ApprovalStatusEnum.Name.DSH;
            argsSalesTemplate.SasT_UpdatedBy = LoginInfoDAX.UserName;
            argsSalesTemplate.SasT_UpdatedTime = BLLCom.GetCurStdDatetime();
            bool saveSalesTemplate = _bll.Save<MDLSD_SalesTemplate>(argsSalesTemplate);

            //将最新数据回写给DetailDS
            CopyModel(argsSalesTemplate, paramHead);

            return saveSalesTemplate;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 服务端检查
        /// </summary>
        /// <param name="paramHead">UIModel</param>
        /// <returns></returns>
        private bool ServerCheck(SalesTemplateManagerUIModel paramHead)
        {
            if (paramHead == null)
            {
                //没有获取到销售模板，保存失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.SD_SalesTemplate, SystemActionEnum.Name.SAVE });
                return false;
            }

            #region 判断是否存在相同名称的销售模板
            int resultCount = _bll.QueryForObject<int>(SQLID.SD_SalesTemplate_SQL01, new MDLSD_SalesTemplate
            {
                WHERE_SasT_ID = paramHead.SasT_ID,
                WHERE_SasT_Org_ID = paramHead.SasT_Org_ID,
                WHERE_SasT_Name = paramHead.SasT_Name,
            });

            if (resultCount > 0)
            {
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0006, new object[] { MsgParam.SAME + MsgParam.OF + SystemTableColumnEnums.SD_SalesTemplate.Name.SasT_Name });
                return false;
            }
            return true;
            #endregion
        }

        #endregion
    }
}
